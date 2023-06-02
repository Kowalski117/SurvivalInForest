using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(UniqueID))]
public class ExchangeKeeper : MonoBehaviour, IInteractable
{
    [SerializeField] private ExchangerItemList _shopItemsHeld;
    [SerializeField] private ExchangeSystem _shopSystem;

    public UnityAction<IInteractable> OnInteractionComplete { get; set; }

    public void EndInteraction()
    {

    }

    public void Interact(Interactor interactor, out bool interactSuccessfull)
    {
        var playerInventory = interactor.GetComponent<PlayerInventoryHolder>();

        if (playerInventory != null)
        {
            OnInteractionComplete?.Invoke(this);
            interactSuccessfull = true;
        }
        else
        {
            interactSuccessfull = false;
            Debug.LogError("Player inventory not found");
        }
    }
}

