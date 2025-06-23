using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Turret : MonoBehaviour

{
    [SerializeField] private Player playerStats;

        public float rotationSpeed;
        public float accelerationTime;
        public float decelerationTime;
        [SerializeField] private GameInput gameInput;
        Transform thisTransform;
        Transform parentTransform;
        float angleY;
        Vector3 currentLocalAngles;
        float turnDirection;
        float turnRate;

    [SerializeField] private AzimuthFind azimuthSystem;
    private bool isAutoRotating = false;
    private float targetAzimuthMil;
    private float azimuthTolerance = 5f; // Mils tolerance

    public void UpdateHorizontalStats()
    {
        rotationSpeed = playerStats.horizontalRotationSpeed;
        accelerationTime = playerStats.horizontalAccelerationTime;
        decelerationTime = playerStats.horizontalDecelerationTime;
    }
    void Init()
    {
        UpdateHorizontalStats();

        thisTransform = transform;
        parentTransform = thisTransform.parent;
        currentLocalAngles = thisTransform.localEulerAngles;
        angleY = currentLocalAngles.y;


    }
    public void RotateToTarget(Target target)
    {
        Transform targetTransform = target.transform;
        isAutoRotating = true;
        // Calculate azimuth to target
        targetAzimuthMil = target.azimuth;
    }
    public void HorizontalEnabled()
    {
        if (isAutoRotating)
        {
            float currentAzimuth = azimuthSystem.CalculateCamaraAzimuth(); 
            float delta = Mathf.DeltaAngle(
                currentAzimuth * (360f / 6400f),
                targetAzimuthMil * (360f / 6400f)
            );

            float dir = Mathf.Sign(delta);
            float absDelta = Mathf.Abs(delta * (6400f / 360f));

            if (absDelta < azimuthTolerance)
            {
                isAutoRotating = false;
                turnRate = 0;
                return;
            }

            turnDirection = dir;
        }
        else
        {
            turnDirection = gameInput.GetMovementVectorNormalized().x;
        }
        if (turnDirection != 0.0f)
        {
            turnRate = Mathf.MoveTowards(turnRate, turnDirection, Time.fixedDeltaTime / accelerationTime);
        }
        else
        {
            turnRate = Mathf.MoveTowards(turnRate, turnDirection, Time.fixedDeltaTime / decelerationTime);
        }
        if (turnRate == 0.0f)
        {
        }
        turnDirection = gameInput.GetMovementVectorNormalized().x;
        angleY += rotationSpeed * turnRate * Time.fixedDeltaTime;
        currentLocalAngles.y = angleY;
        thisTransform.localEulerAngles = currentLocalAngles;
    }
    void Start()
    {
        Init();
    }
    void Update()
    {
    }
}
