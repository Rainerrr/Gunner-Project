using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;


public class UpdateGui : MonoBehaviour
{
    [SerializeField] private TMP_Text azimuthGui;
    [SerializeField] private TMP_Text rangeGui;
    [SerializeField] private TMP_Text ammogui;

    public void UpdateRange(int range)
    {
        if (range == 0)
        {
            rangeGui.text = "....";
        }
        else
        {
            rangeGui.text = range.ToString();
        }
    }
    public void UpdateAmmo(string ammo)
    {
        if (ammo != null){
            ammogui.text = ammo;
        }
    }
    public void UpdateAzimuth(int azimuth)
    {
            azimuthGui.text = azimuth.ToString();
    }
}
