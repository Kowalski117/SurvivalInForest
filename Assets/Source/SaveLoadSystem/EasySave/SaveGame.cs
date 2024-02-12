using Agava.YandexGames;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class SaveGame : MonoBehaviour
{
    [SerializeField] private YandexAds _yandexAds;
    [SerializeField] private bool _isAutoSave;
    [SerializeField] private float _autoSaveDelay;
    [SerializeField] private int _notificationTime = 10;
    [SerializeField] private bool _isSurvivalScene = true;

    private float _timer = 0;
    private float _delay = 0.2f;
    private Coroutine _saveCoroutine;

    public event UnityAction<int> OnNotifyPlayer;
    public event UnityAction OnCloseNotifierPlayer;
    public event UnityAction OnAutoSaved;

    public static event UnityAction OnSaveGame;
    public static event UnityAction OnLoadData;

    private void Start()
    {
        if(_isSurvivalScene)
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
                OnAutoSaved?.Invoke();
            }

            if (_timer >= _autoSaveDelay - _notificationTime)
            {
                OnNotifyPlayer?.Invoke(_notificationTime);
            }
        }
    }

    public void Save()
    {
        OnSaveGame?.Invoke();

        if (_saveCoroutine != null)
        {
            StopCoroutine(_saveCoroutine);
            _saveCoroutine = null;
        }

        _saveCoroutine = StartCoroutine(SetCloudSaveData());
    }

    public static void Load()
    {
        OnLoadData?.Invoke();
        ES3.Save(SaveLoadConstants.TransitionScene, false);
    }

    public static void Delete()
    {
        ES3.DeleteFile();
        PlayerPrefs.DeleteAll();
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
        Debug.Log("1");
        if (!string.IsNullOrWhiteSpace(json))
        {
            Debug.Log("2");
            if (json != ES3.LoadRawString("SaveFile.es3"))
            {
                Debug.Log("3");
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