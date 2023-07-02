using UnityEngine;
using UnityEngine.UI;

public class ValueMetricView : MonoBehaviour
{
    [SerializeField] private SurvivalAttribute SurvivalAttribute;
    [SerializeField] private Image _image;

    private void OnEnable()
    {
        SurvivalAttribute.OnValueChanged += UpdateFillAmount;
    }

    private void OnDisable()
    {
        SurvivalAttribute.OnValueChanged += UpdateFillAmount;
    }

    private void UpdateFillAmount(float amount)
    {
        _image.fillAmount = amount;
    }
}
