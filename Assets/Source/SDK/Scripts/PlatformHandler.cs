using UnityEngine;

public class PlatformHandler : MonoBehaviour
{
    [SerializeField] private GeneralSettings _settings;
    [SerializeField] private AnimationUI _mobileHUD;
    [SerializeField] private AnimationUI _desktopHUD;

    private void Start()
    {
        ToggleMobileHUD();
    }

    public void ToggleMobileHUD()
    {
        if (Application.isMobilePlatform)
        {
            _mobileHUD.OpenAnimation();
            _desktopHUD.CloseAnimation();
        }
        else
        {
            _mobileHUD.CloseAnimation();

            if(_settings.IsActiveTipsInput)
                _desktopHUD.OpenAnimation();
        }
    }
}
