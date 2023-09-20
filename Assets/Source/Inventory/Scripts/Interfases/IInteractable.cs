using UnityEngine.Events;

public interface IInteractable
{
    public UnityAction<IInteractable> OnInteractionComplete { get; set; }

    public void Interact();
    public void EndInteraction();
}
