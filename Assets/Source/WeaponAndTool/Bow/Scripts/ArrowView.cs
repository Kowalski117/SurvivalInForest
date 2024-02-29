using UnityEngine;

public class ArrowView : MonoBehaviour
{
    [SerializeField] private Bow _bow;
    [SerializeField] private PlayerInventoryHolder _playerInventoryHolder;
    [SerializeField] private InventoryItemData _arrowItemData;
    [SerializeField] private MouseItemData _itemDataMouse;
    [SerializeField] private Transform _arrowTransform;

    private bool _isActive = false;

    private void OnEnable()
    {
        _bow.OnInitialized += Enable;
        _bow.OnCleared += TurnOff;
        _playerInventoryHolder.OnItemSlotUpdated += UpdateAmount;
        _itemDataMouse.OnUpdatedSlots += UpdateAmount; 
    }

    private void OnDisable()
    {
        _bow.OnInitialized -= Enable;
        _bow.OnCleared -= TurnOff;
        _playerInventoryHolder.OnItemSlotUpdated -= UpdateAmount;
        _itemDataMouse.OnUpdatedSlots -= UpdateAmount;
    }

    private void UpdateAmount()
    {
        if(_playerInventoryHolder.InventorySystem.GetItemCount(_arrowItemData) <= 0)
            _arrowTransform.gameObject.SetActive(false);
    }

    private void TurnOff()
    {
        _isActive = false;
        _arrowTransform.gameObject.SetActive(_isActive);
    }

    private void Enable()
    {
        if (_playerInventoryHolder.InventorySystem.GetItemCount(_arrowItemData) > 0)
        {
            _isActive = true;
            _arrowTransform.gameObject.SetActive(_isActive);
        }   
    }
}
