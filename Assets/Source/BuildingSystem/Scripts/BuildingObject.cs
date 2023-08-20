using UnityEngine;
using UnityEngine.Events;

public class BuildingObject : MonoBehaviour
{
    private Building _building;
    private BoxCollider _boxCollider;

    public static UnityAction OnInteractionSleepingPlace;

    private void Awake()
    {
        _building = GetComponentInParent<Building>();
        _boxCollider = GetComponent<BoxCollider>();
        _boxCollider.enabled = false;
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
