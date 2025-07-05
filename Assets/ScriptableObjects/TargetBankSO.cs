using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Target Bank", fileName = "TargetBank")]
public class TargetBankSO : ScriptableObject
{
    public static TargetBankSO ActiveBank { get; private set; }

    [NonSerialized] public List<Target> targets = new List<Target>();

    public Action<Target> OnTargetAdded;
    public Action<Target> OnTargetRemoved;

    public void RegisterSceneBank()
    {
        ActiveBank = this;
    }

    public void AddTarget(Target target)
    {
        TargetBank.Instance?.AddTarget(target);
    }

    public void RemoveTarget(Target target)
    {
        TargetBank.Instance?.RemoveTarget(target);
    }

    public void Clear()
    {
        targets.Clear();
    }
}
