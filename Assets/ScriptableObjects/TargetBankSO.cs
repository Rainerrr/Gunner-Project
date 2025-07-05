using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Target Bank", fileName = "TargetBank")]
public class TargetBankSO : ScriptableObject
{
    public static TargetBankSO ActiveBank { get; private set; }

    public List<Target> targets = new List<Target>();

    public event Action<Target> OnTargetAdded;
    public event Action<Target> OnTargetRemoved;

    public void RegisterSceneBank()
    {
        ActiveBank = this;
        Clear();
    }

    public void AddTarget(Target target)
    {
        if (!targets.Contains(target))
        {
            targets.Add(target);
            OnTargetAdded?.Invoke(target);
        }
    }

    public void RemoveTarget(Target target)
    {
        if (targets.Remove(target))
        {
            OnTargetRemoved?.Invoke(target);
        }
    }

    public void Clear()
    {
        targets.Clear();
    }
}
