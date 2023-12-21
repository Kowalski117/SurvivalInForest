using UnityEngine;
using UnityEngine.UI;

public class SettingScreen : ScreenUI
{
    [SerializeField] private bool _isOnGeneral;
    [SerializeField] private Toggle _toggleGeneral;
    [SerializeField] private bool _isOnAudio;
    [SerializeField] private Toggle _toggleAudio;

    private void Awake()
    {
        if(_isOnGeneral)
            _toggleGeneral.isOn = true;
        else if(_toggleAudio)
            _toggleAudio.isOn = true;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }

    protected override void ExitButtonClick()
    {
        base.ExitButtonClick();
        ToggleScreen();
        SaveGame.SetCloudSaveData();
        PlayerPrefs.Save();
    }
}
