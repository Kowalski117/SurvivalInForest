using UnityEngine;

public class ItemAnimator : MonoBehaviour
{
    [SerializeField] private InventoryItemData _itemData;
    [SerializeField] private Animator _animatorHand;

    public InventoryItemData ItemData => _itemData;
    public Animator HandAnimator => _animatorHand;

    public void ToggleItem(bool isActive)
    {
        if (_animatorHand != null)
            _animatorHand.gameObject.SetActive(isActive);
    }
}
