using System.Collections.Generic;
using UnityEngine;

public class SaveBuildingHandler : SaveHandler
{
    [SerializeField] private List<BuildingRecipe> _buildingDataList;

    private string _idsBuildings = "idsBuildings";

    private Building _currentBuilding;

    protected override void SaveBase()
    {
        ES3.Save(_idsBuildings, Ids);
    }

    protected override void LoadBase()
    {
        if (ES3.KeyExists(_idsBuildings)) 
        {
            Ids = ES3.Load<List<string>>(_idsBuildings);

            foreach (var buildingData in Ids)
            {
                if (ES3.KeyExists(buildingData))
                {
                    BuildingSaveData itemSaveData = ES3.Load<BuildingSaveData>(buildingData);

                    foreach (var data in _buildingDataList)
                    {
                        if (data.BuildingData.Id == itemSaveData.Id)
                        {
                            _currentBuilding = Instantiate(data.BuildingData.Prefab, itemSaveData.Position, itemSaveData.Rotation, Container);
                            _currentBuilding.Init(data, buildingData);
                            _currentBuilding.PlaceBuilding();
                            _currentBuilding = null;
                            break;
                        }
                    }
                }
            }
        }
    }
}
