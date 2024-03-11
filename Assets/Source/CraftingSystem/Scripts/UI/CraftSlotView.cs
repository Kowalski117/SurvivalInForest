using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftSlotView : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _timeText;
    [SerializeField] private IngridientSlotView _slotIngridientPrefab;
    [SerializeField] private Transform _ingridientsContainer;
    [SerializeField] private Button _craftedButton;

    private List<IngridientSlotView> _ingridientSlots = new List<IngridientSlotView>();
    private PlayerInventoryHolder _inventoryHolder;
    private CraftingÑategory _craftingÑategory;

    private DateTime _craftedTime;
    private CraftRecipe _craftRecipe;
    private bool _isEmpty = true;

    public event Action<CraftRecipe> OnCreatedRecipeButtonClick;

    public bool IsEmpty => _isEmpty;
    public CraftingÑategory Category => _craftingÑategory;
    public CraftRecipe Recipe => _craftRecipe;

    private void OnEnable()
    {
        _craftedButton.onClick.AddListener(CreateRecipeButtonClick);
    }

    private void OnDisable()
    {
        _craftedButton.onClick.RemoveListener(CreateRecipeButtonClick);
    }

    public void Init(PlayerInventoryHolder playerInventory, CraftRecipe craftRecipe, CraftingÑategory ñategory)
    {
        _isEmpty = false;
        _craftedTime = DateTime.MinValue;
        _craftRecipe = craftRecipe;
        _inventoryHolder = playerInventory;
        _craftingÑategory = ñategory;
        _craftedTime = _craftedTime + TimeSpan.FromHours(craftRecipe.CraftingTime);
        _timeText.text = _craftedTime.ToString(GameConstants.HHmm);

        if(_craftRecipe is BuildingRecipe buildingRecipe)
        {
            _icon.sprite = buildingRecipe.BuildingData.Icon;
            _name.text = buildingRecipe.BuildingData.DisplayName;
        }
        else if(_craftRecipe is ItemRecipe itemRecipe)
        {
            _icon.sprite = itemRecipe.CraftedItem.Icon;
            _name.text = itemRecipe.CraftedItem.DisplayName;
        }


        foreach (var ingridient in craftRecipe.CraftingIngridients)
        {
            IngridientSlotView slotView = Instantiate(_slotIngridientPrefab, _ingridientsContainer);
            slotView.Init(playerInventory, ingridient.ItemRequired, ingridient.AmountRequured);
            _ingridientSlots.Add(slotView);
        }
    }

    public void UpdateRecipe()
    {
        foreach (var ingridient in _ingridientSlots)
        {
            ingridient.UpdateAmount(_inventoryHolder);
        }

        UpdateLanguage();
    }

    public void CloseForCrafting()
    {
        gameObject.SetActive(false);
    }

    public void OpenForCrafting()
    {
        gameObject.SetActive(true);
    }

    private void CreateRecipeButtonClick()
    {
        if (_inventoryHolder.CheckIfCanCraft(_craftRecipe))
            OnCreatedRecipeButtonClick?.Invoke(_craftRecipe);
    }

    private void UpdateLanguage()
    {
        if (_craftRecipe is BuildingRecipe buildingRecipe)
            _name.text = buildingRecipe.BuildingData.DisplayName;
        else if (_craftRecipe is ItemRecipe itemRecipe)
            _name.text = itemRecipe.CraftedItem.DisplayName;
    }
}

