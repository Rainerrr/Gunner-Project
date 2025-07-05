using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class TargetBank : MonoBehaviour
{
    public static TargetBank Instance;

    [SerializeField] private GameObject targetPrefab;
    [SerializeField] private Transform newTargetPositon;
    [SerializeField] public RangeFind rangeFind;
    [SerializeField] public AzimuthFind azimuthFind;
    [SerializeField] public MapContentBehavior mapContentBehavior;
    [SerializeField] public Turret turret;
    [SerializeField] private TargetBankSO bankData;

    public List<Target> runtimeTargets = new List<Target>();

    /// Creates a new Target instance and adds it to the bank
    public Target CreateNewTarget(string name, TargetType targetType, Vector3 worldPosition, string description)
    {
        if (targetPrefab == null)
        {
            Debug.LogError("TargetBank: Missing targetPrefab in the Inspector!");
            return null;
        }
        // 1. Instantiate the prefab at the desired position
        GameObject go = Instantiate(targetPrefab, worldPosition, Quaternion.identity, transform);
        go.name = name;

        // 2. Retrieve and initialize the Target component
        Target t = go.GetComponent<Target>();
        if (t == null)
        {
            Debug.LogError("TargetBank: Prefab does not contain a Target component!");
            Destroy(go);
            return null;
        }
        // 3. Call Init

        t.Init(name, targetType, go, rangeFind, azimuthFind, mapContentBehavior);
        t.description = description;

        // 4. Add to the list
        AddTarget(t);
        return t;
    }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Instance.RegisterSceneBank(bankData);
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        RegisterSceneBank(bankData);
    }

    void Start()
    {

    }
    void Update()
    {

    }

    public void RegisterSceneBank(TargetBankSO newBank)
    {
        if (newBank == null)
            return;

        TargetBankSO.ActiveBank = newBank;
        bankData = newBank;
        runtimeTargets.Clear();
    }

    public void AddTarget(Target target)
    {
        if (target == null || runtimeTargets.Contains(target))
            return;

        runtimeTargets.Add(target);
        bankData?.OnTargetAdded?.Invoke(target);
    }

    public void RemoveTarget(Target target)
    {
        if (target == null)
            return;

        if (runtimeTargets.Remove(target))
        {
            bankData?.OnTargetRemoved?.Invoke(target);
        }
    }
}
