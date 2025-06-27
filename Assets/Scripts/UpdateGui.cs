using TMPro;
using UnityEngine;

public class UpdateGUI : MonoBehaviour
{
    [SerializeField] private TMP_Text azimuthGui;
    [SerializeField] private TMP_Text rangeGui;
    [SerializeField] private TMP_Text ammoGui;
    [SerializeField] private RangeDataSO rangeData;  // Reference to RangeDataSO

    void OnEnable()
    {
        if (rangeData != null)
            rangeData.OnRangeUpdated += UpdateRangeFromSO;
    }

    void OnDisable()
    {
        if (rangeData != null)
            rangeData.OnRangeUpdated -= UpdateRangeFromSO;
    }

    private void UpdateRangeFromSO(int range, bool isSrak, float lastDistance, Vector3 lastLaser)
    {
        UpdateRange(range);
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