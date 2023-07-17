using UnityEngine;
using UnityEngine.Events;

public class SaveGame : MonoBehaviour
{
    public static UnityAction OnSaveGame;
    public static UnityAction OnLoadData;

    //private void Start()
    //{
    //    Load();
    //}

    public void Save()
    {
        OnSaveGame?.Invoke();
    }

    public void Load()
    {
        OnLoadData?.Invoke();
    }

    public void Delete()
    {
        ES3.DeleteFile();
    }
}
