using UnityEngine;

public class BaseHandler : MonoBehaviour
{
    [SerializeField] private LayerMask _defoultLayerInt;

    private void OnEnable()
    {
        SaveLoad.OnLoadData += LoadBase;
    }

    private void OnDisable()
    {
        SaveLoad.OnLoadData -= LoadBase;
    }

    private void LoadBase(SaveData data)
    {
        foreach (var building in data.BuildingSaveData)
        {
            GameObject gameObject = new GameObject
            {
                layer = _defoultLayerInt,
                name = building.BuildingName
            };

            Building _spawnedBuilding = gameObject.AddComponent<Building>();
            _spawnedBuilding.Init(building.AssignedData, building);
            _spawnedBuilding.transform.position = building.Position;
            _spawnedBuilding.transform.rotation = building.Rotation;

            _spawnedBuilding.PlaceBuilding();
        }
    }
}
