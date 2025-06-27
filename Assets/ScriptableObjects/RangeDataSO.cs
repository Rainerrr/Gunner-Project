using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Range Data", fileName = "RangeData")]
public class RangeDataSO : ScriptableObject
{
    [Header("Physical Bounds")]
    public float minPhysicalDistance;
    public float maxPhysicalDistance;

    [Header("Normalized Range Settings")]
    public int outputMinValue = 10;
    public int outputMaxValue = 5000;

    [Header("Measured State")]  // read-only at runtime
    public int currentRange;
    public bool isSrak;
    public float lastPhysicalDistance;
    public Vector3 lastLaserPoint;

    // Event to notify listeners whenever the range updates
    public event Action<int, bool, float, Vector3> OnRangeUpdated;

    /// Initializes the physical min and max distances once.
    public void InitializeBounds(float minDist, float maxDist)
    {
        minPhysicalDistance = minDist;
        maxPhysicalDistance = maxDist;
        currentRange = 0;
        isSrak = true;
        lastPhysicalDistance = 0f;
        lastLaserPoint = Vector3.zero;
    }
    

    /// Updates the range state based on the measured distance and broadcasts an event.
    public void UpdateRange(float measuredDistance, Transform cameraTransform)
    {
        lastPhysicalDistance = measuredDistance;

        if (measuredDistance < minPhysicalDistance || measuredDistance > maxPhysicalDistance)
        {
            isSrak = true;
            currentRange = 0;
            lastLaserPoint = cameraTransform.position;
        }
        else
        {
            isSrak = false;
            float t = (measuredDistance - minPhysicalDistance) / (maxPhysicalDistance - minPhysicalDistance);
            currentRange = Mathf.RoundToInt(Mathf.Lerp(outputMinValue, outputMaxValue, t));
            lastLaserPoint = cameraTransform.position + cameraTransform.forward * measuredDistance;
        }

        OnRangeUpdated?.Invoke(currentRange, isSrak, lastPhysicalDistance, lastLaserPoint);
    }
}