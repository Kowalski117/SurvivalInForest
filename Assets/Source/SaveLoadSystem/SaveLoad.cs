using System.IO;
using UnityEngine;
using UnityEngine.Events;

public static class SaveLoad
{
    private static string _directory = "/SaveData/";
    private static string _fileName = "SaveGame.sav";

    public static UnityAction OnSaveGame;
    public static UnityAction<SaveData> OnLoadData;

    public static void Save(SaveData data)
    {
        OnSaveGame?.Invoke();

        string directory = Application.persistentDataPath + _directory;

        GUIUtility.systemCopyBuffer = directory;

        if (!Directory.Exists(directory))
            Directory.CreateDirectory(directory);

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(directory + _fileName, json);

        GUIUtility.systemCopyBuffer = directory;

        Debug.Log("Saving game");
    }

    public static SaveData Load()
    {
        string fullPath = Application.persistentDataPath + _directory + _fileName;
        SaveData data = new SaveData();

        if (File.Exists(fullPath))
        {
            string json = File.ReadAllText(fullPath);
            data = JsonUtility.FromJson<SaveData>(json);

            OnLoadData?.Invoke(data);
        }
        else
        {
            Debug.LogWarning("Save file does not exist!");
        }
        return data;
    }

    public static void DeleteSaveData()
    {
        string fullPath = Application.persistentDataPath + _directory + _fileName;

        if (File.Exists(fullPath))
            File.Delete(fullPath);
    }
}
