using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallisticCurve : MonoBehaviour
{
    [SerializeField] private Transform A;
    [SerializeField] float curveOffset = 5f;
    [SerializeField] private Transform B;
    [SerializeField] private Transform Control;

    public void UpdateControlPosition(Transform a, Transform b, float yOffset)
    {
        Vector3 midPoint = (a.position + b.position) / 2f;  // middle between a and b
        midPoint.y += yOffset;  // raise by yOffset
        Control.position = midPoint;  // set the control's position
    }
    public Vector3 evaluate(float t){
        UpdateControlPosition(A,B,curveOffset);
        Vector3 ac = Vector3.Lerp(A.position,Control.position, t);
        Vector3 cb = Vector3.Lerp(Control.position, B.position, t);
        return Vector3.Lerp(ac,cb,t);
    }
    private void OnDrawGizmos()
    {
        if (A == null || B == null || Control == null){
            return;
        }
        for (int i = 0; i < 500; i++)
        {
            Gizmos.DrawWireSphere(evaluate(i / 20f), 0.1f);
        }
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
