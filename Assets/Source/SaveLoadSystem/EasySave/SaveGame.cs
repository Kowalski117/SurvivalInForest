using UnityEngine;
using UnityEngine.Events;

public class SaveGame : MonoBehaviour
{
    [SerializeField] private LoadPanel _loadPanel;

    public static UnityAction OnSaveGame;
    public static UnityAction OnLoadData;

    //private void Start()
    //{
    //    Load();
    //}

    public void Save()
    {
        //_loadPanel.gameObject.SetActive(true);
        OnSaveGame?.Invoke();
        //_loadPanel.Load(1, _loadPanel.Deactivate);
    }

    public void Load()
    {
        //_loadPanel.gameObject.SetActive(true);
        OnLoadData?.Invoke();
        //_loadPanel.Load(1, _loadPanel.Deactivate);
    }

    public void Delete()
    {
        ES3.DeleteFile();
    }
}
