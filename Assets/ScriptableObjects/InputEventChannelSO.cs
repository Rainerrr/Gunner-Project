using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/Input Event Channel", fileName = "InputEventChannel")]
public class InputEventChannelSO : ScriptableObject
{
    // Turret rotation event (yaw)
    public event Action<float> OnTurretRotate;

    // Gun elevation event (pitch)
    public event Action<float> OnGunElevate;

    // Fire Event
    public event Action<float> OnFire;

    // Range Found Event
    public event Action<float> OnRangeFound;

    // Zoom Event
    public event Action<float> OnZoom;

    // Ammo Change Event
    public event Action<int> OnAmmoChange;

    // Raise Methods
    public void RaiseFire(float input) => OnFire?.Invoke(input);

    public void RaiseRangeFound(float input) => OnRangeFound?.Invoke(input);

    public void RaiseZoom(float input) => OnZoom?.Invoke(input);

    public void RaiseAmmoChange(int input) => OnAmmoChange?.Invoke(input);

    public void RaiseTurretRotate(float input) => OnTurretRotate?.Invoke(input);

    public void RaiseGunElevate(float input) => OnGunElevate?.Invoke(input);
}