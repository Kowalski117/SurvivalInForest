using System.Collections.Generic;
using UnityEngine;

public class InventoryOperator : MonoBehaviour
{
    [SerializeField] private PlayerInventoryHolder _playerInventoryHolder;
    [SerializeField] private Transform _removeItemPoint;

    private void OnEnable()
    {
        InventorySlotUI.OnItemRemove += RemoveItem;
    }

    private void OnDisable()
    {
        InventorySlotUI.OnItemRemove -= RemoveItem;
    }

    public void RemoveItem(InventorySlotUI inventorySlot)
    {
        if (_playerInventoryHolder.InventorySystem.GetItemCount(inventorySlot.AssignedInventorySlot.ItemData) >= 0)
        {
            for (int i = 0; i < inventorySlot.AssignedInventorySlot.Size; i++)
            {
                InstantiateItem(inventorySlot.AssignedInventorySlot.ItemData, inventorySlot.AssignedInventorySlot.Durability);
            }
            _playerInventoryHolder.RemoveInventory(inventorySlot.AssignedInventorySlot, inventorySlot.AssignedInventorySlot.Size);

            if (inventorySlot.AssignedInventorySlot.ItemData == null)
                inventorySlot.TurnOffHighlight();
        }
    }

    public void InstantiateItem(InventoryItemData itemData, float durability)
    {
        if (itemData.ItemPrefab != null)
        {
            ItemPickUp itemPickUp = Instantiate(itemData.ItemPrefab, _removeItemPoint.position, Quaternion.identity);
            itemPickUp.GenerateNewID();
            itemPickUp.UpdateDurability(durability);
        }
    }

    public List<InventoryItem> GetItemsWithInsufficientSpace(InventoryItem[] items)
    {
        List<InventoryItem> itemsWithInsufficientSpace = new List<InventoryItem>();

        foreach (var itemData in items)
        {
            int remainingAmount = itemData.Amount; // Сначала устанавливаем оставшееся количество на максимальное количество предметов.

            while (remainingAmount > 0) // Пока есть оставшееся количество предметов.
            {
                bool addedSuccessfully = _playerInventoryHolder.AddToInventory(itemData.ItemData, 1, itemData.ItemData.Durability);

                if (!addedSuccessfully)
                {
                    // Если не удалось добавить ни одного предмета, добавляем оставшееся количество, которое можно вместить, в список itemsWithInsufficientSpace.
                    itemsWithInsufficientSpace.Add(new InventoryItem(itemData.ItemData, remainingAmount));
                    break; // Завершаем цикл, так как не можем добавить больше этого предмета.
                }
                else
                {
                    // Уменьшаем оставшееся количество предметов на 1, так как был успешно добавлен один предмет в инвентарь.
                    remainingAmount--;
                }
            }
        }

        return itemsWithInsufficientSpace;
    }
}
