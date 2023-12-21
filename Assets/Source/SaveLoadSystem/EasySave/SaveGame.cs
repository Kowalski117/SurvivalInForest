using Agava.YandexGames;
using BehaviorDesigner.Runtime.Tasks.Unity.UnityPlayerPrefs;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class SaveGame : MonoBehaviour
{
    [SerializeField] private YandexAds _yandexAds;
    [SerializeField] private bool _isAutoSave;
    [SerializeField] private float _autoSaveDelay;
    [SerializeField] private int _notificationTime = 10;

    private float _timer = 0;
    private float _delay = 0.2f;

    public event Action<int> OnNotifyPlayer;
    public event Action OnCloseNotifierPlayer;
    public static event Action OnSaveGame;
    public static event Action OnLoadData;

    private void Start()
    {
        StartCoroutine(StartLoad());
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
        SetCloudSaveData();
    }

    public static void Load()
    {
        OnLoadData?.Invoke();
        ES3.Save(SaveLoadConstants.TransitionScene, false);
    }

    public void Delete()
    {
        ES3.DeleteFile();
        PlayerPrefs.DeleteAll();
    }

    public void GetCloudSaveData()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        if (PlayerAccount.IsAuthorized)
            PlayerAccount.GetCloudSaveData(OnSuccessLoad, OnErrorLoad);
#endif
    }

    public static void SetCloudSaveData()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        if (!PlayerAccount.IsAuthorized)
            return;
        string str = ES3.LoadRawString("SaveFile.es3");
        PlayerAccount.SetCloudSaveData(str);
#endif
    }

    private IEnumerator WaitForLoad(float delay)
    {
        yield return new WaitForSeconds(delay);
        Load();
    }

    private void OnSuccessLoad(string json)
    {
        if (!string.IsNullOrWhiteSpace(json))
        {
            if (json != ES3.LoadRawString("SaveFile.es3"))
            {
                ES3.DeleteFile();
                ES3.AppendRaw(json, "SaveFile.es3");
                Load();
            }
        }
    }

    private void OnErrorLoad(string message)
    {
        Debug.Log("ύμμμ");
    }

    private IEnumerator StartLoad()
    {
        yield return new WaitForSeconds(_delay);

        if (ES3.KeyExists(SaveLoadConstants.IsNewGame) && ES3.Load<bool>(SaveLoadConstants.IsNewGame) == true)
        {
            Delete();
            ES3.Save(SaveLoadConstants.IsCutscenePlayed, true);
            StartCoroutine(WaitForLoad(0.5f));
        }

        if (ES3.KeyExists(SaveLoadConstants.StartLastSaveScene) && ES3.Load<bool>(SaveLoadConstants.StartLastSaveScene) == true || ES3.KeyExists(SaveLoadConstants.TransitionScene) && ES3.Load<bool>(SaveLoadConstants.TransitionScene) == true)
            StartCoroutine(WaitForLoad(0.5f));
    }
}