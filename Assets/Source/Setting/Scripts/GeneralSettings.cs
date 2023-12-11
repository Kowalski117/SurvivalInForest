using IL3DN;
using System;
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
    [SerializeField] private TMP_Dropdown _language;

    [SerializeField] private PlayerInputHandler _playerInputHandler;
    [SerializeField] private IL3DN_Wind _wind;
    [SerializeField] private Water _water;

    public Action<float> OnSensitivityChanged;
    public Action<bool> OnAutomaticCollectionChanged;
    public Action<bool> OnMovementOfTreesChanged;
    public Action<bool> OnLowTextureWaterChanged;
    public Action<bool> OnHightTextureWaterChanged;
    public Action<int> OnLanguageChanged;

    private void OnEnable()
    {
        _sensitivitySlider.onValueChanged.AddListener(ChangeSensitivity);
        _automaticCollectionOfItems.onValueChanged.AddListener(ChangeAutomaticCollection);
        _movementOfTrees.onValueChanged.AddListener(ChangeMovementOfTrees);
        _lowTextureWater.onValueChanged.AddListener(ChangeLowTextureWater);
        _hightTextureWater.onValueChanged.AddListener(ChangeHightTextureWater);
    }

    private void OnDisable()
    {
        _sensitivitySlider.onValueChanged.RemoveListener(ChangeSensitivity);
        _automaticCollectionOfItems.onValueChanged.RemoveListener(ChangeAutomaticCollection);
        _movementOfTrees.onValueChanged.RemoveListener(ChangeMovementOfTrees);
        _lowTextureWater.onValueChanged.RemoveListener(ChangeLowTextureWater);
        _hightTextureWater.onValueChanged.RemoveListener(ChangeHightTextureWater);
    }

    private void ChangeSensitivity(float value)
    {
        _playerInputHandler.FirstPersonController.UpdateRotationSpeed(_sensitivitySlider.value);
    }

    private void ChangeAutomaticCollection(bool value)
    {
        _playerInputHandler.Interactor.UpdateIsKeyPickUp(value);
    }

    private void ChangeMovementOfTrees(bool value)
    {
        _wind.UpdateWind(value);
    }

    private void ChangeLowTextureWater(bool value)
    {
        _water.ToggleLowMaterial(value);
    }

    private void ChangeHightTextureWater(bool value)
    {
        _water.ToggleHighMaterial(value);
    }

    private void ChangeLanguage(int value)
    {

    }
}
