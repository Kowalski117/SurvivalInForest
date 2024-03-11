using UnityEngine;
using UnityEngine.UI;

public class DelayIcon : MonoBehaviour
{
    [SerializeField] private InteractorItem _interactorItem;
    [SerializeField] private Image _delayIcon;

    private void OnEnable()
    {
        _interactorItem.OnTimeUpdated += UpdateFillAmount;
    }

    private void OnDisable()
    {
        _interactorItem.OnTimeUpdated -= UpdateFillAmount;
    }

    private void UpdateFillAmount(float amount)
    {
        _delayIcon.fillAmount = amount;
    }
}
