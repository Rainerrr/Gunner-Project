
using UnityEngine;
using System;

public class Objective : MonoBehaviour
{
    [Header("Objective Settings")]
    public string objectiveTitle = "New Objective";
    public bool Completed { get; private set; }
    public event Action<Objective> OnCompleted;

    /// Call this when your objective condition is met.
    /// Fires the OnCompleted event exactly once.

    protected void Complete()
    {
        if (Completed) return;
        Completed = true;
        OnCompleted?.Invoke(this);
        OnObjectiveCompleted();
        Debug.Log($"Objective Complete: {objectiveTitle}");
    }
    protected virtual void OnObjectiveCompleted()
    {
        Debug.Log($"Objective Complete: {objectiveTitle}");
    }
}