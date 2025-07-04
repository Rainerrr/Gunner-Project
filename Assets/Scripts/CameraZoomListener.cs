using UnityEngine;
using Cinemachine;

public class CameraZoomListener : MonoBehaviour
{
    [SerializeField] private ZoomDataSO zoomData;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;

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
        if (virtualCamera != null)
        {
            virtualCamera.m_Lens.FieldOfView = fov;
        }
    }
}
