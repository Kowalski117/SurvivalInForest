using UnityEngine;
using UnityEngine.UI;

public class ProtectionValueView : MonoBehaviour
{
    [SerializeField] private ProtectionValue _value;
    [SerializeField] private Image _image;

    private void OnEnable()
    {
        _value.OnValueChanged += UpdateFillAmount;
    }

    private void OnDisable()
    {
        _value.OnValueChanged -= UpdateFillAmount;
    }

    private void UpdateFillAmount(float amount)
    {
        _image.fillAmount = amount;
    }
}
