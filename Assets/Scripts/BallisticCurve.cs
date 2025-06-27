using UnityEngine;

public class BallisticCurve : MonoBehaviour
{
    [Header("Points")]
    [SerializeField] private Transform A;          // barrel tip
    [SerializeField] private Transform B;          // fire endpoint
    [SerializeField] private Transform Control;    // control point of curve
    [SerializeField] private Transform LastLaser;  // laser marker

    [Header("Settings")]
    [SerializeField] private float curveOffset = 5f;
    [SerializeField] private RangeDataSO rangeData;

    private bool lockTrajectoryFlag = false;

    private void OnEnable()
    {
        if (rangeData != null)
            rangeData.OnRangeUpdated += HandleRangeUpdated;
    }

    private void OnDisable()
    {
        if (rangeData != null)
            rangeData.OnRangeUpdated -= HandleRangeUpdated;
    }

    private void HandleRangeUpdated(int currentRange, bool isSrak, float lastDistance, Vector3 lastLaserPoint)
    {
        if (!lockTrajectoryFlag)
        {
            if (A != null && B != null)
            {
                // place FireEndpoint at barrel + direction * distance
                Vector3 dir = A.forward;
                B.position = A.position + dir * lastDistance;
            }
        }

        if (LastLaser != null)
        {
            // always update laser marker
            LastLaser.position = lastLaserPoint;
        }
    }

    private void LateUpdate()
    {
        // update control point dynamically to keep the curve nice
        if (A != null && B != null && Control != null)
        {
            Vector3 midPoint = (A.position + B.position) / 2f;
            midPoint.y += curveOffset;
            Control.position = midPoint;
        }
    }

    public Vector3 Evaluate(float t)
    {
        Vector3 ac = Vector3.Lerp(A.position, Control.position, t);
        Vector3 cb = Vector3.Lerp(Control.position, B.position, t);
        return Vector3.Lerp(ac, cb, t);
    }

    private void OnDrawGizmos()
    {
        if (A == null || B == null || Control == null) return;

        for (float t = 0; t <= 1; t += 0.05f)
        {
            Gizmos.DrawWireSphere(Evaluate(t), 0.1f);
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
