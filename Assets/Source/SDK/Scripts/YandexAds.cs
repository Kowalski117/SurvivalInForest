using Agava.YandexGames;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;

public class YandexAds : MonoBehaviour
{
    [SerializeField] private AudioHandler _audioHandler;
    [SerializeField] private PlayerInputHandler _playerInputHandler;

    public event UnityAction OnReceivedAward;

    public void ShowInterstitial()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        InterstitialAd.Show(OnAdOpen, OnIterstitialAddClose);
#endif
    }

    public void ShowRewardAd()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        VideoAd.Show(() => OnAdOpen(), ReceiveAward, () => OnAdClose(), null);
#endif
    }

    public void ReceiveAward()
    {
        OnReceivedAward?.Invoke();
    }

    public void OnAdOpen()
    {
        Time.timeScale = 0;
        _audioHandler.FadeIn();
        if (_playerInputHandler)
            _playerInputHandler.ToggleAllParametrs(false);
    }

    public void OnAdClose()
    {
        Time.timeScale = 1;
        _audioHandler.FadeOut();
        if (_playerInputHandler)
            _playerInputHandler.ToggleAllParametrs(true);
    }

    public void OnIterstitialAddClose(bool value)
    {
        OnAdClose();
    }
}
