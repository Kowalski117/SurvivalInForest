using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CraftSlotView : MonoBehaviour
{
    [SerializeField] protected Image CraftedIcon;
    [SerializeField] protected TMP_Text CraftedName;
    [SerializeField] protected IngridientSlotView SlotIngridientPrefab;
    [SerializeField] protected Transform IngridientsContainer;
    [SerializeField] protected Button CraftedButton;

    protected List<IngridientSlotView> IngridientSlots = new List<IngridientSlotView>();
    protected PlayerInventoryHolder InventoryHolder;
    protected LoadingWindow LoadingWindow;
    protected CraftingÑategory CraftingÑategory;

    public CraftingÑategory Category => CraftingÑategory;

    public void UpdateRecipe()
    {
        foreach (var ingridient in IngridientSlots)
        {
            ingridient.UpdateAmount(InventoryHolder);
        }
    }

    public void CloseForCrafting()
    {
        gameObject.SetActive(false);
    }

    public void OpenForCrafting()
    {
        gameObject.SetActive(true);
    }
}

