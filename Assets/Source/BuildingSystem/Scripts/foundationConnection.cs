using UnityEngine;

public class FoundationConnection : MonoBehaviour
{
    [SerializeField] private Transform _pointPlaceFloor;

    private Collider _collider;

    public Transform PointPlaceFloor => _pointPlaceFloor;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
    }

    public void TurnOffCollider()
    {
        _collider.enabled = false;
    }
}
