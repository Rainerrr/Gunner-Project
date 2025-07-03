using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
    fileName = "NewAmmoDataSO",
    menuName = "Data/AmmoData",
    order = 1)]
public class AmmoDataSO : ScriptableObject
{
    [Tooltip("List of available ammo types")]
    public List<string> ammoTypes = new List<string>()
    {
        "kalna",
        "halool",
        "hazav",
        "hetz"
    };

    [Tooltip("Index of the currently selected ammo type")]
    [SerializeField] private int selectedAmmoIndex = 0;

    /// Invoked whenever the ammo selection changes, passing the name of the selected ammo.
    public event Action<string> OnAmmoChanged;

    /// Cycles through the ammo list in a positive or negative direction.
    public void CycleAmmo(int direction)
    {
        if (ammoTypes.Count == 0) return;
        selectedAmmoIndex = (selectedAmmoIndex + direction + ammoTypes.Count) % ammoTypes.Count;
        NotifyChange();
    }

    /// Returns the name of the currently selected ammo.
    public string GetSelectedAmmo()
    {
        return ammoTypes.Count > 0
            ? ammoTypes[selectedAmmoIndex]
            : string.Empty;
    }

    /// Fires the ammo changed event.
    private void NotifyChange()
    {
        OnAmmoChanged?.Invoke(GetSelectedAmmo());
    }
}
