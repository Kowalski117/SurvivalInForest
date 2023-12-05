using StarterAssets;
using UnityEngine;
using UnityEngine.EventSystems;

public class AttackButton : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    private UICanvasControllerInput _canvasControllerInput;

    private void Awake()
    {
        _canvasControllerInput = GetComponentInParent<UICanvasControllerInput>();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _canvasControllerInput.Attack(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _canvasControllerInput.Attack(true);
    }
}
