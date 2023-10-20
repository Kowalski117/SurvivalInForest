using UnityEngine;
using UnityEngine.Events;

public class SaveGame : MonoBehaviour
{
    [SerializeField] private LoadPanel _loadPanel;
    [SerializeField] private bool _isAutoSave;
    [SerializeField] private float _autoSaveDelay;

    float _timer = 0;

    public static UnityAction OnSaveGame;
    public static UnityAction OnLoadData;

    //private void Start()
    //{
    //    Load();
    //}

    private void Update()
    {
        if(_isAutoSave)
        {
            _timer += Time.deltaTime;

            if (_timer >= _autoSaveDelay)
            {
                _timer = 0;
                Save();
            }
        }
    }

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
