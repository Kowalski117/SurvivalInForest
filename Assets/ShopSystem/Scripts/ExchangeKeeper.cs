using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(UniqueID))]
public class ExchangeKeeper : MonoBehaviour, IInteractable
{
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
        }
    }
}

