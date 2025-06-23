using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class RangeFind : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private UpdateGui gui;
    [SerializeField] private MapContentBehavior mapContentBehavior;
    [SerializeField] private FireEndpoint FireEndpoint;

    public bool isSrak;
    private int srak = 0;
    private GameObject lastDebugRay;
    private Tuple<float, float> rangePreset;
    [SerializeField] private Transform ballisticCurve;
    [SerializeField] private Transform minObjectTransform;
    [SerializeField] private Transform maxObjectTransform;
    [SerializeField] private float minRange = 10;
    [SerializeField] private float maxRange = 5000;
    public int currentRange;
    public Tuple<float, float> SetRangePreset(Transform min, Transform Max) //sets the furthest and closest point the player can find the range to
    {
        float minObjectDistance = Vector3.Distance(playerCamera.transform.position, minObjectTransform.position);
        float maxObjectDistance = Vector3.Distance(playerCamera.transform.position, maxObjectTransform.position);
        Tuple<float, float> rangePreset = Tuple.Create(minObjectDistance, maxObjectDistance);
        return rangePreset;
    }

    int NormalizeRange(float value, Tuple<float, float> rangePreset)
    {
        if (value < rangePreset.Item1 || value > rangePreset.Item2)
        {
            gui.UpdateRange(srak);
            return 0;
        }
        float t = (value - rangePreset.Item1) / (rangePreset.Item2 - rangePreset.Item1);// normalize 0–1
        int result = Mathf.RoundToInt(Mathf.Lerp(minRange, maxRange, t));// interpolate output
        return result;
    }
    public int GetRangeToTarget(Transform targetTransform)
    {
        float distance = Vector3.Distance(this.transform.position, targetTransform.position);
        int range = NormalizeRange(distance, rangePreset);
        return range;
    }
    public void FindRange(float input)
    {
        // Cast a ray from the camera forward
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            float distance = hit.distance;
            Debug.Log("Hit object: " + hit.collider.name + " at distance: " + distance);
            // Draw a frozen debug ray
            DrawFrozenRay(ray.origin, ray.direction * distance, Color.red);
            Debug.Log("returned input = " + NormalizeRange(distance, rangePreset));
            isSrak = false;
            currentRange = NormalizeRange(distance, rangePreset);
            FireEndpoint.UpdateRange(distance);
            mapContentBehavior.UpdateLaserIndicator(Mathf.RoundToInt(currentRange));
            gui.UpdateRange(currentRange);
        }
        else
        {
            Debug.Log("No object hit");
            DrawFrozenRay(ray.origin, ray.direction * 100f, Color.gray);
            NormalizeRange(srak, rangePreset);
            isSrak = true;
            currentRange = srak;
            FireEndpoint.UpdateRange(srak);
            mapContentBehavior.UpdateLaserIndicator(Mathf.RoundToInt(srak));
        }
        UpdateLastLaserObject(hit);
    }
    private void UpdateLastLaserObject(RaycastHit hit)
    {
        if (isSrak)
        {
            ballisticCurve.Find("LastLaser").position = transform.position;
            Debug.Log("srak");
        }
        else
        {
            ballisticCurve.Find("LastLaser").position = hit.point;
        }
    }
    private void DrawFrozenRay(Vector3 origin, Vector3 direction, Color color)
    {
        // Destroy the previous ray if it exists
        if (lastDebugRay != null)
        {
            Destroy(lastDebugRay);
        }

        // Create a line to represent the ray
        GameObject lineObj = new GameObject("LaserDebugRay");
        LineRenderer lineRenderer = lineObj.AddComponent<LineRenderer>();

        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, origin);
        lineRenderer.SetPosition(1, origin + direction);
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;

        lastDebugRay = lineObj;
    }
    void Start()
    {
        rangePreset = SetRangePreset(minObjectTransform, maxObjectTransform);
        currentRange = srak;
        isSrak = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
