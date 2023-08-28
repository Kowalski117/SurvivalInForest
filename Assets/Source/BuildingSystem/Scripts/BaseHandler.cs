using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class BaseHandler : MonoBehaviour
{
    [SerializeField] private List<BuildingSaveData> _buildingDataList;
    [SerializeField] private Transform _buildingContainer; 

    private List<string> _ids = new List<string>();
    private string _idsBuildings = "idsBuildings";

    private BuildingData _currentBuildingData;
    private Building _currentBuilding;

    private void OnEnable()
    {
        SaveGame.OnLoadData += LoadBase;
        SaveGame.OnSaveGame += SaveBase;
    }

    private void OnDisable()
    {
        SaveGame.OnLoadData -= LoadBase;
        SaveGame.OnSaveGame -= SaveBase;
    }

    public void AddId(string id)
    {
        _ids.Add(id);
    }

    private void SaveBase()
    {
        ES3.Save(_idsBuildings, _ids);
    }

    private void LoadBase()
    {
        _ids = ES3.Load<List<string>>(_idsBuildings);

        foreach (var buildingData in _ids)
        {
            if (ES3.KeyExists(buildingData))
            {
                Debug.Log(buildingData);
                //BuildingSaveData itemSaveData = ES3.Load<BuildingSaveData>(buildingData);
                Debug.Log(ES3.Load<BuildingSaveData>(buildingData).BuildingPrefab);
                //Debug.Log(ES3.Load<Vector3>(buildingData));
                //Debug.Log(ES3.Load<Quaternion>(buildingData));
                //_currentBuilding = Instantiate(, ,, _buildingContainer);
                    _currentBuilding = null;
            }
        }
    }
}
