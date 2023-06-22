using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(UniqueID))]
public class ExchangeKeeper : MonoBehaviour, IInteractable
{
    public static UnityAction<ExchangeKeeper> OnExchangeDisplayRequested;
    public UnityAction<IInteractable> OnInteractionComplete { get; set; }

    public void EndInteraction()
    {

    }

    public void Interact(Interactor interactor, out bool interactSuccessfull)
    {
        var playerInventory = interactor.PlayerInventoryHolder;

        if (playerInventory != null)
        {
            OnExchangeDisplayRequested?.Invoke(this);
            interactSuccessfull = true;
        }
        else
        {
            interactSuccessfull = false;
        }
    }
}

