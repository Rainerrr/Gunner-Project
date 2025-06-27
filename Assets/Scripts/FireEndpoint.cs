using UnityEngine;

public class FireEndpoint : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RangeDataSO rangeData;
    [SerializeField] private Transform barrel;

    [Header("Debug")]
    public bool lockTrajectoryFlag = false;

    private void LateUpdate()
    {
        if (lockTrajectoryFlag) return;

        if (rangeData != null && barrel != null)
        {
            float distance = rangeData.lastPhysicalDistance;
            Vector3 dir = barrel.forward;
            transform.position = barrel.position + dir * distance;
        }
    }

    public void LockTrajectory()
    {
        lockTrajectoryFlag = true;
    }

    public void UnlockTrajectory()
    {
        lockTrajectoryFlag = false;
    }
}
