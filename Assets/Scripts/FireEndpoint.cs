using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireEndpoint : MonoBehaviour
{
    [SerializeField] RangeFind rangeFind;
    [SerializeField] Transform barrel;
    public float endpointRange;
    public bool lockTrajectoryflag = false;
    public void UpdateRange(float Range)
    {
        endpointRange = Range;
    }
    public void LockTrajectory()
    {
        lockTrajectoryflag = true;
    }

    public void UnlockTrajectory()
    {
        lockTrajectoryflag = false;
    }
    void Start()
    {
        
    }
    void LateUpdate()
    {
        if (!lockTrajectoryflag)
        {
            Vector3 dir = barrel.forward;
            Vector3 targetpos = barrel.position + dir * endpointRange;
            this.transform.position = targetpos;
        }

    }
}
