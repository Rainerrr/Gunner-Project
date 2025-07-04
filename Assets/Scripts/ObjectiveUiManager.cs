using System.Collections;
using UnityEngine;

/// <summary>
/// Spawns an <see cref="ObjectiveGui"/> UI panel for the currently active
/// objective and replaces it whenever a stage completes.
/// </summary>
public class ObjectiveUiManager : MonoBehaviour
{
    [SerializeField] private ObjectiveManager objectiveManager;
    [SerializeField] private ObjectiveGui objectiveUiPrefab;

    private ObjectiveGui currentUi;
    private int currentStageIndex;

    private void Start()
    {
        if (objectiveManager == null)
            objectiveManager = GetComponent<ObjectiveManager>();

        currentStageIndex = 0;
        ShowCurrentStage();
    }

    /// <summary>
    /// Instantiates the UI for the current stage and subscribes to its
    /// completion event so we know when to advance to the next one.
    /// </summary>
    private void ShowCurrentStage()
    {
        if (objectiveManager == null)
            return;

        if (objectiveManager.TryGetStage(currentStageIndex, out var stage))
        {
            currentUi = Instantiate(objectiveUiPrefab, this.transform);
            currentUi.Init(stage.objective);
            stage.onStageCompleted.AddListener(OnStageCompleted);
        }
    }

    private void OnStageCompleted(int stageNumber)
    {
        if (objectiveManager.TryGetStage(currentStageIndex, out var stage))
        {
            stage.onStageCompleted.RemoveListener(OnStageCompleted);
        }

        StartCoroutine(AdvanceAfterDelay());
    }

    /// <summary>
    /// Waits a few seconds so the check mark is visible before swapping to the
    /// next objective panel.
    /// </summary>
    private IEnumerator AdvanceAfterDelay()
    {
        yield return new WaitForSeconds(3f);

        if (currentUi != null)
            Destroy(currentUi.gameObject);

        currentStageIndex++;
        ShowCurrentStage();
    }
}
