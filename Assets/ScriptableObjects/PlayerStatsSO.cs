using System;
using UnityEngine;

[Serializable]
public struct ZoomSpeedPair
{
    public float horizontal;
    public float vertical;
}

[CreateAssetMenu(menuName = "Data/Player Stats", fileName = "PlayerStats")]
public class PlayerStatsSO : ScriptableObject
{
    [Header("Cannon movement settings")]
    public float elevationSpeed = 10f;
    public float elevateAccelerationTime = 0.2f;
    public float elevateDecelerationTime = 0.2f;
    public float maxElevation = 15f;
    public float maxDepression = 10f;

    [Header("Turret movement settings")]
    public float horizontalRotationSpeed = 20f;
    public float horizontalAccelerationTime = 0.05f;
    public float horizontalDecelerationTime = 0.3f;

    [Header("Zoom Settings")]
    public float baseCrosshairScaleMultiplier = 3f;
    public float firstZoomFov = 90f;
    public float secondZoomMult = 3f;
    public float thirdZoomMult = 6f;

    [Header("Zoom Speeds")]
    public ZoomSpeedPair firstZoomSpeed;
    public ZoomSpeedPair secondZoomSpeed;
    public ZoomSpeedPair thirdZoomSpeed;
}
