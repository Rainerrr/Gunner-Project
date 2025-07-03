using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireEvent : MonoBehaviour
{
    [SerializeField] private RangeDataSO rangeData;
    [SerializeField] private InputEventChannelSO inputEventChannel;
    [SerializeField] GameObject shellPrefab;
    [SerializeField] public BallisticCurve ballisticCurve;
    public GameObject CreateFromPrefab(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        GameObject obj = Instantiate(prefab, position, rotation);
        return obj;
    }

    private void OnEnable()
    {
        if (inputEventChannel != null)
            inputEventChannel.OnFire += Fire;
    }

    private void OnDisable()
    {
        if (inputEventChannel != null)
            inputEventChannel.OnFire -= Fire;
    }
    public void Fire(float input){
        if (!rangeData.isSrak)
        {
            ballisticCurve.LockTrajectory();
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
