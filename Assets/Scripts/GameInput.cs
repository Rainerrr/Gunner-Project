using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public PlayerInputActions playerInputActions;
    public event System.Action<float> OnLaserInput;
    public event System.Action<float> OnFireInput;
    public event System.Action<float> OnZoomInput;
    public event System.Action<int> OnAmmoInput;
    public void ListenToZoom()
    {
        playerInputActions.Player.Zoom.performed += ctx =>
        {
            float input = ctx.ReadValue<float>();
            OnZoomInput?.Invoke(input);
        };
    }
        public float GetZoomInput()
    {
        float zoomInput = playerInputActions.Player.Zoom.ReadValue<float>();
        return(zoomInput);
    }
    public void ListenToAmmo()
    {
        playerInputActions.Player.Ammo.performed += ctx =>
        {
            int input = Mathf.RoundToInt(ctx.ReadValue<float>());
            OnAmmoInput?.Invoke(input);
        };
    }
    public void ListenToLaser()
    {
        playerInputActions.Player.Laser.performed += ctx =>
        {
            float input = ctx.ReadValue<float>();
            OnLaserInput?.Invoke(input);
        };
    }
    public void ListenToFire()
    {
        playerInputActions.Player.Fire.performed += ctx =>
        {
            float input = ctx.ReadValue<float>();
            OnFireInput?.Invoke(input);
        };
    }
    public Vector2 GetMovementVectorNormalized() 
    {
        Vector2 inputVector = playerInputActions.Player.Aim.ReadValue<Vector2>();
        inputVector = inputVector.normalized;
        return (inputVector);
    }
    public Vector2 GetwheelMovementVectorNormalized()
    {
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();
        inputVector = inputVector.normalized;
        return inputVector;
    }

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
        ListenToZoom();
        ListenToLaser();
        ListenToAmmo();
        ListenToFire();
    }
}
