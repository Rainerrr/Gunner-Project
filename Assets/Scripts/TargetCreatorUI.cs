using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TargetCreatorUI : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private RangeDataSO rangeData;
    [SerializeField] private TargetBank targetBank;

    [Header("Target Type Toggles")]
    [SerializeField] private List<TargetTypeToggleInfo> targetToggles = new List<TargetTypeToggleInfo>();
    [SerializeField] private string targetNamePrefix = "Target";

    private TargetType? selectedType = null;

    private void Awake()
    {
        if (targetBank == null)
            targetBank = TargetBank.Instance;
        foreach (var info in targetToggles)
        {
            if (info == null || info.Toggle == null)
                continue;
            TargetType localType = info.Type;
            info.Toggle.onValueChanged.AddListener(isOn => OnToggleChanged(localType, isOn));
        }
    }

    private void OnEnable()
    {
        if (rangeData != null)
            rangeData.OnRangeUpdated += CreateSelectedTargetByLaser;
    }

    private void OnDisable()
    {
        if (rangeData != null)
            rangeData.OnRangeUpdated -= CreateSelectedTargetByLaser;
    }

    private void OnToggleChanged(TargetType type, bool isOn)
    {
        if (isOn)
            selectedType = type;
        else if (selectedType == type)
            selectedType = null;
    }

    private void CreateSelectedTargetByLaser(int currentRange, bool isSrak, float lastDistance, Vector3 lastPoint)
    {
        if (!selectedType.HasValue || targetBank == null)
            return;

        Vector3 spawnPos = rangeData.lastLaserPoint;
        string name = $"{targetNamePrefix}_{targetBank.runtimeTargets.Count + 1}";
        targetBank.CreateNewTarget(name, selectedType.Value, spawnPos, string.Empty);

        selectedType = null;
        foreach (var info in targetToggles)
        {
            if (info != null && info.Toggle != null)
                info.Toggle.SetIsOnWithoutNotify(false);
        }
    }
}
