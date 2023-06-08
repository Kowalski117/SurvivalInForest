using UnityEngine;
using UnityEngine.Events;

public class CraftBench : MonoBehaviour, IInteractable
{
    [SerializeField] private Crafting�ategory _crafting�ategory;

    private PlayerInventoryHolder _playerInventoryHolder;

    public static UnityAction<Crafting�ategory> OnCraftingDisplayRequested;
    public UnityAction<IInteractable> OnInteractionComplete { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public Crafting�ategory Crafting�ategory => _crafting�ategory;

    public void EndInteraction()
    {

    }

    public void Interact(Interactor interactor, out bool interactSuccessfull)
    {
        _playerInventoryHolder = interactor.PlayerInventoryHolder;

        if (_playerInventoryHolder != null)
        {
            OnCraftingDisplayRequested?.Invoke(_crafting�ategory);
            EndInteraction();
            interactSuccessfull = true;
        }
        else
        {
            interactSuccessfull = false;
        }
    }
}
