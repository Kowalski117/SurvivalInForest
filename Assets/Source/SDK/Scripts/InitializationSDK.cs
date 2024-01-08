using System.Collections;
using Agava.YandexGames;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InitializationSDK : MonoBehaviour
{
    [SerializeField] private LocalizationHandler _localization;
    [SerializeField] private YandexAds _yandexAds;
    [SerializeField] private SaveGame _saveGame;

#if YANDEX_GAMES && UNITY_WEBGL && !UNITY_EDITOR
    private void Awake()
    {
        YandexGamesSdk.CallbackLogging = true;
    }

    private IEnumerator Start()
    {
        yield return YandexGamesSdk.Initialize(OnInitialized);
    }
#endif

#if YANDEX_GAMES && UNITY_WEBGL && !UNITY_EDITOR
    private void OnInitialized()
    {
        if (PlayerPrefs.HasKey(ConstantsSDK.Language))
            _localization.SetLanguageString(PlayerPrefs.GetString(ConstantsSDK.Language));
        else
            _localization.SetLanguageString(YandexGamesSdk.Environment.i18n.lang);

        //if (_yandexAds != null)
        //    _yandexAds.ShowInterstitial();

        if (PlayerAccount.IsAuthorized)
            _saveGame.GetCloudSaveData();

        if (ES3.KeyExists(SaveLoadConstants.IsCutscenePlayed) && ES3.Load<bool>(SaveLoadConstants.IsCutscenePlayed) == true)
        {
            Debug.Log("11");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else
        {
            Debug.Log("22");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
        }
    }
#endif
}
