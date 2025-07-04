using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InputEventChannelSO inputEventChannel;
    [SerializeField] private AmmoDataSO ammoDataSO;
    [SerializeField] private Turret turret;
    [SerializeField] private Cannon cannon;
    [SerializeField] private ZoomControl zoomControl;
    [SerializeField] private RangeFind rangeFind;
    [SerializeField] private AmmoControl ammoControl;
    [SerializeField] private FireEvent fireEvent;
    [Header("Stats")]
    [SerializeField] public PlayerStatsSO playerStats;

    private bool isMovementEnabled = false;

    private void Start()
    {
        // init zoom control (for FOV and crosshair)
        zoomControl.Init();
        // subscribe to input events
        EnableInputs();
    }

    private void OnDestroy()
    {
        DisableInputs();
    }

    public void EnableInputs()
    {
        inputEventChannel.OnZoom += zoomControl.HandleZoomInput;
        inputEventChannel.OnAmmoChange += ammoControl.HandleAmmoInput;
        inputEventChannel.OnTurretRotate += HandleTurretRotate;
        inputEventChannel.OnGunElevate += HandleGunElevate;

        isMovementEnabled = true;
    }

    public void DisableInputs()
    {
        inputEventChannel.OnZoom -= zoomControl.HandleZoomInput;
        inputEventChannel.OnAmmoChange -= ammoControl.HandleAmmoInput;
        inputEventChannel.OnTurretRotate -= HandleTurretRotate;
        inputEventChannel.OnGunElevate -= HandleGunElevate;

        isMovementEnabled = false;
    }

    /// Called by InputEventChannel when X-axis input is received.
    private void HandleTurretRotate(float inputX)
    {
        if (!isMovementEnabled) return;
        if (turret != null)
        {
            turret.SetTurnDirection(inputX);
        }
    }

    /// Called by InputEventChannel when Y-axis input is received.
    private void HandleGunElevate(float inputY)
    {
        if (!isMovementEnabled) return;
        if (cannon != null)
        {
            cannon.SetElevateDirection(inputY);
        }
    }

    /// Updates stats to the turret and cannon from the Player values.
    public void UpdateMovementStats()
    {
        if (turret != null) turret.UpdateHorizontalStats();
        if (cannon != null) cannon.UpdateElevationStats();
    }
}
