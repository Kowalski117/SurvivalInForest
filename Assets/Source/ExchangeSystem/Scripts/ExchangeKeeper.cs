using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(UniqueID))]
public class ExchangeKeeper : MonoBehaviour, IInteractable
{
    private bool _isActive = false;

    public static UnityAction<ExchangeKeeper> OnExchangeDisplayRequested;
    public UnityAction<IInteractable> OnInteractionComplete { get; set; }

    public void EndInteraction() { }

    public void Interact()
    {
        OnExchangeDisplayRequested?.Invoke(this);
        _isActive = !_isActive;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out PlayerHealth playerHealth))
        {
            if (_isActive)
            {
                OnExchangeDisplayRequested?.Invoke(this);
                _isActive = false;
            }
        }
    }
}

