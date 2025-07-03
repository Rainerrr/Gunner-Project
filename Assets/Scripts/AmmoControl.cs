using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoControl : MonoBehaviour
{
    [SerializeField] private AmmoDataSO ammoDataSO;
    public void HandleAmmoInput(int input)
    {
        ammoDataSO.CycleAmmo(input);
    }
    void Start()
    {

    }
    void Update()
    {
    }
}
