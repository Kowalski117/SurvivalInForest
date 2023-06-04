using UnityEngine;
using UnityEngine.Events;

public class CraftBench : MonoBehaviour, IInteractable
{
    [SerializeField] private RecipeItemList _recipeItemList;

    private PlayerInventoryHolder _playerInventoryHolder;

    public static UnityAction<CraftBench> OnCraftingDisplayRequested;
    public UnityAction<IInteractable> OnInteractionComplete { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public RecipeItemList RecipeItemList => _recipeItemList;

    public void EndInteraction()
    {

    }

    public void Interact(Interactor interactor, out bool interactSuccessfull)
    {
        OnCraftingDisplayRequested?.Invoke(this);

        _playerInventoryHolder = interactor.PlayerInventoryHolder;

        if (_playerInventoryHolder != null)
        {
            //if (CheckIfCanCraft())
            //{
            //    foreach (var ingredient in _activeRecipe.CraftingIngridients)
            //    {
            //        _playerInventoryHolder.InventorySystem.RemoveItemsInventory(ingredient.ItemRequired, ingredient.AmountRequured);
            //        Debug.Log(ingredient.ItemRequired);
            //    }

            //    _playerInventoryHolder.AddToInventory(_activeRecipe.CraftedItem, _activeRecipe.CraftingAmount, true);
            //}

            EndInteraction();
            interactSuccessfull = true;
        }
        else
        {
            interactSuccessfull = false;
        }
    }

    //private bool CheckIfCanCraft()
    //{
    //    var itemsHeld = _playerInventoryHolder.InventorySystem.GetAllItemsHeld();

    //    foreach (var ingredient in _activeRecipe.CraftingIngridients)
    //    {
    //        if (!itemsHeld.TryGetValue(ingredient.ItemRequired, out int amountHeld))
    //            return false;

    //        if (amountHeld < ingredient.AmountRequured)
    //            return false;
    //    }

    //    return true;
    //}
}
