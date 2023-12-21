using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class CraftSlotView : MonoBehaviour
{
    [SerializeField] protected Image CraftedIcon;
    [SerializeField] protected TMP_Text CraftedName;
    [SerializeField] protected TMP_Text CraftedTimeText;
    [SerializeField] protected IngridientSlotView SlotIngridientPrefab;
    [SerializeField] protected Transform IngridientsContainer;
    [SerializeField] protected Button CraftedButton;

    protected List<IngridientSlotView> IngridientSlots = new List<IngridientSlotView>();
    protected PlayerInventoryHolder InventoryHolder;
    protected DelayWindow LoadingWindow;
    protected CraftingÑategory CraftingÑategory;

    protected DateTime CraftedTime;

    public CraftingÑategory Category => CraftingÑategory;

    public void UpdateRecipe()
    {
        foreach (var ingridient in IngridientSlots)
        {
            ingridient.UpdateAmount(InventoryHolder);
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

    protected abstract void UpdateLanguage();
}

