using UnityEngine;
using UnityEngine.UI;

public class HealthMetricView : MonoBehaviour
{
    [SerializeField] private PlayerHealth _health;
    [SerializeField] private Image _image;

    private void OnEnable()
    {
        _health.OnHealthChanged += UpdateFillAmount;
    }

    private void OnDisable()
    {
        _health.OnHealthChanged += UpdateFillAmount;
    }

    protected void UpdateFillAmount(float amount)
    {
        _image.fillAmount = amount;
    }
}
