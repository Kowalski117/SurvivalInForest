using System.Collections.Generic;
using UnityEngine;

public class SaveBuildingHandler : SaveHandler
{
    [SerializeField] private List<BuildingRecipe> _buildingDataList;

    private Building _currentBuilding;

    protected override void SaveBase()
    {
        ES3.Save(SaveLoadConstants.IdsBuildings, Ids);
    }

    protected override void LoadBase()
    {
        if (ES3.KeyExists(SaveLoadConstants.IdsBuildings)) 
        {
            Ids = ES3.Load<List<string>>(SaveLoadConstants.IdsBuildings);

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
                            _currentBuilding.Place();
                            _currentBuilding = null;
                            break;
                        }
                    }
                }
            }
        }
    }
}
