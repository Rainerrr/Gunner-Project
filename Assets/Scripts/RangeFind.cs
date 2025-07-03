using UnityEngine;

public class RangeFind : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private InputEventChannelSO inputEventChannel;
    [SerializeField] private RangeDataSO rangeData;
    [SerializeField] private Camera playerCamera;

    private void Start()
    {
        // Auto-find camera if not set
        if (playerCamera == null)
        {
            playerCamera = Camera.main;
            if (playerCamera == null)
            {
                Debug.LogError("RangeFind: No player camera found with tag MainCamera.");
            }
        }
    }

    public void OnRangeRequest(float _)
    {
        if (playerCamera == null)
        {
            Debug.LogError("RangeFind: playerCamera is null, cannot measure range.");
            return;
        }

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            rangeData.UpdateRange(hit.distance, playerCamera.transform);
            Debug.DrawLine(ray.origin, hit.point, Color.red, 5f);
        }
        else
        {
            rangeData.UpdateRange(0f, playerCamera.transform);
            Debug.DrawRay(ray.origin, ray.direction * 100f, Color.gray, 5f);
        }
    }
    /// Utility method for static targets, for example in a TargetBank.
    /// Computes normalized range to a specific transform without triggering events.

    public int GetRangeToTarget(Transform targetTransform)
    {
        if (playerCamera == null)
            playerCamera = Camera.main;
        if (playerCamera == null)
        {
            Debug.LogError("RangeFind: No player camera found in GetRangeToTarget.");
            return 0;
        }

        float distance = Vector3.Distance(playerCamera.transform.position, targetTransform.position);
        return NormalizeDistance(distance);
    }

    /// Normalizes a distance based on the preset distances from RangeDataSO.
    private int NormalizeDistance(float distance)
    {
        float minDist = rangeData.minPhysicalDistance;
        float maxDist = rangeData.maxPhysicalDistance;

        if (distance < minDist || distance > maxDist)
            return 0;

        float t = (distance - minDist) / (maxDist - minDist);
        return Mathf.RoundToInt(Mathf.Lerp(
            rangeData.outputMinValue,
            rangeData.outputMaxValue,
            t
        ));
    }

}