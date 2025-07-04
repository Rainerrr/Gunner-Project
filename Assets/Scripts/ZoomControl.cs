using UnityEngine;

public class ZoomControl : MonoBehaviour
{
    [SerializeField] private ZoomDataSO zoomData;

    public void HandleZoomInput(float input)
    {
        if (input == 1f)
        {
            zoomData.IncreaseZoom();
        }
        else if (Mathf.Approximately(input, -1f))
        {
            zoomData.DecreaseZoom();
        }
    }

    public void Init()
    {
        zoomData.Init();
    }
}
