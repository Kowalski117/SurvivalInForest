using StarterAssets;
using UnityEngine;
using UnityEngine.EventSystems;

public class SprintButton : MonoBehaviour, IPointerUpHandler
{
    private UICanvasControllerInput _canvasControllerInput;
    private bool _isPressed;
    private void Awake()
    {
        _canvasControllerInput = GetComponentInParent<UICanvasControllerInput>();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _isPressed = !_isPressed;
        _canvasControllerInput.VirtualSprintInput(_isPressed);
    }
}
