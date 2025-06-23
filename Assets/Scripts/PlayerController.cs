using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Cannon movement settings")]
    [Tooltip("Elevation speed. (Degree per second)")] public float elevationSpeed = 10.0f;
    [Tooltip("Time to reach the maximum speed from zero. (Sec)")] public float elevateAccelerationTime = 0.2f;
    [Tooltip("Time to stop from the maximum speed. (Sec)")] public float elevateDecelerationTime = 0.2f;
    [Tooltip("Maximum elevation angle. (Degree)")] public float maxElevation = 15.0f;
    [Tooltip("Maximum depression angle. (Degree)")] public float maxDepression = 10.0f;

    [Header("Turret movement settings")]
    [Tooltip("Maximum rotation speed. (Degree per Second)")] public float horizontalRotationSpeed = 20.0f;
    [Tooltip("Time to reach the maximum speed from zero. (Sec)")] public float horizontalAccelerationTime = 0.05f;
    [Tooltip("Time to stop from the maximum speed. (Sec)")] public float horizontalDecelerationTime = 0.3f;
    [SerializeField] private Cannon elevation;
    [SerializeField] private Turret horizontal;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private Turret turret;
    [SerializeField] private Cannon cannon;
    [SerializeField] private ZoomControl zoomControl;
    [SerializeField] private RangeFind rangeFind;
    [SerializeField] private AmmoControl ammoControl;
    [SerializeField] private FireEvent FireEvent;

    public void UpdateMovementStats()
    {
        horizontal.UpdateHorizontalStats();
        elevation.UpdateElevationstats();
    }
    public void EnableLaserInput()
    {
        if (gameInput != null)
        {
            gameInput.OnLaserInput += rangeFind.FindRange;
        }
    }
    public void DisableLaserInput()
    {
        if (gameInput != null)
        {
            gameInput.OnLaserInput -= rangeFind.FindRange;
        }
    }
    public void EnableFireInput()
    {
        if (gameInput != null)
        {
            gameInput.OnFireInput += FireEvent.Fire;
        }
    }
    public void DisableFireInput()
    {
        if (gameInput != null)
        {
            gameInput.OnFireInput -= FireEvent.Fire;
        }
    }

    public void EnableZoomInput()
    {
        if (gameInput != null)
        {
            gameInput.OnZoomInput += zoomControl.HandleZoomInput;
        }
    }
    public void DisableZoomInput()
    {
        if (gameInput != null)
        {
            gameInput.OnZoomInput -= zoomControl.HandleZoomInput;
        }
    }
    public void EnableAmmoInput()
    {
        if (gameInput != null)
        {
            gameInput.OnAmmoInput += ammoControl.HandleAmmoInput;
        }
    }
    public void DisableAmmoInput()
    {
        if (gameInput != null)
        {
            gameInput.OnAmmoInput -= ammoControl.HandleAmmoInput;
        }
    }
    private void Start()
    {
        zoomControl.crosshair.scaleFactor = zoomControl.baseCrosshairScaleMultiplier;
        zoomControl.cinemachineVirtualCamera.m_Lens.FieldOfView = zoomControl.firstZoomFov;
        EnableZoomInput();
        EnableAmmoInput();
        EnableLaserInput();
        EnableFireInput();
    }
    private void Update()
    {
        turret.HorizontalEnabled();
        cannon.ElevationAnabled();
    }
}
