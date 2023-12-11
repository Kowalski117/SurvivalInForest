using UnityEngine;
using UnityEngine.UI;

public class ValueMetricView : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private Color _minAmountColor;

    private Color _defoultColor;

    private void Awake()
    {
        _defoultColor = _image.color;
    }

    protected void UpdateFillAmount(float amount)
    {
        _image.fillAmount = amount;
        _image.color = Color.Lerp(_minAmountColor, _defoultColor, (amount - 0.3f) / 0.4f);
    }
}
