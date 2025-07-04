using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class TargetBank : MonoBehaviour
{
    [SerializeField] private GameObject targetPrefab;
    [SerializeField] private Transform newTargetPositon;
    [SerializeField] public RangeFind rangeFind;
    [SerializeField] public AzimuthFind azimuthFind;
    [SerializeField] public MapContentBehavior mapContentBehavior;
    [SerializeField] public Turret turret;
    public List<Target> targetList;

    /// Creates a new Target instance and adds it to targetList
    public Target CreateNewTarget(string name, TargetType targetType, Vector3 worldPosition, string description)
    {
        if (targetPrefab == null)
        {
            Debug.LogError("TargetBank: Missing targetPrefab in the Inspector!");
            return null;
        }
        // 1. Instantiate the prefab at the desired position
        GameObject go = Instantiate(targetPrefab, worldPosition, Quaternion.identity, transform);

        // 2. Retrieve and initialize the Target component
        Target t = go.GetComponent<Target>();
        if (t == null)
        {
            Debug.LogError("TargetBank: Prefab does not contain a Target component!");
            Destroy(go);
            return null;
        }
        // 3. Call Init

        t.Init(name, targetType, go);
        t.description = description;

        // 4. Add to the list
        targetList.Add(t);
        return t;
    }
    void Start()
    {
        CreateNewTarget("Alpha", TargetType.Enemy, newTargetPositon.position, "test target");
        turret.RotateToTarget(targetList[0]);
    }
    void Update()
    {
        
    }
}
