using UnityEngine;

public class ChangingInventory : MonoBehaviour
{
    [SerializeField] private PlayerInventoryHolder _inventoryHolder;
    [SerializeField] private ViewInventoryNotifier[] _views;

    private void OnEnable()
    {
        _inventoryHolder.InventorySystem.OnItemDataChanged += ShowNotification;
    }

    private void OnDisable()
    {
        _inventoryHolder.InventorySystem.OnItemDataChanged -= ShowNotification;
    }

    private void ShowNotification(InventoryItemData itemData, int amount)
    {
        foreach (var item in _views)
        {
            if(item.gameObject.activeInHierarchy == false)
            {
                item.gameObject.SetActive(true);
                item.Init(itemData, amount);
                return;
            }
        }
    }
}
