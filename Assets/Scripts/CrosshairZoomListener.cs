using UnityEngine;

public class CrosshairZoomListener : MonoBehaviour
{
    [SerializeField] private ZoomDataSO zoomData;
    [SerializeField] private Canvas crosshairCanvas;

    private void OnEnable()
    {
        if (zoomData != null)
            zoomData.OnZoomChanged += HandleZoomChanged;
    }

    private void OnDisable()
    {
        if (zoomData != null)
            zoomData.OnZoomChanged -= HandleZoomChanged;
    }

    private void HandleZoomChanged(float fov, float crosshairScale, ZoomSpeedPair speeds)
    {
        if (crosshairCanvas != null)
        {
            crosshairCanvas.scaleFactor = crosshairScale;
        }
    }
}
