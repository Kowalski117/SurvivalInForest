using UnityEngine;
using UnityEngine.UI;

public class DelayIcon : MonoBehaviour
{
    [SerializeField] private Interactor _interactor;
    [SerializeField] private Image _delayIcon;

    private void OnEnable()
    {
        _interactor.OnTimeUpdate += UpdateFillAmount;
    }

    private void OnDisable()
    {
        _interactor.OnTimeUpdate -= UpdateFillAmount;
    }

    private void UpdateFillAmount(float amount)
    {
        _delayIcon.fillAmount = amount;
    }
}
