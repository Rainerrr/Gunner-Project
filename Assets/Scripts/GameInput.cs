using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    [SerializeField] private InputEventChannelSO inputEventChannel;
    public PlayerInputActions playerInputActions;
    public void ListenToZoom()
    {
        playerInputActions.Player.Zoom.performed += ctx =>
        {
            float input = ctx.ReadValue<float>();
            inputEventChannel.RaiseZoom(input);
        };
    }
    public void ListenToAmmo()
    {
        playerInputActions.Player.Ammo.performed += ctx =>
        {
            int input = Mathf.RoundToInt(ctx.ReadValue<float>());
            inputEventChannel.RaiseAmmoChange(input);
        };
    }
    public void ListenToLaser()
    {
        playerInputActions.Player.Laser.performed += ctx =>
        {
            float input = ctx.ReadValue<float>();
            inputEventChannel.RaiseRangeFound(input);
        };
    }
    public void ListenToFire()
    {
        playerInputActions.Player.Fire.performed += ctx =>
        {
            float input = ctx.ReadValue<float>();
            inputEventChannel.RaiseFire(input);
        };
    }
    // public Vector2 GetMovementVectorNormalized()
    // {
    //     Vector2 inputVector = playerInputActions.Player.Aim.ReadValue<Vector2>();
    //     inputVector = inputVector.normalized;
    //     return (inputVector);
    // }

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
        ListenToZoom();
        ListenToLaser();
        ListenToAmmo();
        ListenToFire();
    }
    private void Update()
    {
        Vector2 input = playerInputActions.Player.Aim.ReadValue<Vector2>();
        inputEventChannel.RaiseTurretRotate(input.x);
        inputEventChannel.RaiseGunElevate(input.y);
    }
}
