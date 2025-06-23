using System.Diagnostics;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;

public class Target : MonoBehaviour
{
    public MapContentBehavior mapContentBehavior;
    public TargetBank targetBank;
    [Header("Target Settings")]
    public string targetName;
    public TargetType type;

    [Header("Linked GameObjects")]
    public GameObject targetObject;       // Reference to physical object
    public TargetIcon targetIcon;             // Reference to minimap icon (optional)

    [Header("Initial Stats")]
    public int range;
    public int azimuth;

    [Header("Description")]
    [TextArea(2, 5)]
    public string description;

    [HideInInspector] public bool destroyed = false;
    [HideInInspector] public bool isActive = true;
    public void Init(string name, TargetType targetType, GameObject targetObject)
    {
        this.targetBank = transform.parent.GetComponent<TargetBank>();
        this.mapContentBehavior = FindObjectOfType<MapContentBehavior>();


        this.targetName = name;
        this.type = targetType;
        this.targetObject = targetObject;

        this.azimuth = Mathf.RoundToInt(targetBank.azimuthFind.GetAzimuthToTarget(targetObject.transform));
        this.range = targetBank.rangeFind.GetRangeToTarget(targetObject.transform);

        if (targetType == TargetType.Enemy)
        {
            targetIcon = mapContentBehavior.AddTargetToMap(this);
        }
        else
        {
            UnityEngine.Debug.Log("null map icon");
        }
        if (targetIcon != null)
        {
            targetIcon.linkedTarget = this;
        }

    }
    private void RotateToThisTarget()
    {
        targetBank.turret.RotateToTarget(this);
    }
    private void Start()
    {
    }

    private void OnDestroy()
    {

    }
}

public enum TargetType
{
    Enemy,
    Ally,
    Objective
}