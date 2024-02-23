using UnityEngine;

public class AttributeMetricView : ValueMetricView
{
    [SerializeField] private SurvivalAttribute _survivalAttribute;

    private void OnEnable()
    {
        _survivalAttribute.OnValueChanged += UpdateFillAmount;
    }

    private void OnDisable()
    {
        _survivalAttribute.OnValueChanged += UpdateFillAmount;
    }
}
