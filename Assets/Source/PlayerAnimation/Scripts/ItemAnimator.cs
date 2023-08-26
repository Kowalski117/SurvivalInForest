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

    public void SetTrigger(string name)
    {
        _animatorHand.SetTrigger(name);
    }

    public void SetBool(string name, bool value)
    {
        _animatorHand.SetBool(name, value);
    }

    public void SetFloat(string name, float value)
    {
        _animatorHand.SetFloat(name, value);
    }
}
