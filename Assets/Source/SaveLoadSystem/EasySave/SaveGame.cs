using System;
using System.Collections;
using UnityEngine;

public class SaveGame : MonoBehaviour
{
    [SerializeField] private LoadPanel _loadPanel;
    [SerializeField] private YandexAds _yandexAds;
    [SerializeField] private bool _isAutoSave;
    [SerializeField] private float _autoSaveDelay;
    [SerializeField] private int _notificationTime = 10;

    private float _timer = 0;

    public Action<int> OnNotifyPlayer;
    public Action OnCloseNotifierPlayer;
    public static Action OnSaveGame;
    public static Action OnLoadData;

    private void Start()
    {
        if(ES3.KeyExists(SaveLoadConstants.IsNewGame) && ES3.Load<bool>(SaveLoadConstants.IsNewGame) == true)
        {
            Delete();
            StartCoroutine(WaitForLoad(0.5f));
        }

        if (ES3.KeyExists(SaveLoadConstants.StartLastSaveScene) && ES3.Load<bool>(SaveLoadConstants.StartLastSaveScene) == true || ES3.KeyExists(SaveLoadConstants.TransitionScene) && ES3.Load<bool>(SaveLoadConstants.TransitionScene) == true)
            StartCoroutine(WaitForLoad(0.5f));
    }

    private void Update()
    {
        if (_isAutoSave)
        {
            _timer += Time.deltaTime;

            if (_timer >= _autoSaveDelay)
            {
                _timer = 0;
                OnCloseNotifierPlayer?.Invoke();
                _yandexAds.ShowInterstitial();
                Save();
            }

            if(_timer >= _autoSaveDelay - _notificationTime)
            {
                OnNotifyPlayer?.Invoke(_notificationTime);
            }
        }
    }

    public void Save()
    {
        OnSaveGame?.Invoke();
    }

    public void Load()
    {
        OnLoadData?.Invoke();
        ES3.Save(SaveLoadConstants.TransitionScene, false);
    }

    public void Delete()
    {
        ES3.DeleteFile();
        PlayerPrefs.DeleteAll();
    }

    private IEnumerator WaitForLoad(float delay)
    {
        yield return new WaitForSeconds(delay);
        Load();
    }
}