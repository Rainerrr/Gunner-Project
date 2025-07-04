using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ObjectiveGui : MonoBehaviour
{
    [SerializeField] TMP_Text descriptionText;
    [SerializeField] Image checkIcon;

    // Initialize this UI entry with the Objective data
    public void Init(Objective obj)
    {
        descriptionText.text = obj.objectiveTitle;
        checkIcon.enabled = obj.Completed;
        // subscribe to updates
        obj.OnCompleted += () => checkIcon.enabled = true;
    }
}
