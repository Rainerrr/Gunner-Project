using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InputEventChannelSO inputEventChannel;
    [SerializeField] private Turret turret;
    [SerializeField] private Cannon cannon;
    [SerializeField] private ZoomControl zoomControl;
    [SerializeField] private RangeFind rangeFind;
    [SerializeField] private AmmoControl ammoControl;
    [SerializeField] private FireEvent fireEvent;
    [SerializeField] private ChobiAssets.KTP.Wheel_Control_CS wheelControl;

    [Header("Cannon movement settings")]
    public float elevationSpeed = 10.0f;
    public float elevateAccelerationTime = 0.2f;
    public float elevateDecelerationTime = 0.2f;
    public float maxElevation = 15.0f;
    public float maxDepression = 10.0f;

    [Header("Turret movement settings")]
    public float horizontalRotationSpeed = 20.0f;
    public float horizontalAccelerationTime = 0.05f;
    public float horizontalDecelerationTime = 0.3f;

    private bool isMovementEnabled = false;

    private void Start()
    {
        // init zoom control (for FOV and crosshair)
        zoomControl.crosshair.scaleFactor = zoomControl.baseCrosshairScaleMultiplier;
        zoomControl.cinemachineVirtualCamera.m_Lens.FieldOfView = zoomControl.firstZoomFov;

        // subscribe to input events
        EnableInputs();
    }

    private void OnDestroy()
    {
        DisableInputs();
    }

    public void EnableInputs()
    {
        inputEventChannel.OnFire += fireEvent.Fire;
        inputEventChannel.OnRangeFound += rangeFind.FindRange;
        inputEventChannel.OnZoom += zoomControl.HandleZoomInput;
        inputEventChannel.OnAmmoChange += ammoControl.HandleAmmoInput;
        inputEventChannel.OnTurretRotate += HandleTurretRotate;
        inputEventChannel.OnGunElevate += HandleGunElevate;

        isMovementEnabled = true;
    }

    public void DisableInputs()
    {
        inputEventChannel.OnFire -= fireEvent.Fire;
        inputEventChannel.OnRangeFound -= rangeFind.FindRange;
        inputEventChannel.OnZoom -= zoomControl.HandleZoomInput;
        inputEventChannel.OnAmmoChange -= ammoControl.HandleAmmoInput;
        inputEventChannel.OnTurretRotate -= HandleTurretRotate;
        inputEventChannel.OnGunElevate -= HandleGunElevate;

        isMovementEnabled = false;
    }

    /// <summary>
    /// Called by InputEventChannel when X-axis input is received.
    /// </summary>
    private void HandleTurretRotate(float inputX)
    {
        if (!isMovementEnabled) return;
        if (turret != null)
        {
            turret.SetTurnDirection(inputX);
        }
    }

    /// <summary>
    /// Called by InputEventChannel when Y-axis input is received.
    /// </summary>
    private void HandleGunElevate(float inputY)
    {
        if (!isMovementEnabled) return;
        if (cannon != null)
        {
            cannon.SetElevateDirection(inputY);
        }
    }

    /// <summary>
    /// Updates stats to the turret and cannon from the Player values.
    /// </summary>
    public void UpdateMovementStats()
    {
        if (turret != null) turret.UpdateHorizontalStats();
        if (cannon != null) cannon.UpdateElevationStats();
    }
}
