using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireEvent : MonoBehaviour
{
    [SerializeField] GameObject shellPrefab;
    [SerializeField] public BallisticCurve ballisticCurve;
    [SerializeField] public RangeFind rangeFind;
    public GameObject CreateFromPrefab(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        GameObject obj = Instantiate(prefab, position, rotation);
        return obj;
    }
    public void Fire(float input){
        if (!rangeFind.isSrak)
        {
            FireEndpoint fireEndpoint = ballisticCurve.GetComponentInChildren<FireEndpoint>();
            fireEndpoint.LockTrajectory();
            GameObject shell = CreateFromPrefab(shellPrefab,transform.position, shellPrefab.transform.rotation);
            shell.GetComponent<ProjectileBehavior>().SetBallisticCurve(ballisticCurve);
        }

    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
