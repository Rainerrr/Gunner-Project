using TMPro;
using UnityEngine;

public class UpdateGUI : MonoBehaviour
{
    [SerializeField] private TMP_Text azimuthGui;
    [SerializeField] private TMP_Text rangeGui;
    [SerializeField] private TMP_Text ammoGui;
    [SerializeField] private AmmoDataSO ammoData;
    [SerializeField] private RangeDataSO rangeData;
    [SerializeField] private AzimuthDataSO azimuthData;
    void OnEnable()
    {
        if (rangeData != null)
            rangeData.OnRangeUpdated += UpdateRangeFromSO;
        if (ammoData != null)
        {
            ammoData.OnAmmoChanged += UpdateAmmoFromSO;
        }
        if (azimuthData != null)
            azimuthData.OnAzimuthUpdated += UpdateAzimuthFromSO;
    }

    void OnDisable()
    {
        if (rangeData != null)
            rangeData.OnRangeUpdated -= UpdateRangeFromSO;
        if (ammoData != null)
        {
            ammoData.OnAmmoChanged -= UpdateAmmoFromSO;
        }
        if (azimuthData != null)
            azimuthData.OnAzimuthUpdated -= UpdateAzimuthFromSO;
    }

    private void UpdateRangeFromSO(int range, bool isSrak, float lastDistance, Vector3 lastLaser)
    {
        UpdateRange(range);
    }
    private void UpdateAmmoFromSO(string ammo)
    {
        UpdateAmmo(ammo);
    }
    private void UpdateAzimuthFromSO(float azimuth)
    {
        UpdateAzimuth(Mathf.RoundToInt(azimuth));
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