using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class TargetTypeToggle
{
    public Toggle toggle;
    public TargetType targetType;
}

public class TargetCreatorUI : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private RangeDataSO rangeData;
    [SerializeField] private TargetBank targetBank;
    [SerializeField] private Transform lastLaser;

    [Header("Target Type Toggles")]
    [SerializeField] private List<TargetTypeToggle> targetToggles = new List<TargetTypeToggle>();
    [SerializeField] private string targetNamePrefix = "Target";

    private TargetType? selectedType = null;

    private void Awake()
    {
        if (targetBank == null)
            targetBank = TargetBank.Instance;

        if (lastLaser == null)
        {
            GameObject go = GameObject.Find("LastLaser");
            if (go != null)
                lastLaser = go.transform;
        }

        foreach (var pair in targetToggles)
        {
            if (pair.toggle == null)
                continue;
            TargetType localType = pair.targetType;
            pair.toggle.onValueChanged.AddListener(isOn => OnToggleChanged(localType, isOn));
        }
    }

    private void OnEnable()
    {
        if (rangeData != null)
            rangeData.OnRangeUpdated += HandleRangeUpdated;
    }

    private void OnDisable()
    {
        if (rangeData != null)
            rangeData.OnRangeUpdated -= HandleRangeUpdated;
    }

    private void OnToggleChanged(TargetType type, bool isOn)
    {
        if (isOn)
            selectedType = type;
        else if (selectedType == type)
            selectedType = null;
    }

    private void HandleRangeUpdated(int currentRange, bool isSrak, float lastDistance, Vector3 lastPoint)
    {
        if (!selectedType.HasValue || targetBank == null)
            return;

        Vector3 spawnPos = lastLaser != null ? lastLaser.position : lastPoint;
        string name = $"{targetNamePrefix}_{targetBank.runtimeTargets.Count + 1}";
        targetBank.CreateNewTarget(name, selectedType.Value, spawnPos, string.Empty);

        selectedType = null;
        foreach (var pair in targetToggles)
        {
            if (pair.toggle != null)
                pair.toggle.SetIsOnWithoutNotify(false);
        }
    }
}
