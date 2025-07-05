using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class MapContentBehavior : MonoBehaviour
{

    [SerializeField] private RangeDataSO rangeData;
    [SerializeField] private AzimuthDataSO azimuthData;

    [SerializeField] public float MapScaleFactor = 0.1f;
    [SerializeField] private GameObject scalecalculation;
    [SerializeField] private RawImage scaleRangeFinder;
    [SerializeField] private RectTransform mapTargetBank;         
    [SerializeField] private RectTransform ATiconPrefab;
    [SerializeField] private RectTransform SafamiconPrefab;
    [SerializeField] private RawImage laserIndicator;
    [SerializeField] private RectTransform tankIcon;
    public TargetIcon AddTargetToMap(Target target)
    {
        //getting the 2d rotation
        scalecalculation.transform.localEulerAngles = new Vector3(0, 0, -(target.azimuth * (360f / 6400f)));
        scaleRangeFinder.rectTransform.sizeDelta = new Vector2(scaleRangeFinder.rectTransform.sizeDelta.x, target.range * MapScaleFactor);
        Vector3 localEndOnLine = new Vector3(0, scaleRangeFinder.rectTransform.rect.height * (1 - scaleRangeFinder.rectTransform.pivot.y), 0);

        // 2. Convert that local point into world space:
        Vector3 worldEnd = scaleRangeFinder.rectTransform.TransformPoint(localEndOnLine);

        // 3. Convert the world position into Content’s local space:
        Vector3 localInContent = mapTargetBank.InverseTransformPoint(worldEnd);
        if (target.type == TargetType.Enemy)
        {
            RectTransform icon = Instantiate(ATiconPrefab, mapTargetBank);
            icon.localPosition = localInContent;
            icon.name = target.name + "Icon";
            return icon.gameObject.GetComponent<TargetIcon>();
        }
        else if (target.type == TargetType.safam)
        {
            RectTransform icon = Instantiate(SafamiconPrefab, mapTargetBank);
            icon.localPosition = localInContent;
            icon.name = target.name + "Icon";
            return icon.gameObject.GetComponent<TargetIcon>();
            
        }
        else
        {
            UnityEngine.Debug.Log("null map icon");
            return null;
        }
    
    }
    public void UpdateLaserIndicator(int range)
    {
        laserIndicator.rectTransform.sizeDelta = new Vector2(laserIndicator.rectTransform.sizeDelta.x, (range * MapScaleFactor));
    }
    void UpdateTankIcon(float degrees)
    {
        tankIcon.localEulerAngles = new Vector3(0, 0, -degrees);

    }

    private void OnEnable()
    {
        tankIcon = transform.Find("MapPosition/TurretIcon").GetComponent<RectTransform>();
        if (rangeData != null)
            rangeData.OnRangeUpdated += OnRangeUpdated;
        if (azimuthData != null)
            azimuthData.OnAzimuthUpdated += OnAzimuthUpdated;
    }

    private void OnDisable()
    {
        if (rangeData != null)
            rangeData.OnRangeUpdated -= OnRangeUpdated;
        if (azimuthData != null)
            azimuthData.OnAzimuthUpdated -= OnAzimuthUpdated;
    }

    private void OnRangeUpdated(int range, bool isSrak, float lastDistance, Vector3 lastLaser)
    {
        UpdateLaserIndicator(range);
    }

    private void OnAzimuthUpdated(float azimuthMil)
    {
        float degrees = azimuthMil * (360f / 6400f);
        UpdateTankIcon(degrees);
    }

    void Start()
    {
        tankIcon = transform.Find("MapPosition/TurretIcon").GetComponent<RectTransform>();
        if (azimuthData != null)
        {
            float degrees = azimuthData.currentAzimuth * (360f / 6400f);
            UpdateTankIcon(degrees);
        }
        if (rangeData != null)
        {
            UpdateLaserIndicator(rangeData.currentRange);
        }
    }
}
