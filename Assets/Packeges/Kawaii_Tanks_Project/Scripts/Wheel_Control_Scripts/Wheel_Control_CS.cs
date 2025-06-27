using UnityEngine;
using System.Collections;


namespace ChobiAssets.KTP
{

    public class Wheel_Control_CS : MonoBehaviour
    {
        /*
		 * This script is attached to the "MainBody" of the tank.
		 * This script controls the movement of the tank.
		 * "Wheel_Rotate_CS" script rotates the wheel referring to this variables.
         * Also, some scripts refer to this script to get the current tank speed.
		*/

        [Header("Driving settings")]
        [Tooltip("Torque added to each wheel")] public float wheelTorque = 5000.0f; // Referred to from "Wheel_Rotate_CS".
        [Tooltip("Maximum speed (meter per second)")] public float maxSpeed = 10.0f; // Referred to from "Wheel_Rotate_CS" and "SE_Control_CS".
        [Tooltip("Downforce added to the body")] public float downforce = 25000.0f;
        [Tooltip("Set the distance from the body's pivot to the ground.")] public float rayDistance = 1.0f;
        [Tooltip("Rate for ease of pivot-turning"), Range(0.0f, 1.0f)] public float pivotTurnClamp = 0.5f;
        [Tooltip("Makes it easier to turn")] public bool supportBrakeTurn = false;
        [Tooltip("Makes it easier to turn")] public float supportBrakeTurnTorque = 50000.0f;


        // Referred to from "Wheel_Rotate_CS".
        [HideInInspector] public float wheelLeftRate;
        [HideInInspector] public float wheelRightRate;

        // Referred to from "SE_Control_CS".
        [HideInInspector] public float currentVelocityMag;
        [HideInInspector] public float currentAngularVelocityMag;

        // Set by "Wheel_Control_Input_##_###_CS".
        [HideInInspector] public Vector2 moveAxis;

        // Referret to from "Wheel_Control_Input_##_###_CS".
        [HideInInspector] public bool isSelected;

        Rigidbody bodyRigidbody;
        Transform bodyTransform;

        bool parkingBrake = false;
        float stoppingTime;
        const float parkingBrakeVelocity = 0.5f;
        const float parkingBrakeLag = 0.5f;

        void Start()
        {
            bodyRigidbody = GetComponent<Rigidbody>();
            bodyTransform = transform;
        }

        void Update()
        {
            if (!isSelected) return;

            Speed_Control();

            currentVelocityMag = bodyRigidbody.velocity.magnitude;
            currentAngularVelocityMag = bodyRigidbody.angularVelocity.magnitude;
        }

        void FixedUpdate()
        {
            Control_Parking_Brake();
            Anti_Spin();
            Add_Downforce();
            Anti_Slip();

            if (supportBrakeTurn)
            {
                Support_Brake_Turn();
            }
        }

        void Speed_Control()
        {
            if (moveAxis.y < 0.0f)
            {
                moveAxis.x = -moveAxis.x;
            }

            if (moveAxis.y != 0.0f)
            {
                var clamp = Mathf.Abs(moveAxis.y);
                moveAxis.x = Mathf.Clamp(moveAxis.x, -clamp, clamp);
                wheelLeftRate = Mathf.Clamp(-moveAxis.y - moveAxis.x, -1.0f, 1.0f);
                wheelRightRate = Mathf.Clamp(moveAxis.y - moveAxis.x, -1.0f, 1.0f);
            }
            else
            {
                moveAxis.x = Mathf.Clamp(moveAxis.x, -pivotTurnClamp, pivotTurnClamp);
                wheelLeftRate = -moveAxis.x;
                wheelRightRate = -moveAxis.x;
            }
        }

        void Control_Parking_Brake()
        {
            if (moveAxis.y == 0.0f && moveAxis.x == 0.0f)
            {
                if (parkingBrake)
                {
                    if (currentVelocityMag > parkingBrakeVelocity || currentAngularVelocityMag > parkingBrakeVelocity)
                    {
                        parkingBrake = false;
                        bodyRigidbody.constraints = RigidbodyConstraints.None;
                        stoppingTime = 0.0f;
                    }
                }
                else
                {
                    if (currentVelocityMag < parkingBrakeVelocity && currentAngularVelocityMag < parkingBrakeVelocity)
                    {
                        stoppingTime += Time.fixedDeltaTime;
                        if (stoppingTime > parkingBrakeLag)
                        {
                            parkingBrake = true;
                            bodyRigidbody.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationY;
                        }
                    }
                }
            }
            else
            {
                if (parkingBrake)
                {
                    parkingBrake = false;
                    bodyRigidbody.constraints = RigidbodyConstraints.None;
                    stoppingTime = 0.0f;
                }
            }
        }

        void Anti_Spin()
        {
            if (moveAxis.x == 0.0f)
            {
                Vector3 currentAngularVelocity = bodyRigidbody.angularVelocity;
                currentAngularVelocity.y = 0.0f;
                bodyRigidbody.angularVelocity = currentAngularVelocity;
            }
        }

        void Add_Downforce()
        {
            if (bodyRigidbody != null)
            {
                bodyRigidbody.AddRelativeForce(Vector3.up * -downforce);
            }
        }

        void Anti_Slip()
        {
            var ray = new Ray(bodyTransform.position, -bodyTransform.up);
            if (Physics.Raycast(ray, rayDistance, Layer_Settings_CS.Anti_Slipping_Layer_Mask))
            {
                Vector3 currentVelocity = bodyRigidbody.velocity;
                if (moveAxis == Vector2.zero)
                {
                    currentVelocity.x *= 0.9f;
                    currentVelocity.z *= 0.9f;
                }
                else
                {
                    float sign = (moveAxis.y != 0.0f) ? Mathf.Sign(moveAxis.y) : 1.0f;
                    currentVelocity = Vector3.MoveTowards(currentVelocity, bodyTransform.forward * sign * currentVelocityMag, 32.0f * Time.fixedDeltaTime);
                }

                bodyRigidbody.velocity = currentVelocity;
            }
        }

        void Support_Brake_Turn()
        {
            bodyRigidbody.AddRelativeTorque(Vector3.up * moveAxis.x * supportBrakeTurnTorque * Mathf.Abs(moveAxis.y));
        }

        public void Pause(bool isPaused)
        {
            this.enabled = !isPaused;
        }

        public void Selected(bool selected)
        {
            this.isSelected = selected;
        }
    }
}