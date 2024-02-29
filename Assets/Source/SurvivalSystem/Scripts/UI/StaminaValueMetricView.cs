using UnityEngine;
using UnityEngine.UI;

public class StaminaValueMetricView : MonoBehaviour
{
    [SerializeField] private StaminaAttribute _survivalAttribute;
    [SerializeField] private Image _image;

    private void OnEnable()
    {
        _survivalAttribute.OnValueChanged += UpdateFillAmount;
    }

    private void OnDisable()
    {
        _survivalAttribute.OnValueChanged -= UpdateFillAmount;
    }

    private void UpdateFillAmount(float percent)
    {
        _image.fillAmount = percent;
    }
}
