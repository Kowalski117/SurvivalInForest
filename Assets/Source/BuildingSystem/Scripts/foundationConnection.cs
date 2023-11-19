using UnityEngine;

public class FoundationConnection : MonoBehaviour
{
    [SerializeField] private Transform _pointPlaceFloor;
    
    private BuildType _buildType = BuildType.Floor;
    private Collider _collider;

    public Transform PointPlaceFloor => _pointPlaceFloor;
    public BuildType BuildType => _buildType;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
    }

    public void TurnOffCollider()
    {
        _collider.enabled = false;
    }

    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.gameObject.layer != 7 || !other.GetComponent<Building>())
    //        TurnOffCollider();
    //}
}
