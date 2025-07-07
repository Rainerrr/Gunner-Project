using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class TargetTypeToggleInfo : MonoBehaviour
{
    [SerializeField] private Toggle toggle;
    [SerializeField] public TargetType targetType;

    public Toggle Toggle => toggle;
    public TargetType Type => targetType;

    private void Reset()
    {
        toggle = GetComponent<Toggle>();
    }
}
