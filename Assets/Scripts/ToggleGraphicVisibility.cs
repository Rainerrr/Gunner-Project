using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class ToggleGraphicVisibility : MonoBehaviour
{
    [Tooltip("The Image (or any Graphic) you want to hide/show.")]
    public Graphic targetGraphic;

    private Toggle _toggle;

    void Awake()
    {
        _toggle = GetComponent<Toggle>();
        _toggle.onValueChanged.AddListener(OnToggleChanged);
        // init
        OnToggleChanged(_toggle.isOn);
    }

    void OnToggleChanged(bool isOn)
    {
        if (targetGraphic == null) return;
        var c = targetGraphic.color;
        c.a = isOn ? 1f : 0f;
        targetGraphic.color = c;
    }
}
