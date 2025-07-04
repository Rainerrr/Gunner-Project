using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Manages a sequence of objectives that are supplied as child game objects.
/// On start the manager gathers the objectives in hierarchy order, subscribes
/// to their completion events and activates them one by one.
/// </summary>
public class ObjectiveManager : MonoBehaviour
{
    /// <summary>
    /// Wrapper class for a single objective stage.
    /// Holds a reference to the objective behaviour and an event that fires
    /// when this stage is completed.
    /// </summary>
    [Serializable]
    public class ObjectiveStage
    {
        // Objective behaviour controlling the stage logic
        public Objective objective;

        // Event invoked when the stage finishes. The integer parameter
        // represents the stage number that was completed.
        public UnityEvent<int> onStageCompleted = new UnityEvent<int>();
    }

    private readonly List<ObjectiveStage> stages = new List<ObjectiveStage>();
    private int currentStageIndex = -1;

    private void Awake()
    {
        BuildStageList();
        StartNextStage();
    }

    /// <summary>
    /// Debug helper that logs whenever a stage is completed.
    /// </summary>
    /// <param name="stageNumber">1-based index of the completed stage.</param>
    private void LogStageCompleted(int stageNumber)
    {
        Debug.Log($"stage number {stageNumber} completed");
    }

    /// <summary>
    /// Iterates over all direct children and creates an ordered list of stages.
    /// Every child is expected to contain an <see cref="Objective"/> component.
    /// </summary>
    private void BuildStageList()
    {
        stages.Clear();
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            Objective objective = child.GetComponent<Objective>();
            if (objective == null)
            {
                Debug.LogWarning($"Child {child.name} does not contain an Objective component");
                continue;
            }

            // deactivate until the manager activates it
            child.gameObject.SetActive(false);

            ObjectiveStage stage = new ObjectiveStage { objective = objective };
            // Subscribe a debug listener so we know when each stage completes.
            stage.onStageCompleted.AddListener(LogStageCompleted);
            stages.Add(stage);
        }
    }

    /// <summary>
    /// Allows external scripts to retrieve a stage by index so they can
    /// subscribe to its completion event.
    /// </summary>
    /// <param name="index">Zero-based stage index.</param>
    /// <param name="stage">The stage instance if found.</param>
    /// <returns>True if the stage exists.</returns>
    public bool TryGetStage(int index, out ObjectiveStage stage)
    {
        if (index >= 0 && index < stages.Count)
        {
            stage = stages[index];
            return true;
        }

        stage = null;
        return false;
    }

    /// <summary>
    /// Activates the next objective stage if available.
    /// </summary>
    private void StartNextStage()
    {
        currentStageIndex++;
        if (currentStageIndex >= stages.Count)
            return;

        ObjectiveStage stage = stages[currentStageIndex];
        Objective objective = stage.objective;

        stage.objective.gameObject.SetActive(true);
        objective.OnCompleted += HandleCurrentStageCompleted;
        objective.Activate();
    }

    /// <summary>
    /// Called when the active objective reports completion.
    /// Cleans up and moves on to the next objective.
    /// </summary>
    private void HandleCurrentStageCompleted()
    {
        ObjectiveStage stage = stages[currentStageIndex];
        Objective objective = stage.objective;
        objective.OnCompleted -= HandleCurrentStageCompleted;

        // Notify listeners that this stage finished. We pass the stage number
        // (1-based) so external systems know which objective was just
        // completed.
        stage.onStageCompleted.Invoke(currentStageIndex + 1);
        objective.gameObject.SetActive(false);

        StartNextStage();
    }
}
