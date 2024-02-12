using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class AudioHandler : MonoBehaviour
{
    [SerializeField] private AudioMixer _mixer;
    [SerializeField] private SettingScreen _settingScreen;

    private const float _zeroVolume = -80f;
    private const float _defoultValue = 0.8f;

    private bool _isMuteAllSound = false;
    private bool _isMuteMusic = false;
    private bool _isMuteEffects = false;
    private bool _isMute = false;
    private bool _isMutePrevious = false;

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
        StartCoroutine(Load());
    }

    public IEnumerator Load()
    {
        _currentValueMaster = PlayerPrefs.HasKey(SettingConstants.MasterStr) ? PlayerPrefs.GetFloat(SettingConstants.MasterStr) : 1;
        _currentValueMusic = PlayerPrefs.HasKey(SettingConstants.MusicStr) ? PlayerPrefs.GetFloat(SettingConstants.MusicStr) : _defoultValue;
        _currentValueEffects = PlayerPrefs.HasKey(SettingConstants.EffectsStr) ? PlayerPrefs.GetFloat(SettingConstants.EffectsStr) : _defoultValue;
        _isMuteAllSound = (PlayerPrefs.GetFloat(SettingConstants.MasterStr + SettingConstants.BoolStr) == 1) ? true : false;
        _isMuteMusic = (PlayerPrefs.GetFloat(SettingConstants.MusicStr + SettingConstants.BoolStr) == 1) ? true : false;
        yield return _isMuteEffects = (PlayerPrefs.GetFloat(SettingConstants.EffectsStr + SettingConstants.BoolStr) == 1) ? true : false;

        if (!_isMute) 
            SetAllSound(_currentValueMaster);
        SetMusic(_currentValueMusic);
        SetEffects(_currentValueEffects);
    }

    public void Save()
    {
        PlayerPrefs.Save();
    }

    public void OnOffAllSound()
    {
        _isMuteAllSound = !_isMuteAllSound;
        _mixer.SetFloat(SettingConstants.MasterStr, _isMuteAllSound ? _zeroVolume : GetValue(_currentValueMaster));
        PlayerPrefs.SetFloat(SettingConstants.MasterStr + SettingConstants.BoolStr, _isMuteAllSound ? 1 : 0);
    }

    public void OnOffMusic()
    {
        _isMuteMusic = !_isMuteMusic;
        _mixer.SetFloat(SettingConstants.MusicStr, _isMuteMusic ? _zeroVolume : GetValue(_currentValueMusic));
        PlayerPrefs.SetFloat(SettingConstants.MusicStr + SettingConstants.BoolStr, _isMuteMusic ? 1 : 0);
    }

    public void OnOffEffects()
    {
        _isMuteEffects = !_isMuteEffects;
        _mixer.SetFloat(SettingConstants.EffectsStr, _isMuteEffects ? _zeroVolume : GetValue(_currentValueEffects));
        PlayerPrefs.SetFloat(SettingConstants.EffectsStr + SettingConstants.BoolStr, _isMuteEffects ? 1 : 0);
    }

    public void SetAllSound(float value)
    {
        _currentValueMaster =  value;

        if(!_isMuteAllSound) 
            _mixer.SetFloat(SettingConstants.MasterStr, GetValue(value));
        else
            _mixer.SetFloat(SettingConstants.MasterStr,  _zeroVolume);

        PlayerPrefs.SetFloat(SettingConstants.MasterStr, value);
    }

    public void SetMusic(float value)
    {
        _currentValueMusic =  value;

        if (!_isMuteMusic)
            _mixer.SetFloat(SettingConstants.MusicStr, GetValue(value));
        else
            _mixer.SetFloat(SettingConstants.MusicStr, _zeroVolume);

        PlayerPrefs.SetFloat(SettingConstants.MusicStr, _currentValueMusic);
    }

    public void SetEffects(float value)
    {
        _currentValueEffects = value;

        if (!_isMuteEffects)
            _mixer.SetFloat(SettingConstants.EffectsStr, GetValue(value));
        else
            _mixer.SetFloat(SettingConstants.EffectsStr, _zeroVolume);

        PlayerPrefs.SetFloat(SettingConstants.EffectsStr, _currentValueEffects);
    }

    public void FadeIn()
    {
        _isMute = true;
        _mixer.SetFloat(SettingConstants.MasterStr, _zeroVolume);
    }

    public void FadeOut()
    {
        _isMute = false;
        SetAllSound(_currentValueMaster);
    }

    private float GetValue(float value)
    {
        return Mathf.Lerp(_zeroVolume, 0, value);
    }

    private void OnApplicationFocus(bool focus)
    {
        if(_isMutePrevious && focus)
        {
            _isMutePrevious = false;
            return;
        }
        _isMutePrevious = _isMute; 

        if (focus)
        {
            FadeOut();
        }
        else
        {
            FadeIn();
        }
    }
}
