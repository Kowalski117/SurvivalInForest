using UnityEngine;

[RequireComponent (typeof(InventorySlotUI))]
public class ClothingSlot : MonoBehaviour
{
    [SerializeField] private PlayerEquipmentHandler _playerEquipmentHandler;
    [SerializeField] private PlayerHealth _playerHealth;

    private InventorySlotUI _inventorySlotUI;
    private ClothesItemData _clothesItemData;

    private void Awake()
    {
        _inventorySlotUI = GetComponent<InventorySlotUI>();
    }

    private void OnEnable()
    {
        _inventorySlotUI.OnItemUpdated += UpdateItem;
        _playerHealth.OnEnemyDamageDone += TakeDamage;
    }

    private void OnDisable()
    {
        _inventorySlotUI.OnItemUpdated -= UpdateItem;
        _playerHealth.OnEnemyDamageDone -= TakeDamage;
    }

    private void Update()
    {
        if (_clothesItemData != null && _clothesItemData.TypeOfClothingUse == TypeOfClothingUse.AmountOfTime)
        {
            _playerEquipmentHandler.UpdateDurabilityWithGameTime(_inventorySlotUI.AssignedInventorySlot);
            Deselect();
        }
    }

    private void TakeDamage()
    {
        if (_clothesItemData != null && _clothesItemData.TypeOfClothingUse == TypeOfClothingUse.AmountOfDamage)
        {
            _playerEquipmentHandler.UpdateDurabilityItem(_inventorySlotUI.AssignedInventorySlot);
            Deselect();
        }
    }

    private void UpdateItem(InventorySlotUI inventorySlotUI)
    {
        if (inventorySlotUI.AssignedInventorySlot.ItemData != null)
        {
            if (_inventorySlotUI.AssignedInventorySlot.ItemData is ClothesItemData clothesItemData)          
                _clothesItemData = clothesItemData; 
        }
        else
            _clothesItemData = null;
    }

    private void Deselect()
    {
        if (_inventorySlotUI.AssignedInventorySlot.Durability <= 0)
            _inventorySlotUI.TurnOffHighlight();
    }
}
