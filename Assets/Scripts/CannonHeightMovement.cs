using UnityEngine;

public class Cannon : MonoBehaviour
{
    [Header("Player Stats")]
    [SerializeField] private Player playerStats;

    [Header("Input Event Channel")]
    [SerializeField] private InputEventChannelSO inputEventChannel;

    [Header("Elevation Settings")]
    public float rotationSpeed;
    public float accelerationTime;
    public float decelerationTime;
    public float maxElevation;
    public float maxDepression;

    // internals
    private Transform thisTransform;
    private float angleX;
    private Vector3 currentLocalAngles;
    private float elevateDir;
    private float elevateRate;

    void Start()
    {
        Initialize();
    }

    void Initialize()
    {
        UpdateElevationStats();
        thisTransform = transform;
        currentLocalAngles = thisTransform.localEulerAngles;
        angleX = currentLocalAngles.x;

        // setup range
        maxElevation = angleX - playerStats.maxElevation;
        maxDepression = angleX + playerStats.maxDepression;
    }
    public void SetElevateDirection(float input)
    {
        elevateDir = input;
    }

    public void UpdateElevationStats()
    {
        rotationSpeed = playerStats.elevationSpeed;
        accelerationTime = playerStats.elevateAccelerationTime;
        decelerationTime = playerStats.elevateDecelerationTime;
    }

    private void OnEnable()
    {
        if (inputEventChannel != null)
            inputEventChannel.OnGunElevate += HandleGunInput;
    }

    private void OnDisable()
    {
        if (inputEventChannel != null)
            inputEventChannel.OnGunElevate -= HandleGunInput;
    }

    private void HandleGunInput(float inputY)
    {
        elevateDir = inputY;
    }

    void FixedUpdate()
    {
        if (elevateDir != 0.0f)
        {
            elevateRate = Mathf.MoveTowards(elevateRate, elevateDir, Time.fixedDeltaTime / accelerationTime);
        }
        else
        {
            elevateRate = Mathf.MoveTowards(elevateRate, 0.0f, Time.fixedDeltaTime / decelerationTime);
        }

        angleX -= rotationSpeed * elevateRate * Time.fixedDeltaTime;

        if (angleX <= maxDepression && angleX >= maxElevation)
        {
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
            }
            if (angleX < maxElevation)
            {
                angleX = maxElevation;
                currentLocalAngles.x = angleX;
                thisTransform.localEulerAngles = currentLocalAngles;
                Debug.Log("out of range");
            }
        }
    }
}
