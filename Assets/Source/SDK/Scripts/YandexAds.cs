using Agava.YandexGames;
//using CrazyGames;
using UnityEngine;
using UnityEngine.Events;

public class YandexAds : MonoBehaviour
{
    [SerializeField] private AudioHandler _audioHandler;
    [SerializeField] private PlayerHandler _playerInputHandler;

    public void ShowInterstitial()
    {
#if YANDEX_GAMES && UNITY_WEBGL && !UNITY_EDITOR
        InterstitialAd.Show(OnAdOpen, OnIterstitialAddClose);
#endif
//#if CRAZY_GAMES && UNITY_WEBGL && !UNITY_EDITOR
//        CrazyAds.Instance.beginAdBreak();
//#endif
    }

    public void ShowRewardAd(UnityAction OnReceiveAward)
    {
#if YANDEX_GAMES && UNITY_WEBGL && !UNITY_EDITOR
        VideoAd.Show(() => OnAdOpen(), () => OnReceiveAward?.Invoke(), () => OnAdClose(), null);
#endif
//#if CRAZY_GAMES && UNITY_WEBGL && !UNITY_EDITOR
//        CrazyAds.Instance.beginAdBreakRewarded(() => OnReceiveAward?.Invoke());
//#endif
    }

    public void OnAdOpen()
    {
        _audioHandler.FadeIn();

        if (_playerInputHandler)
        {
            _playerInputHandler.ToggleAllParametrs(false);
            _playerInputHandler.TogglePersonController(false);
            _playerInputHandler.SetCursorVisible(true);
        }
        Time.timeScale = 0;
    }

    public void OnAdClose()
    {
        Time.timeScale = 1;
        _audioHandler.FadeOut();
        if (_playerInputHandler)
        {
            _playerInputHandler.ToggleAllParametrs(true);
            _playerInputHandler.TogglePersonController(true);
            _playerInputHandler.SetCursorVisible(false);
        }
    }

    public void OnIterstitialAddClose(bool value)
    {
        OnAdClose();
    }
}
