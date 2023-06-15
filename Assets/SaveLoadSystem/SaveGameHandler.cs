using System.IO;
using UnityEngine;
using UnityEngine.Events;

public class SaveGameHandler : MonoBehaviour
{
    private static SaveData _data;

    public static UnityAction OnLoadGameStart;
    public static UnityAction OnLoadGameFinish;

    public static SaveData Data => _data;

    private void Awake()
    {
        _data = new SaveData();
        SaveLoad.OnLoadData += LoadData;
    }

    public static void SaveData()
    {
        var saveData = _data;

        SaveLoad.Save(saveData);
    }

    public static void LoadData(SaveData data)
    {
        _data = data;
    }

    public void DeleteData()
    {
        SaveLoad.DeleteSaveData();
    }

    public static void TryLoadData()
    {
        SaveLoad.Load();
    }

    private void OnDestroy()
    {
        SaveLoad.OnLoadData -= LoadData;
    }
}
