using System;
using System.Collections;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;

public class AudioHandler : MonoBehaviour
{
    [SerializeField] private AudioMixer _mixer;
    [SerializeField] private SettingScreen _settingScreen;

    private const string _masterStr = "Master";
    private const string _musicStr = "Music";
    private const string _effectsStr = "Effects";
    private const string _boolStr = "Bool";
    private const float _zeroVolume = -80f;
    private const float _fadeSpeed = 60f;
    private const float _defoultValue = 0.4f;

    private Coroutine _musicCoroutine;
    private Coroutine _effectsCoroutine;
    private float _effectsValue;
    private float _musicValue;

    private bool _isMuteAllSound = false;
    private bool _isMuteMusic = false;
    private bool _isMuteEffects = false;

    private float _currentValueMaster;
    private float _currentValueMusic;
    private float _currentValueEffects;

    public bool IsMuteAllSound => _isMuteAllSound;
    public bool IsMuteMusic => _isMuteMusic;
    public bool IsMuteEffects => _isMuteEffects;
    public float CurrentVolumeMaster => _currentValueMaster;
    public float CurrentVolumeMusic => _currentValueMusic;
    public float CurrentVolumeEffects => _currentValueEffects;

    private void OnEnable()
    {
        if(_settingScreen)
            _settingScreen.OnCloseScreen += Save;
    }

    private void OnDisable()
    {
        if (_settingScreen)
            _settingScreen.OnCloseScreen -= Save;
    }

    private void Awake()
    {
        Load();
    }

    public void Load()
    {
        _currentValueMaster = PlayerPrefs.HasKey(_masterStr) ? PlayerPrefs.GetFloat(_masterStr) : 0;
        _currentValueMusic = PlayerPrefs.HasKey(_musicStr) ? PlayerPrefs.GetFloat(_musicStr) : _defoultValue;
        _currentValueEffects = PlayerPrefs.HasKey(_effectsStr) ? PlayerPrefs.GetFloat(_effectsStr) : _defoultValue;
        _isMuteAllSound = (PlayerPrefs.GetFloat(_masterStr + _boolStr) == 1) ? true : false;
        _isMuteMusic = (PlayerPrefs.GetFloat(_musicStr + _boolStr) == 1) ? true : false;
        _isMuteEffects = (PlayerPrefs.GetFloat(_effectsStr + _boolStr) == 1) ? true : false;
    }

    public void Save()
    {
        PlayerPrefs.Save();
    }

    public void OnOffAllSound()
    {
        _isMuteAllSound = !_isMuteAllSound;
        _mixer.SetFloat(_masterStr, _isMuteAllSound ? _zeroVolume : GetValue(_currentValueMaster));
        PlayerPrefs.SetFloat(_masterStr + _boolStr, _isMuteAllSound ? 1 : 0);
    }

    public void OnOffMusic()
    {
        _isMuteMusic = !_isMuteMusic;
        _mixer.SetFloat(_musicStr, _isMuteMusic ? _zeroVolume : GetValue(_currentValueMusic));
        PlayerPrefs.SetFloat(_musicStr + _boolStr, _isMuteMusic ? 1 : 0);
    }

    public void OnOffEffects()
    {
        _isMuteEffects = !_isMuteEffects;
        _mixer.SetFloat(_effectsStr, _isMuteEffects ? _zeroVolume : GetValue(_currentValueEffects));
        PlayerPrefs.SetFloat(_effectsStr + _boolStr, _isMuteEffects ? 1 : 0);
    }

    public void SetAllSound(float value)
    {
        _currentValueMaster =  value;

        if(!_isMuteAllSound) 
            _mixer.SetFloat(_masterStr, GetValue(value));
        else
            _mixer.SetFloat(_masterStr,  _zeroVolume);

        PlayerPrefs.SetFloat(_masterStr, _currentValueMaster);
    }

    public void SetMusic(float value)
    {
        _currentValueMusic =  value;

        if (!_isMuteMusic)
            _mixer.SetFloat(_musicStr, GetValue(value));
        else
            _mixer.SetFloat(_musicStr, _zeroVolume);

        PlayerPrefs.SetFloat(_musicStr, _currentValueMusic);
    }

    public void SetEffects(float value)
    {
        _currentValueEffects = value;

        if (!_isMuteEffects)
            _mixer.SetFloat(_effectsStr, GetValue(value));
        else
            _mixer.SetFloat(_effectsStr, _zeroVolume);

        PlayerPrefs.SetFloat(_effectsStr, _currentValueEffects);
    }

    public void Mute()
    {
        _mixer.SetFloat(_masterStr, _zeroVolume);
    }

    //public void Load()
    //{
    //    _mixer.SetFloat(_masterStr, _isMuteAllSound ? _zeroVolume : 0);
    //}

    public void FadeIn()
    {
        //VolumeFade(_musicValue, _zeroVolume, _musicStr, _musicCoroutine);
        //VolumeFade(_effectsValue, _zeroVolume, _effectsStr, _effectsCoroutine);
    }

    public void FadeOut()
    {
        //_mixer.SetFloat(_masterStr, _isMuteAllSound ? _zeroVolume : 0);
        //VolumeFade(_zeroVolume, _musicValue, _musicStr, _musicCoroutine, _defoultValue);
        //VolumeFade(_zeroVolume, _effectsValue, _effectsStr, _effectsCoroutine, _defoultValue);
    }

    private void VolumeFade(float startValue, float endValue, string audioType, Coroutine coroutine, float waitTime = 0f)
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = StartCoroutine(Fade(startValue, endValue, waitTime, audioType));
        }
        else
        {
            coroutine = StartCoroutine(Fade(startValue, endValue, waitTime, audioType));
        }
    }

    private IEnumerator Fade(float startValue, float endValue, float waitTime, string audioType)
    {
        _mixer.SetFloat(audioType, startValue);
        yield return new WaitForSeconds(waitTime);

        while (startValue != endValue)
        {
            startValue = Mathf.MoveTowards(startValue, endValue, Time.unscaledDeltaTime * _fadeSpeed);
            _mixer.SetFloat(audioType, startValue);
            yield return null;
        }

        yield break;
    }

    private float GetValue(float value)
    {
        return Mathf.Lerp(_zeroVolume, 0, value);
    }
}
