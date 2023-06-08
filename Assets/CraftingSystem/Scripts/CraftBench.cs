using UnityEngine;
using UnityEngine.Events;

public class CraftBench : MonoBehaviour, IInteractable
{
    [SerializeField] private CraftingÑategory _craftingÑategory;

    private PlayerInventoryHolder _playerInventoryHolder;

    public static UnityAction<CraftingÑategory> OnCraftingDisplayRequested;
    public UnityAction<IInteractable> OnInteractionComplete { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public CraftingÑategory CraftingÑategory => _craftingÑategory;

    public void EndInteraction()
    {

    }

    public void Interact(Interactor interactor, out bool interactSuccessfull)
    {
        _playerInventoryHolder = interactor.PlayerInventoryHolder;

        if (_playerInventoryHolder != null)
        {
            OnCraftingDisplayRequested?.Invoke(_craftingÑategory);
            EndInteraction();
            interactSuccessfull = true;
        }
        else
        {
            interactSuccessfull = false;
        }
    }
}
