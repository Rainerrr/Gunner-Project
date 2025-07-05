using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ToggleMenuController : MonoBehaviour
{
    [Header("The toggle that opens/closes the menu")]
    [SerializeField] private Toggle triggerToggle;

    [Header("All option toggles (single‐choice buttons)")]
    [SerializeField] private List<Toggle> optionToggles = new List<Toggle>();

    void Start()
    {
        if (triggerToggle == null)
        {
            Debug.LogError("ToggleMenuController: triggerToggle not set in Inspector!", this);
            return;
        }

        // 1) Ensure menu is closed at start
        triggerToggle.SetIsOnWithoutNotify(false);
        HideOptions();

        // 2) Listen for opening/closing the menu
        triggerToggle.onValueChanged.AddListener(OnTriggerChanged);

        // 3) Listen for each option being clicked
        foreach (var t in optionToggles)
        {
            // start each option OFF
            t.SetIsOnWithoutNotify(false);
            // hide its GameObject
            t.gameObject.SetActive(false);

            // capture loop variable so the correct reference is passed
            Toggle local = t;
            local.onValueChanged.AddListener(isOn => OnOptionChanged(local, isOn));
        }
    }

    private void OnTriggerChanged(bool isOn)
    {
        if (isOn) ShowOptions();
        else HideOptions();
    }

    private void ShowOptions()
    {
        foreach (var t in optionToggles)
        {
            t.SetIsOnWithoutNotify(false);  // reset to OFF
            t.gameObject.SetActive(true);   // show it
        }
    }

    private void HideOptions()
    {
        foreach (var t in optionToggles)
        {
            t.SetIsOnWithoutNotify(false);  // reset to OFF
            t.gameObject.SetActive(false);  // hide it
        }
    }

    private void OnOptionChanged(Toggle changedToggle, bool isOn)
    {
        if (!isOn) return;
        // turn all other options off
        foreach (var t in optionToggles)
            if (t != changedToggle)
                t.SetIsOnWithoutNotify(false);
    }

    void OnDestroy()
    {
        // cleanup all listeners
        if (triggerToggle != null)
            triggerToggle.onValueChanged.RemoveListener(OnTriggerChanged);

        foreach (var t in optionToggles)
            t.onValueChanged.RemoveAllListeners();
    }
}