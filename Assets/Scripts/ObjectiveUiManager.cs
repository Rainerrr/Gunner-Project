using UnityEditor.SceneManagement;
using UnityEngine;

/// <summary>
/// Example script showing how to react to a specific objective completing.
/// It subscribes to the completion event of objective number 2 and logs
/// a message when that objective finishes.
/// </summary>
public class ObjectiveUiManager : MonoBehaviour
{
    // Reference to the manager containing the objectives
    public ObjectiveManager objectiveManager;
    [SerializeField] private ObjectiveUi objectiveUi;
    private ObjectiveManager.ObjectiveStage currentStage;

    private void Start()
    {
        TryGetStageFromManager(0);
    }
    private void TryGetStageFromManager(int index)
    {
        if (objectiveManager != null &&
            objectiveManager.TryGetStage(index, out var stage))
        {
            currentStage = stage;
            ObjectiveUi newpanel = Instantiate(objectiveUi, this.transform);
            newpanel.Init(stage.objective);    
            stage.onStageCompleted.AddListener(NextObjective);
        }
        else
        {
            return;
        }
    }

    private void NextObjective(int index)
    {
        if (objectiveManager != null &&
         objectiveManager.TryGetStage(index, out var stage))
        {
            currentStage = stage;
            ObjectiveUi newpanel = Instantiate(objectiveUi, this.transform);
            newpanel.Init(stage.objective);
            stage.onStageCompleted.AddListener(NextObjective);
        }
        else
        {
            return;
        }
    }
}
