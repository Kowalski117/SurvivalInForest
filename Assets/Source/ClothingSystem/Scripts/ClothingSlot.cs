using UnityEngine;

public class ClothingSlot : MonoBehaviour
{
    [SerializeField] private PlayerInteraction _playerInteraction;
    [SerializeField] private PlayerHealth _playerHealth;

    private InventorySlotUI _inventorySlotUI;
    private ClothesItemData _clothesItemData;

    private void Awake()
    {
        _inventorySlotUI = GetComponent<InventorySlotUI>();
    }

    private void OnEnable()
    {
        _inventorySlotUI.OnItemUpdate += UpdateCurrentSlot;
        _playerHealth.OnEnemyDamageDone += TakeDamage;
    }

    private void OnDisable()
    {
        _inventorySlotUI.OnItemUpdate -= UpdateCurrentSlot;
        _playerHealth.OnEnemyDamageDone -= TakeDamage;
    }

    private void Update()
    {
        if (_clothesItemData != null && _clothesItemData.TypeOfClothingUse == TypeOfClothingUse.AmountOfTime)
        {
            _playerInteraction.UpdateDurabilityWithGameTime(_inventorySlotUI.AssignedInventorySlot);
            DeselectSlot();
        }
    }

    private void TakeDamage()
    {
        if (_clothesItemData != null && _clothesItemData.TypeOfClothingUse == TypeOfClothingUse.AmountOfDamage)
        {
            _playerInteraction.UpdateDurabilityItem(_inventorySlotUI.AssignedInventorySlot);
            DeselectSlot();
        }
    }

    private void UpdateCurrentSlot(InventorySlotUI inventorySlotUI)
    {
        if (inventorySlotUI.AssignedInventorySlot.ItemData != null)
        {
            if (_inventorySlotUI.AssignedInventorySlot.ItemData is ClothesItemData clothesItemData)          
                _clothesItemData = clothesItemData; 
        }
        else
        {
            _clothesItemData = null;
        }
    }

    private void DeselectSlot()
    {
        if (_inventorySlotUI.AssignedInventorySlot.Durability <= 0)
            _inventorySlotUI.TurnOffHighlight();
        
    }
}
