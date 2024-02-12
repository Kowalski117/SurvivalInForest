using Agava.YandexGames;
using IL3DN;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GeneralSettings : MonoBehaviour
{
    [SerializeField] private Slider _sensitivitySlider;
    [SerializeField] private Toggle _automaticCollectionOfItems;
    [SerializeField] private Toggle _movementOfTrees;
    [SerializeField] private Toggle _lowTextureWater;
    [SerializeField] private Toggle _hightTextureWater;
    [SerializeField] private Toggle _tipsInput;
    [SerializeField] private TMP_Dropdown _language;
    [SerializeField] private Button _authorizationButton;

    [SerializeField] private SettingScreen _screen;
    [SerializeField] private PlayerHandler _playerInputHandler;
    [SerializeField] private LocalizationHandler _localizationHandler;
    [SerializeField] private IL3DN_Wind _wind;
    [SerializeField] private Water _water;
    [SerializeField] private ButtonsInputHandler _buttonsInputHandler;

    private float _defoultSensitivity = 0.1f;

    public bool IsActiveTipsInput => _tipsInput.isOn;

    private void Start()
    {
        Load();
    }

    private void OnEnable()
    {
        _screen.OnCloseScreen += Save;
        _sensitivitySlider.onValueChanged.AddListener(ChangeSensitivity);
        _tipsInput.onValueChanged.AddListener(ChangeActiveTipsInput);
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
        _tipsInput.onValueChanged.RemoveListener(ChangeActiveTipsInput);
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

    private void ChangeActiveTipsInput(bool value)
    {
        if (!_buttonsInputHandler)
            return;

        if(value)
            _buttonsInputHandler.gameObject.SetActive(true);
        else
            _buttonsInputHandler.gameObject.SetActive(false);
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
        _localizationHandler.SetLanguageIndex(value);
    }

    private void AuthorizationButtonClick()
    {
#if YANDEX_GAMES && UNITY_WEBGL && !UNITY_EDITOR
        PlayerAccount.Authorize();
#endif
    }

    private void Save()
    {
        PlayerPrefs.SetFloat(SettingConstants.Sensitivity, _sensitivitySlider.value);
        PlayerPrefs.SetInt(SettingConstants.TipsInput, _tipsInput.isOn ? 1 : 0);
        PlayerPrefs.SetInt(SettingConstants.AutomaticCollectionOfItems, _automaticCollectionOfItems.isOn ? 1 : 0);
        PlayerPrefs.SetInt(SettingConstants.MovementOfTrees, _movementOfTrees.isOn ? 1 : 0);
        PlayerPrefs.SetInt(SettingConstants.LowTextureWater, _lowTextureWater.isOn ? 1 : 0);
        PlayerPrefs.SetInt(SettingConstants.HightTextureWater, _hightTextureWater.isOn ? 1 : 0);
    }

    private void Load()
    {
        if (PlayerPrefs.HasKey(ConstantsSDK.Language))
            _localizationHandler.SetLanguageString(PlayerPrefs.GetString(ConstantsSDK.Language));

        if (!_sensitivitySlider.gameObject.activeInHierarchy)
            return;

        if (PlayerPrefs.HasKey(SettingConstants.Sensitivity))
            _sensitivitySlider.value = PlayerPrefs.GetFloat(SettingConstants.Sensitivity);
        else
            _sensitivitySlider.value = _defoultSensitivity;

        if (PlayerPrefs.HasKey(SettingConstants.TipsInput))
            _tipsInput.isOn = PlayerPrefs.GetInt(SettingConstants.TipsInput) == 0 ? false : true;
        else
            _tipsInput.isOn = true;

        if (PlayerPrefs.HasKey(SettingConstants.AutomaticCollectionOfItems))
            _automaticCollectionOfItems.isOn = PlayerPrefs.GetInt(SettingConstants.AutomaticCollectionOfItems) == 0 ? false : true;
        else
            _automaticCollectionOfItems.isOn = true;

        if (PlayerPrefs.HasKey(SettingConstants.MovementOfTrees))
            _movementOfTrees.isOn = PlayerPrefs.GetInt(SettingConstants.MovementOfTrees) == 0 ? false : true;
        else
            _movementOfTrees.isOn = true;

        if (PlayerPrefs.HasKey(SettingConstants.LowTextureWater))
            _lowTextureWater.isOn = PlayerPrefs.GetInt(SettingConstants.LowTextureWater) == 0 ? false : true;
        else
            _lowTextureWater.isOn = false;

        if (PlayerPrefs.HasKey(SettingConstants.HightTextureWater))       
            _hightTextureWater.isOn = PlayerPrefs.GetInt(SettingConstants.HightTextureWater) == 0 ? false : true;      
        else
            _hightTextureWater.isOn = true;

        ChangeSensitivity(_sensitivitySlider.value);
        ChangeActiveTipsInput(_tipsInput.isOn);
        ChangeAutomaticCollection(_automaticCollectionOfItems.isOn);
        ChangeMovementOfTrees(_movementOfTrees.isOn);
        ChangeLowTextureWater(_lowTextureWater.isOn);
        ChangeHightTextureWater(_hightTextureWater.isOn);
    }
}
