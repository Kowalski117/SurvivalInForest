using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryDescriptionUI : MonoBehaviour
{
    [SerializeField] Image _iconImage;
    [SerializeField] TMP_Text _titleText;
    [SerializeField] TMP_Text _descriptionText;

    [SerializeField] private Button _useButton;
    [SerializeField] private Button _discardButton;

    [SerializeField] private InventoryOperator _inventoryOperator;
    [SerializeField] private SurvivalHandler _survivalHandler;

    [SerializeField] private ParamertView[] _paramertsView;

    private InventorySlotUI _currentSlot;
    private InventorySlotUI _previousSlot;

    private void Awake()
    {
        ResetDescription();
    }

    private void OnEnable()
    {
        _useButton.onClick.AddListener(UseItemButtonClick);
        _discardButton.onClick.AddListener(DiscardButtonClick);
    }

    private void OnDisable()
    {
        _useButton.onClick.RemoveListener(UseItemButtonClick);
        _discardButton.onClick.RemoveListener(DiscardButtonClick);
    }

    public void ResetDescription()
    {
        _iconImage.gameObject.SetActive(false);
        _titleText.text = "";
        _descriptionText.text = "";
        _useButton.gameObject.SetActive(false);
        _discardButton.gameObject.SetActive(false);
        ClearParametrs();
    }

    public void SetDescription(InventorySlotUI inventorySlotUI)
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
            _iconImage.sprite = inventorySlotUI.AssignedInventorySlot.ItemData.Icon;
            _titleText.text = inventorySlotUI.AssignedInventorySlot.ItemData.DisplayName;
            _descriptionText.text = inventorySlotUI.AssignedInventorySlot.ItemData.Description;

            if (inventorySlotUI.AssignedInventorySlot.ItemData.Type == ItemType.Food)           
                _useButton.gameObject.SetActive(true);           
            else            
                _useButton.gameObject.SetActive(false); 
            
            _discardButton.gameObject.SetActive(true);

            TurnOnParametrs(inventorySlotUI.AssignedInventorySlot);
        }
        else
        {
            ResetDescription();
        }
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
            SetParametr(ParamertType.GorenjeTime, itemData.GorenjeTime * 60);
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
                SetParametr(ParamertType.Armor, GetInterest(clothesItemData.Protection, _survivalHandler.PlayerHealth.ProtectionValue.Protection));
            if (clothesItemData.Boost > 0)
                SetParametr(ParamertType.Speed, clothesItemData.Boost);
        }
    }

    private void SetFoodParameters(InventoryItemData itemData)
    {
        if (itemData is FoodItemData foodItemData)
        {
            //if (foodItemData.AmountSatiety > 0)
                SetParametr(ParamertType.Satiety, GetInterest(foodItemData.AmountSatiety * _currentSlot.AssignedInventorySlot.Durability, _survivalHandler.Hunger.MaxValueInHours));
            //if (foodItemData.AmountWater > 0)
                SetParametr(ParamertType.Thirst, GetInterest(foodItemData.AmountWater * _currentSlot.AssignedInventorySlot.Durability, _survivalHandler.Thirst.MaxValueInHours));
            //if (foodItemData.AmountHealth > 0)
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
            paramert.UpdateParametr(type, value);
        }
    }

    private int GetInterest( float currentValue, float maxValue)
    {
        if(maxValue == 0)
            return 0;

        return Mathf.RoundToInt((currentValue / maxValue) * 100.0f);
    }

    private void UseItemButtonClick()
    {
        _survivalHandler.Eat(_currentSlot.AssignedInventorySlot);

        if (_currentSlot.AssignedInventorySlot.ItemData == null)
            ResetDescription();
    }

    private void DiscardButtonClick()
    {
        _inventoryOperator.RemoveItem(_currentSlot);

        if (_currentSlot.AssignedInventorySlot.ItemData == null)
            ResetDescription();
    }
}
