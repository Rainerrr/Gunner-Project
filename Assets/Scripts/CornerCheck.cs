using System;
using UnityEngine;

/// <summary>
/// Objective that is completed once the player "hits" four target corners
/// with the laser object. Each corner must have a collider component.
/// </summary>
public class CornerCheck : Objective
{
    [Header("Corners to check")]
    public Transform[] cornerTransforms = new Transform[4];

    [Header("Laser object that triggers collisions")]
    public GameObject lastLaser;

    // Flags that track which corners have been hit
    [HideInInspector] public bool[] cornerFlag = new bool[4];

    // Internal cached colliders for quick lookup
    private Collider laserCollider;
    private readonly Collider[] cornerColliders = new Collider[4];


    private void Awake()
    {
        // Find laser collider
        lastLaser = GameObject.Find("LastLaser");
        laserCollider = lastLaser != null ? lastLaser.GetComponent<Collider>() : null;

        // Cache corner colliders
        for (int i = 0; i < cornerColliders.Length; i++)
        {
            if (cornerTransforms[i] != null)
            {
                cornerColliders[i] = cornerTransforms[i].GetComponent<Collider>();
                if (cornerColliders[i] == null)
                    Debug.LogError($"Corner {i + 1} is missing a Collider!");
            }
        }

        // Start disabled until the manager activates us
        enabled = false;
    }

    public override void Activate()
    {
        // Called by the ObjectiveManager when this objective becomes active
        enabled = true;
    }

    private void FixedUpdate()
    {
        if (laserCollider == null) return;

        // Check each corner that has not been hit yet
        for (int i = 0; i < cornerFlag.Length; i++)
        {
            if (!cornerFlag[i] && IsIntersecting(cornerColliders[i], laserCollider))
            {
                cornerFlag[i] = true;
                Debug.Log($"Corner #{i + 1} hit!");
            }
        }

        // If all corners were hit we complete the objective
        if (cornerFlag[0] && cornerFlag[1] && cornerFlag[2] && cornerFlag[3])
        {
            CompleteObjective();
        }
    }

    private bool IsIntersecting(Collider a, Collider b)
    {
        return a != null && b != null && a.bounds.Intersects(b.bounds);
    }

    private void CompleteObjective()
    {
        enabled = false;
        // Notify the manager via the base class
        Complete();
        Destroy(gameObject);
    }
}
