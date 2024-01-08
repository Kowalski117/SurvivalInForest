using Agava.YandexGames;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;

public class YandexAds : MonoBehaviour
{
    [SerializeField] private AudioHandler _audioHandler;
    [SerializeField] private PlayerInputHandler _playerInputHandler;

    private bool _isCursorEnable = false;

    public void ShowInterstitial()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        InterstitialAd.Show(OnAdOpen, OnIterstitialAddClose);
#endif
    }

    public void ShowRewardAd(UnityAction OnReceiveAward)
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        VideoAd.Show(() => OnAdOpen(), () => OnReceiveAward?.Invoke(), () => OnAdClose(), null);
#endif
    }

    public void OnAdOpen()
    {
        _audioHandler.FadeIn();

        _isCursorEnable = !_playerInputHandler.IsCursorEnable;

        if (_playerInputHandler)
            _playerInputHandler.ToggleAllParametrs(false);
        Time.timeScale = 0;
    }

    public void OnAdClose()
    {
        Time.timeScale = 1;
        _audioHandler.FadeOut();
        if (_playerInputHandler)
        {
            _playerInputHandler.ToggleAllParametrs(true);

            if (!_isCursorEnable)
                _playerInputHandler.SetCursorVisible(false);
            else
                _playerInputHandler.SetCursorVisible(true);
        }
    }

    public void OnIterstitialAddClose(bool value)
    {
        OnAdClose();
    }
}
