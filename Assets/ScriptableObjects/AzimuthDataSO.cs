using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Azimuth Data", fileName = "AzimuthData")]
public class AzimuthDataSO : ScriptableObject
{
    public float currentAzimuth;
    public event Action<float> OnAzimuthUpdated;

    public void UpdateAzimuth(float azimuthMil)
    {
        currentAzimuth = azimuthMil;
        OnAzimuthUpdated?.Invoke(currentAzimuth);
    }
}
