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
            EndInteraction();
            interactSuccessfull = true;
        }
        else
        {
            interactSuccessfull = false;
        }
    }
}
