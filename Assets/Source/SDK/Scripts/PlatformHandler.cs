using UnityEngine;

public class PlatformHandler : MonoBehaviour
{
    [SerializeField] private GeneralSettings _settings;
    [SerializeField] private GameObject _mobileHUD;
    [SerializeField] private GameObject _desktopHUD;

    private void Start()
    {
        ToggleMobileHUD();
    }

    public void ToggleMobileHUD()
    {
        if (Application.isMobilePlatform)
        {
            _mobileHUD.SetActive(true);
            _desktopHUD.SetActive(false);
        }
        else
        {
            _mobileHUD.SetActive(false);
            _desktopHUD.SetActive(_settings.IsActiveTipsInput);
        }
    }
}
