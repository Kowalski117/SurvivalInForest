using Agava.YandexGames;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;

public class YandexAds : MonoBehaviour
{
    //[SerializeField] private SoundSettings _soundSettings;
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
        VideoAd.Show(() => OnAdOpen(), ReceiveAward, () => OnAdOpen(), null);
#endif
    }

    public void ReceiveAward()
    {
        OnReceivedAward?.Invoke();
    }

    public void OnAdOpen()
    {
        Time.timeScale = 0;

        if (_playerInputHandler)
            _playerInputHandler.ToggleAllParametrs(false);
        //_soundSettings.Mute();
    }

    public void OnIterstitialAddClose(bool value)
    {
        Time.timeScale = 1;

        if(_playerInputHandler)
            _playerInputHandler.ToggleAllParametrs(true);
        //_soundSettings.Load();
    }
}
