using System.IO;
using UnityEngine;
using UnityEngine.Events;

namespace SaveLoadSystem
{
    public static class SaveGameHandler
    {
        public static SaveData CurrentSaveData = new SaveData();

        private const string SaveDirectory = "/SaveData/";
        private const string FileName = "SaveGame.sav";

        public static UnityAction OnLoadGameStart;
        public static UnityAction OnLoadGameFinish;

        public static bool SaveGame()
        {
            string dir = Application.persistentDataPath + SaveDirectory;

            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            string json = JsonUtility.ToJson(CurrentSaveData, true);
            File.WriteAllText(dir + FileName, json);

            GUIUtility.systemCopyBuffer = dir;

            return true;
        }

        public static void LoadGame()
        {
            OnLoadGameStart?.Invoke();
            string fullPath = Application.persistentDataPath + SaveDirectory + FileName;
            SaveData tempData = new SaveData();

            if(File.Exists(fullPath))
            {
                string json = File.ReadAllText(fullPath);
                tempData = JsonUtility.FromJson<SaveData>(json);
            }
            else
            {
                Debug.LogWarning("Save file does not exist!");
            }

            CurrentSaveData = tempData;

            OnLoadGameFinish?.Invoke();
        }
    }
}
