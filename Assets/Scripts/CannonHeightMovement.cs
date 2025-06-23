using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    public float rotationSpeed;
    public float accelerationTime;
    public float decelerationTime;
    public float maxElevation;
    public float maxDepression;
    [SerializeField] private Player playerStats;
    [SerializeField] private GameInput gameInput;
    Transform thisTransform;
    Transform turretBaseTransform;
    Transform parentTransform;
    float angleX;
    Vector3 currentLocalAngles;
    float elevateDir;
    float elevateRate;
    bool isElevating = false;
    public void UpdateElevationstats()
    {
        //get Playerstats
        rotationSpeed = playerStats.elevationSpeed;
        accelerationTime = playerStats.elevateAccelerationTime;
        decelerationTime = playerStats.elevateDecelerationTime;


    }
    void Initialize()
        {
            UpdateElevationstats();
             maxElevation = playerStats.maxElevation;
             maxDepression = playerStats.maxDepression;
             thisTransform = transform;
            turretBaseTransform = thisTransform.parent;
            currentLocalAngles = thisTransform.localEulerAngles;
            angleX = currentLocalAngles.x;
            maxElevation = angleX - maxElevation;
            maxDepression = angleX + maxDepression;
            elevateDir = gameInput.GetMovementVectorNormalized().y;
        }
    void Start()
    {
       Initialize();
    }
    
    public void ElevationAnabled()
    {
                if (elevateDir != 0.0f)
        {
            elevateRate = Mathf.MoveTowards(elevateRate, elevateDir, Time.fixedDeltaTime / accelerationTime);
        }
        else
        {
            elevateRate = Mathf.MoveTowards(elevateRate, elevateDir, Time.fixedDeltaTime / decelerationTime);
        }
        if (elevateRate == 0.0f)
        {
            isElevating = false;
        }    

            elevateDir = gameInput.GetMovementVectorNormalized().y;
            angleX -= rotationSpeed * elevateRate * Time.fixedDeltaTime;
            
        if (angleX <= maxDepression && angleX >= maxElevation)
        {
            isElevating = true;
            currentLocalAngles.x = angleX;
            thisTransform.localEulerAngles = currentLocalAngles; 
        }
        else
        {
            elevateRate = 0;
            if (angleX > maxDepression)
            {
                angleX = maxDepression;
                currentLocalAngles.x = angleX;
                thisTransform.localEulerAngles = currentLocalAngles;
                Debug.Log("out of range");
                isElevating = false;

            }
            if (angleX < maxElevation)
            {
                angleX = maxElevation;
                currentLocalAngles.x = angleX;
                thisTransform.localEulerAngles = currentLocalAngles;
                Debug.Log("out of range");
                isElevating = false;
            }

        }
    }
    void Update()
    {

    }
}

    

