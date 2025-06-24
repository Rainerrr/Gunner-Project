using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectiveManager : MonoBehaviour
{
    [Header("UI Setup")]
    public Transform objectivesPanel;      // Left-side panel (VerticalLayoutGroup)
    public GameObject objectiveGui;  // Your ObjectiveUIItem prefab

    // internal state
    private List<Objective> objectives;
    private int currentIndex = 0;
    private ObjectiveGui currentObjectiveGui;

    void Start()
    {
        // 1. grab and sort by sibling index (their place in the Hierarchy under the same parent)
        objectives = FindObjectsOfType<Objective>()
                       .OrderBy(o => o.transform.GetSiblingIndex())
                       .ToList();

        // 2. hide them all initially
        objectives.ForEach(o => o.gameObject.SetActive(false));

        // 3. start the first one
        ActivateObjective(0);
    }

    void ActivateObjective(int idx)
    {
        if (idx < 0 || idx >= objectives.Count) return;

        currentIndex = idx;
        var obj = objectives[idx];

        // enable the GameObject so its logic & Complete() can fire
        obj.gameObject.SetActive(true);

        // instantiate & init the UI row
        var uiGO = Instantiate(objectiveGui, objectivesPanel);
        currentObjectiveGui = uiGO.GetComponent<ObjectiveGui>();
        currentObjectiveGui.Init(obj);

        // subscribe to its completion
        obj.OnCompleted += OnCurrentCompleted;
    }

    void OnCurrentCompleted(Objective obj)
    {
        // cleanup this objective's event & UI
        obj.OnCompleted -= OnCurrentCompleted;
        //currentObjectiveGui.Check();       // you can add a helper that just turns on the checkmark
        currentObjectiveGui = null;

        // destroy or disable the old objective prefab if you like
        // Destroy(obj.gameObject);

        // move to the next one
        ActivateObjective(currentIndex + 1);
    }
}