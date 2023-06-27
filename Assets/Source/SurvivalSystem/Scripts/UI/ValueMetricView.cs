using UnityEngine;
using UnityEngine.UI;

public class ValueMetricView : MonoBehaviour
{
    [SerializeField] private SurvivalAttribute _survivalAttribute;
    [SerializeField] private Image _image;

    private void OnEnable()
    {
        _survivalAttribute.OnValueChanged += UpdateFillAmount;
    }

    private void OnDisable()
    {
        _survivalAttribute.OnValueChanged += UpdateFillAmount;
    }

    private void UpdateFillAmount(float amount)
    {
        _image.fillAmount = amount;
    }
}
