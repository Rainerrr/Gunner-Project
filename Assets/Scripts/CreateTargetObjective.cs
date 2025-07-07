using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class CreateTargetObjective : Objective
{
    [Header("toggle to press")]
    [SerializeField] private BoxCollider Collidercheck;
    [SerializeField] private Toggle toggle;
    [SerializeField] private TargetBankSO TargetBankSO;


    public override void Activate()
    {

        toggle.onValueChanged.AddListener(ListentoLaser);
    }
    private void ListentoLaser(bool isON)
    {
        if (isON)
        {
            TargetBankSO.OnTargetAdded += CheckTargetInColider;
        }
        else
        {
            TargetBankSO.OnTargetAdded -= CheckTargetInColider;

        }
        return;
    }
    private void CheckTargetInColider(Target target)
    {
        SphereCollider targetcolider = target.GetComponent<SphereCollider>();
        TargetType targettypeNeeded = toggle.GetComponent<TargetTypeToggleInfo>().targetType;
        if (Collidercheck.bounds.Intersects(targetcolider.bounds) && targettypeNeeded == target.type)
        {
            CompleteObjective();
        }
        else
        {
            Debug.Log("target added - but not in colider");
        }
    }

    private void CompleteObjective()
    {
            enabled = false;
            toggle.onValueChanged.RemoveListener(ListentoLaser);
            TargetBankSO.OnTargetAdded -= CheckTargetInColider;
           // Notify the manager via the base class
            Complete();
            Destroy(gameObject);
        return;
    }
}
