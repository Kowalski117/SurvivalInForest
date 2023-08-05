using System.Collections.Generic;
using UnityEngine;

public class BaseHandler : MonoBehaviour
{
    [SerializeField] private List<BuildingSaveData> _buildingDataList;
    [SerializeField] private Transform _buildingContainer; 

    private List<string> _ids = new List<string>();

    private BuildingData _currentBuildingData;
    private Building _currentBuilding;

    private void OnEnable()
    {
        SaveGame.OnLoadData += LoadBase;
    }

    private void OnDisable()
    {
        SaveGame.OnLoadData -= LoadBase;
    }

    public void AddId(string id)
    {
        _ids.Add(id);
    }

    private void LoadBase()
    {
        //    foreach (var buildingData in _ids)
        //    {
        //        if (ES3.KeyExists(buildingData))
        //        {
        //            BuildingSaveData itemSaveData = ES3.Load(buildingData, new BuildingSaveData(_currentBuildingData, _currentBuilding.transform.position, _currentBuilding.transform.rotation));
        //            _currentBuilding = Instantiate(itemSaveData.AssignedData.Prefab, itemSaveData.Position, itemSaveData.Rotation, _buildingContainer);\
        //            _currentBuilding = null;
        //        }
        //    }
    }
}
