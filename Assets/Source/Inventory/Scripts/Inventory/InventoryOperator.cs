using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryOperator : MonoBehaviour
{
    [SerializeField] private PlayerInventoryHolder _playerInventoryHolder;
    [SerializeField] private Transform _removeItemPoint;
    [SerializeField] private float _creationDelay = 0.2f;

    private Coroutine _coroutine;

    private void OnEnable()
    {
        InventorySlotUI.OnItemRemove += RemoveItems;
    }

    private void OnDisable()
    {
        InventorySlotUI.OnItemRemove -= RemoveItems;
    }

    public void RemoveItems(InventorySlotUI inventorySlot)
    {
        if (_playerInventoryHolder.InventorySystem.GetItemCount(inventorySlot.AssignedInventorySlot.ItemData) >= 0)
        {
            StartCoroutine(CreateItemsWithDelay(inventorySlot.AssignedInventorySlot.ItemData, inventorySlot.AssignedInventorySlot.Durability, inventorySlot.AssignedInventorySlot.Size));

            _playerInventoryHolder.RemoveInventory(inventorySlot.AssignedInventorySlot, inventorySlot.AssignedInventorySlot.Size);

            if (inventorySlot.AssignedInventorySlot.ItemData == null)
                inventorySlot.TurnOffHighlight();
        }
    }

    public void RemoveItem(InventorySlotUI inventorySlot)
    {
        if (_playerInventoryHolder.InventorySystem.GetItemCount(inventorySlot.AssignedInventorySlot.ItemData) >= 0)
        {
            StartCoroutine(CreateItemsWithDelay(inventorySlot.AssignedInventorySlot.ItemData, inventorySlot.AssignedInventorySlot.Durability, 1));
            _playerInventoryHolder.RemoveInventory(inventorySlot.AssignedInventorySlot, 1);

            if (inventorySlot.AssignedInventorySlot.ItemData == null)
                inventorySlot.TurnOffHighlight();
        }
    }

    public void StartCreateItems(InventoryItemData itemData, float durability, int itemCount)
    {
        if(_coroutine != null)
        {
            StopCoroutine(_coroutine);
            _coroutine = null;
        }

        /*_coroutine =*/ StartCoroutine(CreateItemsWithDelay(itemData, durability, itemCount)); 
    }

    public void StartCreateItems(List<InventorySlot> itemData)
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
            _coroutine = null;
        }

        /*_coroutine =*/ StartCoroutine(CreateItemsWithDelay(itemData));
    }

    private IEnumerator CreateItemsWithDelay(InventoryItemData itemData, float durability, int itemCount)
    {
        for (int i = 0; i < itemCount; i++)
        {
            InstantiateItem(itemData, durability);
            yield return new WaitForSeconds(_creationDelay);
        }
    }

    private IEnumerator CreateItemsWithDelay(List<InventorySlot> itemData)
    {
        for (int i = 0; i < itemData.Count; i++)
        {
            StartCreateItems(itemData[i].ItemData, itemData[i].Durability, itemData[i].Size);
            yield return new WaitForSeconds(_creationDelay * itemData[i].Size);
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
}
