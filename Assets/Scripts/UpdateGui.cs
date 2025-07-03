using TMPro;
using UnityEngine;

public class UpdateGUI : MonoBehaviour
{
    [SerializeField] private TMP_Text azimuthGui;
    [SerializeField] private TMP_Text rangeGui;
    [SerializeField] private TMP_Text ammoGui;
    [SerializeField] private AmmoDataSO ammoData;
    [SerializeField] private RangeDataSO rangeData;
    void OnEnable()
    {
        if (rangeData != null)
            rangeData.OnRangeUpdated += UpdateRangeFromSO;
        if (ammoData != null)
        {
            ammoData.OnAmmoChanged += UpdateAmmoFromSO;
        }
    }

    void OnDisable()
    {
        if (rangeData != null)
            rangeData.OnRangeUpdated -= UpdateRangeFromSO;
        if (ammoData != null)
        {
            ammoData.OnAmmoChanged -= UpdateAmmoFromSO;
        }
    }

    private void UpdateRangeFromSO(int range, bool isSrak, float lastDistance, Vector3 lastLaser)
    {
        UpdateRange(range);
    }
    private void UpdateAmmoFromSO(string ammo)
    {
        UpdateAmmo(ammo);
    }

    // Update the range text in the UI (called automatically via event)
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
        if (ammo != null)
        {
            ammoGui.text = ammo;
        }
    }
    public void UpdateAzimuth(int azimuth)
    {
        azimuthGui.text = azimuth.ToString();
    }
}