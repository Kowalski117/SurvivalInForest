using Agava.YandexGames;
using BehaviorDesigner.Runtime.Tasks.Unity.UnityRenderer;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;

public class YandexAds : MonoBehaviour
{
    [SerializeField] private AudioHandler _audioHandler;
    [SerializeField] private PlayerHandler _playerInputHandler;

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

        _isCursorEnable = _playerInputHandler.IsCursorEnable;
        Debug.Log(_isCursorEnable + "0");
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
