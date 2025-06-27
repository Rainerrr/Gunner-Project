using System;
using UnityEngine;

public class RangeSettingsInitializer : MonoBehaviour
{
    [SerializeField] private Transform minRangeTransform;
    [SerializeField] private Transform maxRangeTransform;
    [SerializeField] private RangeDataSO rangeData;
    [SerializeField] private Camera playerCamera;

    void Start()
    {
        // If no camera was assigned, try to find one at runtime
        if (playerCamera == null)
        {
            playerCamera = Camera.main;
            if (playerCamera == null)
            {
                Debug.LogError("RangeSettingsInitializer: No Camera assigned and no main camera found.");
                enabled = false;
                return;
            }
        }
        float minDist = Vector3.Distance(playerCamera.transform.position, minRangeTransform.position);
        float maxDist = Vector3.Distance(playerCamera.transform.position, maxRangeTransform.position);

        rangeData.InitializeBounds(minDist, maxDist);
    }
}