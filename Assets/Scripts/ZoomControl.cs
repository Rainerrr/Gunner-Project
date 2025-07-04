using System;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ZoomControl : MonoBehaviour
{
    private float maxZoomLevel = 2f;
    private float minZoomLevel = 0f;
    private float currentZoomLevel = 0f;

    public float CrosshairScaleMultiplier;

    Dictionary<float, Tuple<float, float>> ZoomLevelProperties;
    Dictionary<float, ZoomSpeedPair> ZoomLevelTurretProperties;
    [SerializeField] public PlayerController playerController;
    [SerializeField] private PlayerStatsSO playerStats;
    [SerializeField] public CinemachineVirtualCamera cinemachineVirtualCamera;
    [SerializeField] public Canvas crosshair;

    private static float GetZoomedFOV(float baseFOV, float zoomMultiplier)
    {
        // Prevent division by zero or negative values
        if (zoomMultiplier <= 0f)
        {
            Debug.LogWarning("Zoom multiplier must be greater than zero.");
            return baseFOV;
        }

        float baseFOVRadians = baseFOV * Mathf.Deg2Rad;
        float zoomedFOVRadians = 2f * Mathf.Atan(Mathf.Tan(baseFOVRadians / 2f) / zoomMultiplier);
        float zoomedFOV = zoomedFOVRadians * Mathf.Rad2Deg;
        return zoomedFOV;
    }
    public void HandleZoomInput(float input)
    {
        if (input == 1f)
        {
            currentZoomLevel = Mathf.Min(currentZoomLevel + 1f, maxZoomLevel);
        }
        else if (input == -1f)
        {
            currentZoomLevel = Mathf.Max(currentZoomLevel - 1f, minZoomLevel);
        }
        Debug.Log("Zoom level x: " + currentZoomLevel);
        AdjustZoomParameters();

    }
    public float GetZoomLevel()
    {
        return currentZoomLevel;
    }
    private void AdjustZoomParameters()
    {
        // set FOV and crosshair size
        crosshair.scaleFactor = ZoomLevelProperties[currentZoomLevel].Item2;
        CrosshairScaleMultiplier = ZoomLevelProperties[currentZoomLevel].Item1;
        cinemachineVirtualCamera.m_Lens.FieldOfView = CrosshairScaleMultiplier;
        // set turret speed
        playerStats.horizontalRotationSpeed = ZoomLevelTurretProperties[currentZoomLevel].horizontal;
        playerStats.elevationSpeed = ZoomLevelTurretProperties[currentZoomLevel].vertical;
        playerController.UpdateMovementStats();

    }
    public void Init()
    {
        //calculating the requried fov to achieve the right multiplier based in 1 zoom level Fov
        float secondZoomFov = GetZoomedFOV(playerStats.firstZoomFov, playerStats.secondZoomMult);
        float thirdZoomFov = GetZoomedFOV(playerStats.firstZoomFov, playerStats.thirdZoomMult);
        //zoom properties intialization item 1 is Camera FOV
        //                              item 2 is crosshair scale multiplier.
        ZoomLevelProperties = new Dictionary<float, Tuple<float, float>>
        {
            { 0f, Tuple.Create(playerStats.firstZoomFov, playerStats.baseCrosshairScaleMultiplier)},
            { 1f, Tuple.Create(secondZoomFov, playerStats.baseCrosshairScaleMultiplier * playerStats.secondZoomMult)},
            { 2f, Tuple.Create(thirdZoomFov, playerStats.baseCrosshairScaleMultiplier * playerStats.thirdZoomMult)}
        };
        ZoomLevelTurretProperties = new Dictionary<float, ZoomSpeedPair>
        {
            { 0f, playerStats.firstZoomSpeed},
            { 1f, playerStats.secondZoomSpeed},
            { 2f, playerStats.thirdZoomSpeed}
        };
        crosshair.scaleFactor = playerStats.baseCrosshairScaleMultiplier;
        cinemachineVirtualCamera.m_Lens.FieldOfView = playerStats.firstZoomFov;
        currentZoomLevel = 0;
        AdjustZoomParameters();
    }
}
