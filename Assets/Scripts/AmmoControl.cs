using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoControl : MonoBehaviour
{
    [SerializeField] private UpdateGUI gui;
    private float activeAmmoCount = 5f;
    public Dictionary<int, string> activeAmmoDict;
    private int selectedAmmoId = 0;
    public void HandleAmmoInput(int input)
    {
        int ammoCount = activeAmmoDict.Count;
        // Wrap key ID like an index
        selectedAmmoId = (selectedAmmoId + input + ammoCount) % ammoCount;
        gui.UpdateAmmo(activeAmmoDict[selectedAmmoId]);
    }
    public string GetSelectedAmmo()
    {
        return activeAmmoDict[selectedAmmoId];
    }
    void Awake()
    {
        activeAmmoDict = new Dictionary<int, string>
        {
            { 0, "kalna"},
            { 1, "halool"},
            { 2, "hazav"},
            { 3, "hetz"},
            { 4, "hakasha"},
            { 5, "haflata"},
        };
    }
    void Start()
    {

    }
    void Update()
    {
    }
}
