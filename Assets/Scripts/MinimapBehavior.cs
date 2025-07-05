using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;  // הוספנו

public class MinimapBehavior : MonoBehaviour
{
    [Header("References")]
    [Tooltip("The RectTransform of your full Minimap GameObject (mask + map + icons)")]
    [SerializeField] private RectTransform minimapRect;
    [Tooltip("Player controller script to enable/disable input when the minimap is toggled")]
    [SerializeField] private PlayerController playerController;
    [Tooltip("The UI Toggle used to expand/collapse the minimap")]
    [SerializeField] private Toggle minimapToggle;  // <-- שדה חדש

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
        if (playerController == null)
        {
            Debug.LogError("MinimapBehavior: please assign playerController in Inspector!");
            enabled = false;
            return;
        }
        if (minimapToggle == null)
        {
            Debug.LogError("MinimapBehavior: please assign minimapToggle in Inspector!");
            enabled = false;
            return;
        }

        // הכוונת הפיבוט והאנקרים למרכז
        minimapRect.pivot = new Vector2(0.5f, 0.5f);
        minimapRect.anchorMin = minimapRect.anchorMax = new Vector2(0.5f, 0.5f);

        // שמירת הסקיילים
        originalScale = minimapRect.localScale;
        targetScale = originalScale * zoomFactor;

        // מצב התחלתי (מקופל)
        isZoomed = false;
        minimapRect.localScale = originalScale;
        minimapRect.anchoredPosition = collapsedPosition;

        // כוונן את ה-Toggle כך שישקף את המצב ההתחלתי, בלי לירות אירוע
        minimapToggle.SetIsOnWithoutNotify(isZoomed);

        // הירשם לאירוע שינוי ערך ה-Toggle
        minimapToggle.onValueChanged.AddListener(HandleToggleValueChanged);
    }

    void Update()
    {
        // אנימציית מעבר
        if (timer > transitionDuration) return;
        timer += Time.deltaTime;
        float t = Mathf.SmoothStep(0f, 1f, timer / transitionDuration);

        // קבע נקודות התחלה וסיום לפי המצב הרצוי
        Vector3 startScale = isZoomed ? originalScale : targetScale;
        Vector3 endScale = isZoomed ? targetScale : originalScale;
        Vector2 startPos = isZoomed ? collapsedPosition : expandedPosition;
        Vector2 endPos = isZoomed ? expandedPosition : collapsedPosition;

        minimapRect.localScale = Vector3.Lerp(startScale, endScale, t);
        minimapRect.anchoredPosition = Vector2.Lerp(startPos, endPos, t);
    }

    private void HandleToggleValueChanged(bool newValue)
    {
        // נוודא שאין אובייקט UI מסומן
        EventSystem.current.SetSelectedGameObject(null);

        // התחל מעבר
        isZoomed = newValue;
        timer = 0f;

        // נהל קלט שחקן
        if (isZoomed)
            playerController.DisableInputs();
        else
            playerController.EnableInputs();
    }
}
