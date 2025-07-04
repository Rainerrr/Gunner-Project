using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Zoom Data", fileName = "ZoomData")]
public class ZoomDataSO : ScriptableObject
{
    [SerializeField] private PlayerStatsSO playerStats;

    private readonly float maxZoomLevel = 2f;
    private readonly float minZoomLevel = 0f;
    private float currentZoomLevel = 0f;

    private Dictionary<float, Tuple<float, float>> zoomLevelProperties;
    private Dictionary<float, ZoomSpeedPair> zoomLevelTurretProperties;

    public event Action<float, float, ZoomSpeedPair> OnZoomChanged;

    public void Init()
    {
        float secondZoomFov = GetZoomedFOV(playerStats.firstZoomFov, playerStats.secondZoomMult);
        float thirdZoomFov = GetZoomedFOV(playerStats.firstZoomFov, playerStats.thirdZoomMult);

        zoomLevelProperties = new Dictionary<float, Tuple<float, float>>
        {
            { 0f, Tuple.Create(playerStats.firstZoomFov, playerStats.baseCrosshairScaleMultiplier)},
            { 1f, Tuple.Create(secondZoomFov, playerStats.baseCrosshairScaleMultiplier * playerStats.secondZoomMult)},
            { 2f, Tuple.Create(thirdZoomFov, playerStats.baseCrosshairScaleMultiplier * playerStats.thirdZoomMult)}
        };

        zoomLevelTurretProperties = new Dictionary<float, ZoomSpeedPair>
        {
            { 0f, playerStats.firstZoomSpeed},
            { 1f, playerStats.secondZoomSpeed},
            { 2f, playerStats.thirdZoomSpeed}
        };

        currentZoomLevel = 0f;
        
        RaiseEvent();
    }

    private static float GetZoomedFOV(float baseFOV, float zoomMultiplier)
    {
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

    public void IncreaseZoom()
    {
        currentZoomLevel = Mathf.Min(currentZoomLevel + 1f, maxZoomLevel);
        RaiseEvent();
    }

    public void DecreaseZoom()
    {
        currentZoomLevel = Mathf.Max(currentZoomLevel - 1f, minZoomLevel);
        RaiseEvent();
    }

    public float GetCurrentFOV() => zoomLevelProperties[currentZoomLevel].Item1;

    public float GetCurrentCrosshairScale() => zoomLevelProperties[currentZoomLevel].Item2;

    public ZoomSpeedPair GetCurrentZoomSpeeds() => zoomLevelTurretProperties[currentZoomLevel];

    private void RaiseEvent()
    {
        OnZoomChanged?.Invoke(GetCurrentFOV(), GetCurrentCrosshairScale(), GetCurrentZoomSpeeds());
    }
}
