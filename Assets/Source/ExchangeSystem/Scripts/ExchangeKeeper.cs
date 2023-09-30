using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(UniqueID))]
public class ExchangeKeeper : MonoBehaviour, IInteractable
{
    private DistanceHandler _distanceHandler;

    public static UnityAction<ExchangeKeeper> OnExchangeDisplayRequested;
    public UnityAction<IInteractable> OnInteractionComplete { get; set; }

    public DistanceHandler DistanceHandler => _distanceHandler;

    private void Awake()
    {
        _distanceHandler = GetComponentInChildren<DistanceHandler>();
    }

    private void OnEnable()
    {
        _distanceHandler.OnDistanceExceeded += Interact;
    }

    private void OnDisable()
    {
        _distanceHandler.OnDistanceExceeded -= Interact;
    }

    public void EndInteraction() { }

    public void Interact()
    {
        OnExchangeDisplayRequested?.Invoke(this);
    }
}

