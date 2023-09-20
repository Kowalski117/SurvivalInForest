using UnityEngine;
using UnityEngine.Events;

public class BuildingObject : MonoBehaviour
{
    [SerializeField] private bool _isEnabledInitially = false;

    private Building _building;
    private BoxCollider _boxCollider;

    public static UnityAction OnInteractionSleepingPlace;

    private void Awake()
    {
        _building = GetComponentInParent<Building>();
        _boxCollider = GetComponent<BoxCollider>();
        _boxCollider.enabled = false;
    }

    private void Start()
    {
        if (_isEnabledInitially)
        {
            _boxCollider.enabled = true;
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
    }
}
