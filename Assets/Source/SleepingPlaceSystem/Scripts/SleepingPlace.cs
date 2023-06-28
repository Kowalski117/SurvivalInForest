using UnityEngine;
using UnityEngine.Events;

public class SleepingPlace : MonoBehaviour
{
    public static UnityAction OnInteractionSleepingPlace;

    public void Interact(Interactor interactor, out bool interactSuccessfull)
    {
        OnInteractionSleepingPlace?.Invoke();
        interactSuccessfull = true;
    }

    public void EndInteraction()
    {

    }
}
