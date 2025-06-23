
using UnityEngine;
using UnityEngine.EventSystems;

public class MinimapBehavior : MonoBehaviour
{
    [Header("References")]
    [Tooltip("The RectTransform of your full Minimap GameObject (mask + map + icons)")]
    [SerializeField] private RectTransform minimapRect;
    [Tooltip("Player controller script to enable/disable input when the minimap is toggled")]
    [SerializeField] private Player playerController;

    [Header("Zoom Settings")]
    [Tooltip("How many times larger the minimap becomes when expanded")]
    [SerializeField] private float zoomFactor = 3f;
    [Tooltip("Duration of the expand/collapse animation in seconds")]
    [SerializeField] private float transitionDuration = 0.3f;

    [Header("Collapsed Position Override")]
    [Tooltip("Exact anchoredPosition when minimap is collapsed (bottom-left)")]
    [SerializeField] private Vector2 collapsedPosition = new Vector2(-3000f, -200f);
    // Always center when expanded
    private Vector2 expandedPosition = Vector2.zero;
    private Vector3 originalScale;
    private Vector3 targetScale;
    private bool isZoomed = false;
    private float timer = Mathf.Infinity;

    void Start()
    {
        if (minimapRect == null)
        {
            Debug.LogError("MinimapBehavior: please assign minimapRect in Inspector!");
            enabled = false;
            return;
        }

        // Ensure the minimap pivot and anchors are centered so (0,0) is screen center
        minimapRect.pivot = new Vector2(0.5f, 0.5f);
        minimapRect.anchorMin = minimapRect.anchorMax = new Vector2(0.5f, 0.5f);

        // Cache scales
        originalScale = minimapRect.localScale;
        targetScale = originalScale * zoomFactor;

        // Force initial collapsed state
        isZoomed = false;
        minimapRect.localScale = originalScale;
        minimapRect.anchoredPosition = collapsedPosition;
    }

    void Update()
    {
        // Only animate during transition
        if (timer > transitionDuration) return;

        timer += Time.deltaTime;
        float t = Mathf.SmoothStep(0f, 1f, timer / transitionDuration);

        // Determine start/end for scale and position based on target state
        Vector3 startScale = isZoomed ? originalScale : targetScale;
        Vector3 endScale = isZoomed ? targetScale : originalScale;

        Vector2 startPos = isZoomed ? collapsedPosition : expandedPosition;
        Vector2 endPos = isZoomed ? expandedPosition : collapsedPosition;

        minimapRect.localScale = Vector3.Lerp(startScale, endScale, t);
        minimapRect.anchoredPosition = Vector2.Lerp(startPos, endPos, t);
    }

    public void ToggleMinimapSize()
    {
        // Deselect any UI element so clicks don't hang
        EventSystem.current.SetSelectedGameObject(null);

        // Flip target state and restart timer
        isZoomed = !isZoomed;
        timer = 0f;

        // Enable/disable player inputs
        if (isZoomed)
        {
            playerController.DisableFireInput();
            playerController.DisableLaserInput();
            playerController.DisableZoomInput();
            playerController.DisableAmmoInput();
        }
        else
        {
            playerController.EnableFireInput();
            playerController.EnableLaserInput();
            playerController.EnableZoomInput();
            playerController.EnableAmmoInput();
        }
    }
}
