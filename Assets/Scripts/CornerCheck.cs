using UnityEngine;

public class CornerCheck : Objective
{
    [Header("Corners to check")]
    public Transform[] cornerTransforms = new Transform[4];

    [Header("Laser object that triggers collisions")]
    public GameObject lastLaser;
    [HideInInspector] public bool[] cornerFlag = new bool[4];
    public bool completed = false;

    // Internal: cache collider references
    private Collider laserCollider;
    private Collider[] cornerColliders = new Collider[4];

    void Awake()
    {
        // Cache the laser's Collider component
        lastLaser = GameObject.Find("LastLaser");
        laserCollider = lastLaser.GetComponent<Collider>();

        // Cache each corner's Collider component
        for (int i = 0; i < cornerColliders.Length; i++)
        {
            cornerColliders[i] = cornerTransforms[i].GetComponent<Collider>();
            if (cornerColliders[i] == null)
                Debug.LogError($"Corner {i + 1} is missing a Collider!");
        }
    }

    void FixedUpdate()
    {
        if (completed) return;

        // For each corner not yet hit, check for intersection
        for (int i = 0; i < cornerFlag.Length; i++)
        {
            if (!cornerFlag[i] && IsIntersecting(cornerColliders[i], laserCollider))
            {
                cornerFlag[i] = true;
                Debug.Log($"Corner #{i + 1} hit!");
            }
        }

        // If all corners have been hit, mark as completed
        if (cornerFlag[0] && cornerFlag[1] && cornerFlag[2] && cornerFlag[3])
        {
            Complete();
        }
    }

    bool IsIntersecting(Collider a, Collider b)
    {
        Bounds boundA = a.bounds;
        return boundA.Intersects(b.bounds);
    }
    protected override void OnObjectiveCompleted()
    {
        base.OnObjectiveCompleted();
        Destroy(gameObject);         
    }
}

