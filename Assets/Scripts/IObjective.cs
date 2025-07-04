using System;

/// <summary>
/// Interface that all objective behaviours must implement so that the
/// ObjectiveManager can interact with them generically.
/// </summary>
public interface IObjective
{
    /// Event fired when the objective conditions have been met.
    event Action OnCompleted;

    /// Called by the ObjectiveManager when this objective becomes active.
    void Activate();
}
