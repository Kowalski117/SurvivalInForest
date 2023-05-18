using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryUIInteraction : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private ItemInfoPanel _itemInfoPanel;
    [SerializeField] private Transform _iconTransform;

    private Transform _draggedItemParent;

    private Slot _slot;

    private void Awake()
    {
        _slot = GetComponent<Slot>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _slot = eventData.pointerPress.GetComponent<Slot>();

        if (_slot == null || _slot.ItemInSlot == null)
            return;

        _draggedItemParent = transform;
        _iconTransform = _draggedItemParent.GetComponentInChildren<ImageTransform>().transform;
        _slot.AmountInSlotText.enabled = false;
        _iconTransform.SetParent(transform.root);
        _iconTransform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(_iconTransform != null)
        {
            _iconTransform.position = Input.mousePosition;
            _slot.Icon.raycastTarget = false;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Slot clickedSlot = eventData.pointerPress.GetComponent<Slot>();
        if (clickedSlot == null || clickedSlot.ItemInSlot == null)
            return;

        _itemInfoPanel.SetItemInfo(clickedSlot);
        _itemInfoPanel.Open();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (_slot == null)
            return;
        _draggedItemParent = transform;
        _slot.Icon.raycastTarget = true;
        _slot.AmountInSlotText.enabled = true;
        if(_iconTransform != null)
        {
            _iconTransform.SetParent(_draggedItemParent);
            _iconTransform.localPosition = Vector3.zero;
        }
        _slot.SetStats();
        _iconTransform = null;
        _draggedItemParent = null;
    }
}
