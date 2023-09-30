using UnityEngine;
using UnityEngine.Events;

public class ColliderHandler : MonoBehaviour
{
    [SerializeField] private bool _isEnabledInitially = false;

    private Building _building;
    private BoxCollider _boxCollider;
    private DistanceHandler _distanceHandler;

    public static UnityAction OnInteractionSleepingPlace;

    private void Awake()
    {
        _building = GetComponentInParent<Building>();
        _boxCollider = GetComponent<BoxCollider>();
        _boxCollider.enabled = false;
        _distanceHandler = GetComponentInChildren<DistanceHandler>();
    }

    private void Start()
    {
        if (_distanceHandler != null)
            _distanceHandler.ToggleCollider(false);

        if (_isEnabledInitially)
        {
            _boxCollider.enabled = true;

            if (_distanceHandler != null)
                _distanceHandler.ToggleCollider(true);
        }
    }

    private void OnEnable()
    {
        _building.OnCompletedBuild += Enable;
    }

    private void OnDisable()
    {
        _building.OnCompletedBuild -= Enable;
    }

    private void Enable()
    {
        _boxCollider.enabled = true;

        if (_distanceHandler != null)
            _distanceHandler.ToggleCollider(true);
    }
}
