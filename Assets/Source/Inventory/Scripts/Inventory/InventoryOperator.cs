using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryOperator : MonoBehaviour
{
    [SerializeField] private PlayerInventoryHolder _playerInventoryHolder;
    [SerializeField] private Transform _removeItemPoint;
    [SerializeField] private float _creationDelay = 0.2f;

    private Coroutine _spawnItemCoroutine;
    private Coroutine _spawnItemsCoroutine;
    private WaitForSeconds _creationWait;

    private void Awake()
    {
        _creationWait = new WaitForSeconds(_creationDelay);
    }

    private void OnEnable()
    {
        InventorySlotUI.OnItemRemoved += RemoveItems;
    }

    private void OnDisable()
    {
        InventorySlotUI.OnItemRemoved -= RemoveItems;
    }

    public void RemoveItems(InventorySlotUI inventorySlot)
    {
        if (_playerInventoryHolder.InventorySystem.GetItemCount(inventorySlot.AssignedInventorySlot.ItemData) >= 0)
        {
            StartCoroutine(CreateItemWithDelay(inventorySlot.AssignedInventorySlot.ItemData, inventorySlot.AssignedInventorySlot.Durability, inventorySlot.AssignedInventorySlot.Size));

            _playerInventoryHolder.RemoveSlot(inventorySlot.AssignedInventorySlot, inventorySlot.AssignedInventorySlot.Size);

            if (inventorySlot.AssignedInventorySlot.ItemData == null)
                inventorySlot.TurnOffHighlight();
        }
    }

    public void RemoveItem(InventorySlotUI inventorySlot)
    {
        if (_playerInventoryHolder.InventorySystem.GetItemCount(inventorySlot.AssignedInventorySlot.ItemData) >= 0)
        {
            StartCoroutine(CreateItemWithDelay(inventorySlot.AssignedInventorySlot.ItemData, inventorySlot.AssignedInventorySlot.Durability, 1));
            _playerInventoryHolder.RemoveSlot(inventorySlot.AssignedInventorySlot, 1);

            if (inventorySlot.AssignedInventorySlot.ItemData == null)
                inventorySlot.TurnOffHighlight();
        }
    }

    public void StartCreateItem(InventoryItemData itemData, float durability, int itemCount)
    {
        if(_spawnItemCoroutine != null)
        {
            StopCoroutine(_spawnItemCoroutine);
            _spawnItemCoroutine = null;
        }

        _spawnItemCoroutine = StartCoroutine(CreateItemWithDelay(itemData, durability, itemCount)); 
    }

    public void StartCreateItems(List<InventorySlot> itemData)
    {
        if (_spawnItemsCoroutine != null)
        {
            StopCoroutine(_spawnItemsCoroutine);
            _spawnItemsCoroutine = null;
        }

        _spawnItemsCoroutine = StartCoroutine(CreateItemsWithDelay(itemData));
    }

    private IEnumerator CreateItemWithDelay(InventoryItemData itemData, float durability, int itemCount)
    {
        for (int i = 0; i < itemCount; i++)
        {
            InstantiateItem(itemData, durability);
            yield return _creationWait;
        }
    }

    private IEnumerator CreateItemsWithDelay(List<InventorySlot> itemData)
    {
        for (int i = 0; i < itemData.Count; i++)
        {
            StartCreateItem(itemData[i].ItemData, itemData[i].Durability, itemData[i].Size);
            yield return new WaitForSeconds(_creationDelay * itemData[i].Size);
        }
    }

    public void InstantiateItem(InventoryItemData itemData, float durability)
    {
        if (itemData != null && itemData.ItemPrefab != null)
        {
            ItemPickUp itemPickUp = Instantiate(itemData.ItemPrefab, _removeItemPoint.position, Quaternion.identity);
            itemPickUp.GenerateNewID();
            itemPickUp.UpdateDurability(durability);
        }
    }
}
