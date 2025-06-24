using System;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ZoomControl : MonoBehaviour
{
    private float maxZoomLevel = 2f;
    private float minZoomLevel = 0f;
    private float currentZoomLevel = 0f;
    private float secondZoomMult = 3f;
    private float thirdZoomMult = 6f;
    [SerializeField] public float baseCrosshairScaleMultiplier = 3f;
    public float CrosshairScaleMultiplier;
    public float firstZoomFov = 90;

    public Tuple<float, float> firstZoomSpeed = Tuple.Create(15f, 20f);
    public Tuple<float, float> secondZoomSpeed = Tuple.Create(4.5f, 4.5f);
    public Tuple<float, float> thirdZoomSpeed = Tuple.Create(1.3f, 1.3f);

    Dictionary<float, Tuple<float, float>> ZoomLevelProperties;
    Dictionary<float, Tuple<float, float>> ZoomLevelTurretProperties;
    [SerializeField] public Player playerController;
    [SerializeField] public CinemachineVirtualCamera cinemachineVirtualCamera;
    [SerializeField] public Canvas crosshair;

    public static float GetZoomedFOV(float baseFOV, float zoomMultiplier)
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
        // set FOV and crosshair size
        crosshair.scaleFactor = ZoomLevelProperties[currentZoomLevel].Item2;
        CrosshairScaleMultiplier = ZoomLevelProperties[currentZoomLevel].Item1;
        cinemachineVirtualCamera.m_Lens.FieldOfView = CrosshairScaleMultiplier;
        // set turret speed
        playerController.horizontalRotationSpeed = ZoomLevelTurretProperties[currentZoomLevel].Item1;
        playerController.elevationSpeed = ZoomLevelTurretProperties[currentZoomLevel].Item2;
        playerController.UpdateMovementStats();

    }
    public float GetZoomLevel()
    {
        return currentZoomLevel;
    }
    void Awake()
    {
        //calculating the requried fov to achieve the right multiplier based in 1 zoom level Fov
        float secondZoomFov = GetZoomedFOV(firstZoomFov, secondZoomMult);
        float thirdZoomFov = GetZoomedFOV(firstZoomFov, thirdZoomMult);
        //zoom properties intialization item 1 is Camara FOV
        //                              item 2 is crosshair scale multiplier.
        ZoomLevelProperties = new Dictionary<float, Tuple<float, float>>
        {
            { 0f, Tuple.Create(firstZoomFov, baseCrosshairScaleMultiplier)},
            { 1f, Tuple.Create(secondZoomFov, baseCrosshairScaleMultiplier*secondZoomMult)},
            { 2f, Tuple.Create(thirdZoomFov, baseCrosshairScaleMultiplier*thirdZoomMult)}
        };
        ZoomLevelTurretProperties = new Dictionary<float, Tuple<float, float>>
        {
            { 0f, firstZoomSpeed},
            { 1f, secondZoomSpeed},
            { 2f, thirdZoomSpeed}
        };

    }
    void Start()
    {

    }
    void Update()
    {
    }
}
