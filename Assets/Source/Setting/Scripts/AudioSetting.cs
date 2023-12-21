using UnityEngine;
using UnityEngine.UI;

public class AudioSetting : MonoBehaviour
{
    [SerializeField] private Slider _allSoundSlider;
    [SerializeField] private Button _onAllSoundButton;
    [SerializeField] private Button _offAllSoundButton;

    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Button _onMusicButton;
    [SerializeField] private Button _offMusicButton;

    [SerializeField] private Slider _effectSlider;
    [SerializeField] private Button _onEffectButton;
    [SerializeField] private Button _offEffectButton;

    [SerializeField] private AudioHandler _audioHandler;

    private void Start()
    {
        Load();
    }

    private void OnEnable()
    {
        _allSoundSlider.onValueChanged.AddListener(ChangeValueAllSound);
        _onAllSoundButton.onClick.AddListener(ToggleAllSound);
        _offAllSoundButton.onClick.AddListener(ToggleAllSound);

        _musicSlider.onValueChanged.AddListener(ChangeValueMusic);
        _onMusicButton.onClick.AddListener(ToggleMusic);
        _offMusicButton.onClick.AddListener(ToggleMusic);

        _effectSlider.onValueChanged.AddListener(ChangeValueEffects);
        _onEffectButton.onClick.AddListener(ToggleEffects);
        _offEffectButton.onClick.AddListener(ToggleEffects);
    }

    private void OnDisable()
    {
        _onAllSoundButton.onClick.RemoveListener(ToggleAllSound);
        _offAllSoundButton.onClick.RemoveListener(ToggleAllSound);

        _onMusicButton.onClick.RemoveListener(ToggleMusic);
        _offMusicButton.onClick.RemoveListener(ToggleMusic);

        _onEffectButton.onClick.RemoveListener(ToggleEffects);
        _offEffectButton.onClick.RemoveListener(ToggleEffects);
    }

    private void ToggleAllSound()
    {
        _audioHandler.OnOffAllSound();
    }

    private void ToggleMusic()
    {
        _audioHandler.OnOffMusic();
    }

    private void ToggleEffects()
    {
        _audioHandler.OnOffEffects();
    }

    private void ChangeValueAllSound(float value)
    {
        _audioHandler.SetAllSound(value);
    }

    private void ChangeValueMusic(float value)
    {
        _audioHandler.SetMusic(value);
    }

    private void ChangeValueEffects(float value)
    {
        _audioHandler.SetEffects(value);
    }

    private void ToggleButtons(bool value, Button onButton, Button offButton)
    {
        if (value)
        {
            onButton.gameObject.SetActive(false);
            offButton.gameObject.SetActive(true);
        }
        else
        {
            onButton.gameObject.SetActive(true);
            offButton.gameObject.SetActive(false);
        }
    }

    private void Load()
    {
        //ChangeValueAllSound(_audioHandler.CurrentVolumeMaster);
        _allSoundSlider.value = _audioHandler.CurrentVolumeMaster;
        ToggleButtons(_audioHandler.IsMuteAllSound, _onAllSoundButton, _offAllSoundButton);

        //ChangeValueMusic(_audioHandler.CurrentVolumeMusic);
        _musicSlider.value = _audioHandler.CurrentVolumeMusic;
        ToggleButtons(_audioHandler.IsMuteMusic, _onMusicButton, _offMusicButton);

        //ChangeValueEffects(_audioHandler.CurrentVolumeEffects);
        _effectSlider.value = _audioHandler.CurrentVolumeEffects;
        ToggleButtons(_audioHandler.IsMuteEffects, _onEffectButton, _offEffectButton);
    }
}
