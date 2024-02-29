using UnityEngine;
using UnityEngine.UI;

public class ValueMetricView : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private Color _minAmountColor;

    private Color _defoultColor;

    private float _amountOffset = 0.3f;
    private float _amountRange = 0.4f;

    private void Awake()
    {
        _defoultColor = _image.color;
    }

    protected void UpdateFillAmount(float amount)
    {
        _image.fillAmount = amount;
        _image.color = Color.Lerp(_minAmountColor, _defoultColor, (amount - _amountOffset) / _amountRange);
    }
}
