using UnityEngine.EventSystems;
using UnityEngine;

public class DragItem : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private RectTransform _rectTransform;
    private CanvasGroup _canvasGroup;
    private Vector2 _initialPosition;
    private Transform _originalParent;

    private Transform _currentParent;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _canvasGroup = GetComponent<CanvasGroup>();
        _originalParent = transform.parent;
        _currentParent = _originalParent;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _initialPosition = _rectTransform.anchoredPosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (_currentParent.GetComponent<InventorySlotUI>().AssignedInventorySlot.ItemData == null)
        {
            return;
        }

        _canvasGroup.blocksRaycasts = false;
        transform.SetParent(_originalParent.parent);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _canvasGroup.blocksRaycasts = true;

        if (transform.parent == _originalParent.parent)
        {
            _rectTransform.anchoredPosition = _initialPosition;
        }

        transform.SetParent(_currentParent);
    }

    public void OnDrag(PointerEventData eventData)
    {
        _rectTransform.anchoredPosition += eventData.delta / transform.lossyScale;
    }
}

