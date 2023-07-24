using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DelayIcon : MonoBehaviour
{
    [SerializeField] private Interactor _interactor;
    [SerializeField] private Image _delayIcon;
    [SerializeField] private TMP_Text _nameItemText;

    private void OnEnable()
    {
        _interactor.OnTimeUpdate += UpdateFillAmount;
    }

    private void OnDisable()
    {
        _interactor.OnTimeUpdate -= UpdateFillAmount;
    }

    private void UpdateFillAmount(float amount, string name)
    {
        _delayIcon.fillAmount = amount;
        _nameItemText.text = name;
    }
}
