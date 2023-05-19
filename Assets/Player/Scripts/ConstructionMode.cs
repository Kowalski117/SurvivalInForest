using UnityEngine;

public class ConstructionMode : MonoBehaviour
{
    [SerializeField] private Transform _container;
    [SerializeField] private Construction _construction;
    [SerializeField] private Camera _camera;
    private Construction _flyingConstruction;

    private void Start()
    {
        CreateBuilding(_construction, 5f);
    }

    private void Update()
    {
        if (_flyingConstruction != null)
        {
            Vector3 cameraPosition = _camera.transform.position;
            Vector3 newPosition = Vector3.ProjectOnPlane(cameraPosition, Vector3.forward);
            newPosition.z = _flyingConstruction.transform.position.z;
            _flyingConstruction.transform.position = newPosition;
        }
    }

    public Construction CreateBuilding(Construction buildingPrefab, float distanceFromCamera)
    {
        if (_flyingConstruction != null)
        {
            Destroy(_flyingConstruction.gameObject);
        }

        Vector3 spawnPosition = _camera.transform.TransformPoint(Vector3.forward * distanceFromCamera);
        _flyingConstruction = Instantiate(buildingPrefab, spawnPosition, Quaternion.identity, _container);
        return _flyingConstruction;
    }
}
