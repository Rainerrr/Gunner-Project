using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewTargetObjective : Objective
{
    [Header("Target Settings")]
    public string targetName;
    public TargetType type;

    [Header("Description")]
    [TextArea(2, 5)]
    public string description;
    [Header("Target position")]
    [SerializeField] private Transform targetPosition;

    [Header("toggle to press")]
    [SerializeField] private Toggle toggle;
    [SerializeField] private GameObject toggleGameObject;
    [SerializeField] private TargetBank targetBank;

    public override void Activate()
    {
        targetBank = TargetBank.Instance;
        if (targetBank != null)
            targetBank.CreateNewTarget(targetName, type, targetPosition.position, description);
        else
            Debug.LogError("NewTargetObjective: TargetBank instance not found");
        // Called by the ObjectiveManager when this objective becomes active
        enabled = true;
        
        toggle.onValueChanged.AddListener(CompleteObjective);
    }
    private void CompleteObjective(bool isON)
    {
        if (isON)
        {
            enabled = false;
            toggle.onValueChanged.RemoveListener(CompleteObjective);
            // Notify the manager via the base class
            Complete();
            Destroy(gameObject);
        }
        return;
    }
}
