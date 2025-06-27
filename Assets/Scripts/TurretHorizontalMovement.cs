using UnityEngine;

public class Turret : MonoBehaviour
{
    [Header("Player Stats")]
    [SerializeField] private Player playerStats;

    [Header("Input Event Channel")]
    [SerializeField] private InputEventChannelSO inputEventChannel;

    [Header("Rotation Settings")]
    public float rotationSpeed;
    public float accelerationTime;
    public float decelerationTime;

    [Header("Auto Rotate")]
    [SerializeField] private AzimuthFind azimuthSystem;
    private bool isAutoRotating = false;
    private float targetAzimuthMil;
    private float azimuthTolerance = 5f;

    // internals
    private Transform thisTransform;
    private float angleY;
    private Vector3 currentLocalAngles;
    private float turnDirection;
    private float turnRate;

    void Start()
    {
        Init();
    }

    void Init()
    {
        UpdateHorizontalStats();
        thisTransform = transform;
        currentLocalAngles = thisTransform.localEulerAngles;
        angleY = currentLocalAngles.y;
    }
    public void SetTurnDirection(float input)
    {
        turnDirection = input;
    }

    public void UpdateHorizontalStats()
    {
        rotationSpeed = playerStats.horizontalRotationSpeed;
        accelerationTime = playerStats.horizontalAccelerationTime;
        decelerationTime = playerStats.horizontalDecelerationTime;
    }

    public void RotateToTarget(Target target)
    {
        isAutoRotating = true;
        targetAzimuthMil = target.azimuth;
    }

    private void OnEnable()
    {
        if (inputEventChannel != null)
            inputEventChannel.OnTurretRotate += HandleTurretInput;
    }

    private void OnDisable()
    {
        if (inputEventChannel != null)
            inputEventChannel.OnTurretRotate -= HandleTurretInput;
    }

    private void HandleTurretInput(float inputX)
    {
        turnDirection = inputX;
    }

    void FixedUpdate()
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

        // smooth turn
        if (turnDirection != 0.0f)
        {
            turnRate = Mathf.MoveTowards(turnRate, turnDirection, Time.fixedDeltaTime / accelerationTime);
        }
        else
        {
            turnRate = Mathf.MoveTowards(turnRate, 0.0f, Time.fixedDeltaTime / decelerationTime);
        }

        angleY += rotationSpeed * turnRate * Time.fixedDeltaTime;
        currentLocalAngles.y = angleY;
        thisTransform.localEulerAngles = currentLocalAngles;
    }
}
