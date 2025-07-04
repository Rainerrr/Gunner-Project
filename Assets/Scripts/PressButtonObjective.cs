using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PressButtonObjective : Objective

{
    [Header("Button to press")]
    [SerializeField] private Button button;
    [SerializeField] private GameObject buttonGameObject;

    public override void Activate()
    {
        // Called by the ObjectiveManager when this objective becomes active
        enabled = true;
        // 1) Make sure the name is exact (including "(Clone)" if it’s a spawned prefab)
        buttonGameObject = GameObject.Find("TargetIconPrefab(Clone)");
        if (buttonGameObject == null)
        {
            // Debug.LogError("TargetIconPrefab(Clone) not found in the Hierarchy!");
            return;
        }

        // 2) Get the Button component
        button = buttonGameObject.GetComponent<Button>();
        if (button == null)
        {
            // If the Button lives on a child, try this:
            button = buttonGameObject.GetComponentInChildren<Button>();
            if (button == null)
            {
                Debug.LogError("No Button found on TargetIconPrefab(Clone) or its children!");
                return;
            }
        }
        button.onClick.AddListener(CompleteObjective);
    }
    private void CompleteObjective()
    {
        enabled = false;
        button.onClick.RemoveListener(CompleteObjective);
        // Notify the manager via the base class
        Complete();
        Destroy(gameObject);
    }
}
