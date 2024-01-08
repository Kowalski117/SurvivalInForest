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
        _bow.OnInitialized += EnableArrow;
        _bow.OnCleared += TurnOffArrow;
        _playerInventoryHolder.OnUpdateItemSlot += UpdateArrowAmount;
        _itemDataMouse.OnUpdatedSlots += UpdateArrowAmount; 
    }

    private void OnDisable()
    {
        _bow.OnInitialized -= EnableArrow;
        _bow.OnCleared -= TurnOffArrow;
        _playerInventoryHolder.OnUpdateItemSlot -= UpdateArrowAmount;
        _itemDataMouse.OnUpdatedSlots -= UpdateArrowAmount;
    }

    private void UpdateArrowAmount()
    {
        if(_playerInventoryHolder.InventorySystem.GetItemCount(_arrowItemData) <= 0)
            _arrowTransform.gameObject.SetActive(false);
    }

    private void TurnOffArrow()
    {
        _isActive = false;
        _arrowTransform.gameObject.SetActive(_isActive);
    }

    private void EnableArrow()
    {
        if (_playerInventoryHolder.InventorySystem.GetItemCount(_arrowItemData) > 0)
        {
            _isActive = true;
            _arrowTransform.gameObject.SetActive(_isActive);
        }   
    }

}
