using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(InventoryDescriptionView))]
public class InventoryDescription : MonoBehaviour
{
    [SerializeField] private InventoryOperator _inventoryOperator;
    [SerializeField] private SurvivalHandler _survivalHandler;
    [SerializeField] private ParamertView[] _paramertsView;

    [SerializeField] private Transform _iconImage;
    [SerializeField] private Transform _discardButton;
    [SerializeField] private Transform _useButton;

    private InventoryDescriptionView _inventoryDescriptionView;
    private InventorySlotUI _currentSlot;
    private InventorySlotUI _previousSlot;

    private float _seconds = 60;
    private float _maxPercent = 100;

    private void Awake()
    {
        _inventoryDescriptionView = GetComponent<InventoryDescriptionView>();
    }

    private void Start()
    {
        Clear();
    }

    private void OnEnable()
    {
        _inventoryDescriptionView.OnUsedButton += UseItem;
        _inventoryDescriptionView.OnDiscardedButton += DiscardItem;
    }

    private void OnDisable()
    {
        _inventoryDescriptionView.OnUsedButton -= UseItem;
        _inventoryDescriptionView.OnDiscardedButton -= DiscardItem;
    }

    public void Clear()
    {
        _iconImage.gameObject.SetActive(false);
        _inventoryDescriptionView.Clear();
        _discardButton.gameObject.SetActive(false);
        _useButton.gameObject.SetActive(false);
        ClearParametrs();
    }

    public void SetInfo(InventorySlotUI inventorySlotUI)
    {
        if (_currentSlot != null && _currentSlot.AssignedInventorySlot.ItemData != null && _currentSlot != inventorySlotUI)
            _currentSlot.ToggleHighlight();

        if (inventorySlotUI.AssignedInventorySlot.ItemData != null)
        {
            _previousSlot = _currentSlot;

            if (_previousSlot != null)
                _previousSlot.TurnOffHighlight();

            _currentSlot = inventorySlotUI;

            if (_currentSlot != null)
                _currentSlot.ToggleHighlight();

            _iconImage.gameObject.SetActive(true);

            _inventoryDescriptionView.Init(inventorySlotUI.AssignedInventorySlot.ItemData.Icon, inventorySlotUI.AssignedInventorySlot.ItemData.DisplayName, inventorySlotUI.AssignedInventorySlot.ItemData.Description);

            if (inventorySlotUI.AssignedInventorySlot.ItemData.Type == ItemType.Food)           
                _useButton.gameObject.SetActive(true);           
            else            
                _useButton.gameObject.SetActive(false); 
            
            _discardButton.gameObject.SetActive(true);

            TurnOnParametrs(inventorySlotUI.AssignedInventorySlot);
        }
        else
            Clear();
    }

    private void TurnOnParametrs(InventorySlot inventorySlot)
    {
        ClearParametrs();

        if (inventorySlot.ItemData != null)
        {
            if (inventorySlot.ItemData.Type == ItemType.Item)
                SetItemParameters(inventorySlot.ItemData);
            else if (inventorySlot.ItemData.Type == ItemType.Seed)
                SetSeedParameters(inventorySlot.ItemData);
            else if (inventorySlot.ItemData.Type == ItemType.Food)
                SetFoodParameters(inventorySlot.ItemData);
            else if (inventorySlot.ItemData.Type == ItemType.Tool)
                SetToolParameters(inventorySlot.ItemData);
            else if (inventorySlot.ItemData.Type == ItemType.Weapon)
                SetWeaponParameters(inventorySlot.ItemData);
            else if (inventorySlot.ItemData is ClothesItemData)
                SetClothesParameters(inventorySlot.ItemData);

            if (inventorySlot.Durability > 1 && inventorySlot.ItemData.Type != ItemType.Food)
                SetParametr(ParamertType.Endurance, GetInterest(_currentSlot.AssignedInventorySlot.Durability, inventorySlot.ItemData.Durability));
        }
    }

