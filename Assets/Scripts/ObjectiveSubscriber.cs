using UnityEngine;

/// <summary>
/// Example script showing how to react to a specific objective completing.
/// It subscribes to the completion event of objective number 2 and logs
/// a message when that objective finishes.
/// </summary>
public class ObjectiveSubscriberExample : MonoBehaviour
{
    // Reference to the manager containing the objectives
    public ObjectiveManager objectiveManager;

    private void Start()
    {
        // Objective indices are zero-based internally, so index 1 is the
        // second objective in the list.
        if (objectiveManager != null &&
            objectiveManager.TryGetStage(1, out var stage))
        {
            stage.onStageCompleted.AddListener(OnObjectiveTwoCompleted);
        }
    }

    private void OnObjectiveTwoCompleted(int stageNumber)
    {
        Debug.Log("objective 2 completed - preparing objective 3");
    }
}
