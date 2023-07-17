using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class BaseHandler : MonoBehaviour
{
    //[SerializeField] private List<BuildingSaveData> _buildingDataList;

    //private List<string> _ids = new List<string>();

    //private void OnEnable()
    //{
    //    SaveGame.OnLoadData += LoadBase;
    //}

    //private void OnDisable()
    //{
    //    SaveGame.OnLoadData -= LoadBase;
    //}

    //public static void AddId(string id)
    //{
    //    _ids.Add(id);
    //}

    //private void LoadBase()
    //{
    //    foreach (var buildingData in _ids)
    //    {
    //        if (ES3.KeyExists(buildingData))
    //        {
    //            Building building = Instantiate();

    //            BuildingSaveData itemSaveData = ES3.Load(_buildingId + _uniqueID.Id, new BuildingSaveData(_assignedData, transform.position, transform.rotation));
    //            _assignedData = itemSaveData.AssignedData;
    //            transform.position = itemSaveData.Position;
    //            transform.rotation = itemSaveData.Rotation;
    //        }
    //    }
    //}
}
