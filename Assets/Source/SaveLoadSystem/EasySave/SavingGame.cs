using Agava.YandexGames;
using System;
using System.Collections;
using UnityEngine;

public class SavingGame : MonoBehaviour
{
    [SerializeField] private YandexAds _yandexAds;
    [SerializeField] private bool _isAutoSave;
    [SerializeField] private float _autoSaveDelay;
    [SerializeField] private int _notificationTime = 10;
    [SerializeField] private bool _isSurvivalScene = true;

    private float _timer = 0;
    private float _delay = 0.2f;
    private Coroutine _saveCoroutine;

    public event Action<int> OnNotifyedPlayer;
    public event Action OnClosedNotifierPlayer;
    public event Action OnAutoSaved;

    public static event Action OnGameSaved;
    public static event Action OnGameLoaded;
    public static event Action OnSaveDeleted;

    private void Start()
    {
        if (_isSurvivalScene)
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
                OnClosedNotifierPlayer?.Invoke();
                _yandexAds.ShowInterstitial();
                Save();
                OnAutoSaved?.Invoke();
            }

            if (_timer >= _autoSaveDelay - _notificationTime)
            {
                OnNotifyedPlayer?.Invoke(_notificationTime);
            }
        }
    }

    public void Save()
    {
        OnGameSaved?.Invoke();

        if (_saveCoroutine != null)
        {
            StopCoroutine(_saveCoroutine);
            _saveCoroutine = null;
        }

        _saveCoroutine = StartCoroutine(SetCloudSaveData());
    }

    public static void Load()
    {
        OnGameLoaded?.Invoke();
    }

    public static void Delete()
    {
        OnSaveDeleted?.Invoke();
    }

    public void GetCloudSaveData()
    {
#if YANDEX_GAMES && UNITY_WEBGL && !UNITY_EDITOR
        //if (PlayerAccount.IsAuthorized)
        //    PlayerAccount.GetCloudSaveData(OnSuccessLoad, OnErrorLoad);
#endif
    }

    public IEnumerator SetCloudSaveData()
    {
        yield return new WaitForSeconds(10f);

#if YANDEX_GAMES && UNITY_WEBGL && !UNITY_EDITOR
        if (!PlayerAccount.IsAuthorized)
            yield return null;

        string str = ES3.LoadRawString("SaveFile.es3");
        Debug.Log(str);
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

    private IEnumerator StartLoad()
    {
        yield return new WaitForSeconds(_delay);

        if (ES3.KeyExists(SaveLoadConstants.IsNewGame) && ES3.Load<bool>(SaveLoadConstants.IsNewGame) == true)
            Delete();

        StartCoroutine(WaitForLoad(_delay));
    }
}