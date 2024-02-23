using UnityEngine;
using UnityEngine.UI;

public class ButtonSprint : MonoBehaviour
{
    [SerializeField] private AnimationUI _barStamina;
    [SerializeField] private Button _sprintButton;

    private void Awake()
    {
        _barStamina.CloseAnimation();
    }

    private void OnEnable()
    {
        _sprintButton.onClick.AddListener(ToggleActiveBar);
    }

    private void OnDisable()
    {
        _sprintButton.onClick.RemoveListener(ToggleActiveBar);
    }

    private void ToggleActiveBar()
    {
        if (_barStamina.IsOpen)
            _barStamina.CloseAnimation();
        else
            _barStamina.OpenAnimation();
    }
}
