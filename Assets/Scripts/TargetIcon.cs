using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetIcon : MonoBehaviour
{
    public Target linkedTarget;
    [SerializeField] Turret turret;
    [SerializeField] Button button;
    // Start is called before the first frame update
    void Start()
    {
        turret = FindAnyObjectByType<Turret>();
        this.button.onClick.AddListener(() => turret.RotateToTarget(linkedTarget));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
