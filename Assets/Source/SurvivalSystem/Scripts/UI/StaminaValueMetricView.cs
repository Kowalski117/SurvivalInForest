using UnityEngine;
using UnityEngine.UI;

public class StaminaValueMetricView : MonoBehaviour
{
    [SerializeField] private StaminaAttribute _survivalAttribute;
    [SerializeField] private Image _image;

    private void FixedUpdate()
    {
        _image.fillAmount = _survivalAttribute.ValuePercent;
    }
}
