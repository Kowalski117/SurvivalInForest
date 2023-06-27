using UnityEngine;
using UnityEngine.UI;

public class ValueMetricView : MonoBehaviour
{
    [SerializeField] private SurvivalAttribute _survivalAttribute;
    [SerializeField] private Image _image;

    private void FixedUpdate()
    {
        _image.fillAmount = _survivalAttribute.ValuePercent;
    }
}
