using IL3DN;
using UnityEngine;

public class TouchColliderBuildingHandler : MonoBehaviour
{
    private Building _parentBuilding;

    private void Awake()
    {
        _parentBuilding = GetComponentInParent<Building>();
    }

    private void OnTriggerStay(Collider other)
    {
        //if(other.GetComponent<TouchColliderBuildingHandler>() == null && !other.GetComponent<SpawnPointAnimals>() && !other.GetComponent<PointOffComponentsAnimal>() && !other.GetComponent<IL3DN_ChangeWalkingSound>() && !other.GetComponent<FoundationConnection>())
        //{
        //    _parentBuilding.SetOverlapping(true);
        //}
    }
}
