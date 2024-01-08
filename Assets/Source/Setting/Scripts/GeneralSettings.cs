using Agava.YandexGames;
using IL3DN;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Device;
using UnityEngine.UI;

public class GeneralSettings : MonoBehaviour
{
    [SerializeField] private Slider _sensitivitySlider;
    [SerializeField] private Toggle _automaticCollectionOfItems;
    [SerializeField] private Toggle _movementOfTrees;
    [SerializeField] private Toggle _lowTextureWater;
    [SerializeField] private Toggle _hightTextureWater;
    [SerializeField] private TMP_Dropdown _language;
    [SerializeField] private Button _authorizationButton;

    [SerializeField] private SettingScreen _screen;
    [SerializeField] private PlayerInputHandler _playerInputHandler;
    [SerializeField] private LocalizationHandler _localizationHandler;
    [SerializeField] private IL3DN_Wind _wind;
    [SerializeField] private Water _water;

    private float _defoultSensitivity = 0.1f;
    private string[] _languages = { "ru", "en", "tr" };

    public Action<float> OnSensitivityChanged;
    public Action<bool> OnAutomaticCollectionChanged;
    public Action<bool> OnMovementOfTreesChanged;
    public Action<bool> OnLowTextureWaterChanged;
    public Action<bool> OnHightTextureWaterChanged;
    public Action<int> OnLanguageChanged;

    private void Start()
    {
        Load();
    }

    private void OnEnable()
    {
        _screen.OnCloseScreen += Save;
        _sensitivitySlider.onValueChanged.AddListener(ChangeSensitivity);
        _automaticCollectionOfItems.onValueChanged.AddListener(ChangeAutomaticCollection);
        _movementOfTrees.onValueChanged.AddListener(ChangeMovementOfTrees);
        _lowTextureWater.onValueChanged.AddListener(ChangeLowTextureWater);
        _hightTextureWater.onValueChanged.AddListener(ChangeHightTextureWater);
        _language.onValueChanged.AddListener(ChangeLanguage);
        _authorizationButton.onClick.AddListener(AuthorizationButtonClick);
    }

    private void OnDisable()
    {
        _screen.OnCloseScreen -= Save;
        _sensitivitySlider.onValueChanged.RemoveListener(ChangeSensitivity);
        _automaticCollectionOfItems.onValueChanged.RemoveListener(ChangeAutomaticCollection);
        _movementOfTrees.onValueChanged.RemoveListener(ChangeMovementOfTrees);
        _lowTextureWater.onValueChanged.RemoveListener(ChangeLowTextureWater);
        _hightTextureWater.onValueChanged.RemoveListener(ChangeHightTextureWater);
        _language.onValueChanged.RemoveListener(ChangeLanguage);
        _authorizationButton.onClick.RemoveListener(AuthorizationButtonClick);
    }

    private void ChangeSensitivity(float value)
    {
        _playerInputHandler.FirstPersonController.UpdateRotationSpeed(value);
    }

    private void ChangeAutomaticCollection(bool value)
    {
        _playerInputHandler.Interactor.UpdateIsKeyPickUp(value);
    }

    private void ChangeMovementOfTrees(bool value)
    {
        if(_wind)
            _wind.UpdateWind(value);
    }

    private void ChangeLowTextureWater(bool value)
    {
        if(_water)
            _water.ToggleLowMaterial(value);
    }

    private void ChangeHightTextureWater(bool value)
    {
        if (_water)
            _water.ToggleHighMaterial(value);
    }

    private void ChangeLanguage(int value)
    {
        PixelCrushers.DialogueSystem.DialogueManager.SetLanguage(_languages[value]);
    }

    private void AuthorizationButtonClick()
    {
        PlayerAccount.Authorize();

#if YANDEX_GAMES && UNITY_WEBGL && !UNITY_EDITOR
        if (PlayerAccount.IsAuthorized)
            _authorizationButton.gameObject.SetActive(false);
#endif
    }

    private void Save()
    {
        PlayerPrefs.SetFloat(SettingConstants.Sensitivity, _sensitivitySlider.value);
        PlayerPrefs.SetInt(SettingConstants.AutomaticCollectionOfItems, _automaticCollectionOfItems.isOn ? 1 : 0);
        PlayerPrefs.SetInt(SettingConstants.MovementOfTrees, _movementOfTrees.isOn ? 1 : 0);
        PlayerPrefs.SetInt(SettingConstants.LowTextureWater, _lowTextureWater.isOn ? 1 : 0);
        PlayerPrefs.SetInt(SettingConstants.HightTextureWater, _hightTextureWater.isOn ? 1 : 0);
    }

    private void Load()
    {
        if (PlayerPrefs.HasKey(ConstantsSDK.Language))
            _localizationHandler.SetLanguageString(PlayerPrefs.GetString(ConstantsSDK.Language));

        for (int i = 0; i < _languages.Length; i++)
        {
            if(PlayerPrefs.GetString(ConstantsSDK.Language) == _languages[i])
                _language.value = i;
        }

        if (PlayerPrefs.HasKey(SettingConstants.Sensitivity))
            _sensitivitySlider.value = PlayerPrefs.GetFloat(SettingConstants.Sensitivity);
        else
            _sensitivitySlider.value = _defoultSensitivity;

        if (PlayerPrefs.HasKey(SettingConstants.Sensitivity))
            _automaticCollectionOfItems.isOn = PlayerPrefs.GetInt(SettingConstants.AutomaticCollectionOfItems) == 0 ? false : true;
        else
            _automaticCollectionOfItems.isOn = true;

        if (PlayerPrefs.HasKey(SettingConstants.Sensitivity))
            _movementOfTrees.isOn = PlayerPrefs.GetInt(SettingConstants.MovementOfTrees) == 0 ? false : true;
        else
            _movementOfTrees.isOn = true;

        if (PlayerPrefs.HasKey(SettingConstants.Sensitivity))
            _lowTextureWater.isOn = PlayerPrefs.GetInt(SettingConstants.LowTextureWater) == 0 ? false : true;
        else
            _lowTextureWater.isOn = false;

        if (PlayerPrefs.HasKey(SettingConstants.Sensitivity))       
            _hightTextureWater.isOn = PlayerPrefs.GetInt(SettingConstants.HightTextureWater) == 0 ? false : true;      
        else
            _hightTextureWater.isOn = true;

        ChangeSensitivity(_sensitivitySlider.value);
        ChangeAutomaticCollection(_automaticCollectionOfItems.isOn);
        ChangeMovementOfTrees(_movementOfTrees.isOn);
        ChangeLowTextureWater(_lowTextureWater.isOn);
        ChangeHightTextureWater(_hightTextureWater.isOn);
    }
}
