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

    /// <summary>
    /// True once <see cref="Complete"/> has been called.
    /// </summary>
    public bool Completed { get; private set; }

    /// <summary>
    /// Fired once when this objective has been satisfied.
    /// </summary>
    public event Action OnCompleted;

    /// <summary>
    /// Called by <see cref="ObjectiveManager"/> when this objective becomes active.
    /// Override in derived classes to enable behaviour.
    /// </summary>
    public virtual void Activate() { }

    /// <summary>
    /// Marks the objective complete and notifies listeners.
    /// Derived classes should call this when their conditions are met.
    /// </summary>
    protected void Complete()
    {
        if (Completed) return;
        Completed = true;
        OnCompleted?.Invoke();
        OnObjectiveCompleted();
        Debug.Log($"Objective Complete: {objectiveTitle}");
    }

    /// <summary>
    /// Optional hook for subclasses when the objective finishes.
    /// </summary>
    protected virtual void OnObjectiveCompleted() { }
}
