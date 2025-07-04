using System;
using UnityEngine;

/// <summary>
/// Base class that all objective behaviours must inherit so the
/// <see cref="ObjectiveManager"/> can interact with them generically.
/// Provides common metadata fields and a completion event.
/// </summary>
public abstract class Objective : MonoBehaviour
{
    [Header("Objective Info")]
    public string objectiveTitle = "New Objective";

    [TextArea]
    public string objectiveDescription;
    
    /// True once <see cref="Complete"/> has been called.
    public bool Completed { get; private set; }

    /// Fired once when this objective has been satisfied.
    public event Action OnCompleted;

    /// Called by <see cref="ObjectiveManager"/> when this objective becomes active.
    /// Override in derived classes to enable behaviour.
    public virtual void Activate() { }

    /// Marks the objective complete and notifies listeners.
    /// Derived classes should call this when their conditions are met.
    protected void Complete()
    {
        if (Completed) return;
        Completed = true;
        OnCompleted?.Invoke();
        OnObjectiveCompleted();
        Debug.Log($"Objective Complete: {objectiveTitle}");
    }

    /// Optional hook for subclasses when the objective finishes.
    protected virtual void OnObjectiveCompleted() { }
}