    private void ClearParametrs()
    {
        foreach (var paramert in _paramertsView)
        {
            paramert.gameObject.SetActive(false);
        }
    }

    private void SetItemParameters(InventoryItemData itemData)
    {
        if (itemData.GorenjeTime > 0)
            SetParametr(ParamertType.GorenjeTime, itemData.GorenjeTime * _seconds);
    }

    private void SetSeedParameters(InventoryItemData itemData)
    {
        if (itemData is SeedItemData seedItemData)
        {
            if (seedItemData.GrowthTime > 0)
                SetParametr(ParamertType.GrowthTime, seedItemData.GrowthTime);
        }
    }

    private void SetClothesParameters(InventoryItemData itemData)
    {
        if (itemData is ClothesItemData clothesItemData)
        {
            if (clothesItemData.Protection > 0)
                SetParametr(ParamertType.Armor, GetInterest(clothesItemData.Protection, _survivalHandler.PlayerHealth.Protection.ProtectionValue));
            if (clothesItemData.Boost > 0)
                SetParametr(ParamertType.Speed, clothesItemData.Boost);
        }
    }

    private void SetFoodParameters(InventoryItemData itemData)
    {
        if (itemData is FoodItemData foodItemData)
        {
            SetParametr(ParamertType.Satiety, GetInterest(foodItemData.AmountSatiety * _currentSlot.AssignedInventorySlot.Durability, _survivalHandler.Hunger.MaxValueInHours));
            SetParametr(ParamertType.Thirst, GetInterest(foodItemData.AmountWater * _currentSlot.AssignedInventorySlot.Durability, _survivalHandler.Thirst.MaxValueInHours));
            SetParametr(ParamertType.Helfth, GetInterest(foodItemData.AmountHealth * _currentSlot.AssignedInventorySlot.Durability, _survivalHandler.PlayerHealth.MaxHealth));
            SetParametr(ParamertType.Sleep, GetInterest(foodItemData.AmountSleep * _currentSlot.AssignedInventorySlot.Durability, _survivalHandler.Sleep.MaxValueInHours));
        }
    }

    private void SetToolParameters(InventoryItemData itemData)
    {
        if (itemData is ToolItemData toolItemData)
        {
            if (toolItemData.DamageResources > 0)
                SetParametr(ParamertType.DamageToResources, toolItemData.DamageResources);
            if (toolItemData.DamageLiving > 0)
                SetParametr(ParamertType.DamageToEnemies, toolItemData.DamageLiving);
            if (toolItemData.Speed > 0)
                SetParametr(ParamertType.ImpactSpeed, toolItemData.Speed);
        }
    }

    private void SetWeaponParameters(InventoryItemData itemData)
    {
        if (itemData is WeaponItemData weaponItemData)
        {
            if (weaponItemData.Damage > 0)
                SetParametr(ParamertType.DamageToEnemies, weaponItemData.Damage);
            if (weaponItemData.OverTimeDamage > 0)
                SetParametr(ParamertType.DamageAfterTime, weaponItemData.OverTimeDamage);
            if (weaponItemData.Speed > 0)
                SetParametr(ParamertType.ImpactSpeed, weaponItemData.Speed);
        }
    }

    private void SetParametr(ParamertType type, float value)
    {
        foreach (var paramert in _paramertsView)
        {
            paramert.UpdateInfo(type, value);
        }
    }

    private int GetInterest( float currentValue, float maxValue)
    {
        if(maxValue == 0)
            return 0;

        return Mathf.RoundToInt((currentValue / maxValue) * _maxPercent);
    }

    private void UseItem()
    {
        _survivalHandler.Eat(_currentSlot.AssignedInventorySlot);

        if (_currentSlot.AssignedInventorySlot.ItemData == null)
            Clear();
    }

    private void DiscardItem()
    {
        _inventoryOperator.RemoveItem(_currentSlot);

        if (_currentSlot.AssignedInventorySlot.ItemData == null)
            Clear();
    }
}
