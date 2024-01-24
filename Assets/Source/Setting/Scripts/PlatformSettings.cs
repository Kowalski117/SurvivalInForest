using UnityEngine;

public class PlatformSettings : MonoBehaviour
{
    [SerializeField] private GameObject[] _desktopPanels;
    [SerializeField] private GameObject[] _mobillePanels;


    private void Start()
    {
        ToggleMobileHUD();
    }

    public void ToggleMobileHUD()
    {
        if (Application.isMobilePlatform)
        {
            foreach (var panel in _desktopPanels)
            {
                panel.gameObject.SetActive(false);
            }

            foreach (var panel in _mobillePanels)
            {
                panel.gameObject.SetActive(true);
            }
        }
        else
        {
            foreach (var panel in _desktopPanels)
            {
                panel.gameObject.SetActive(true);
            }

            foreach (var panel in _mobillePanels)
            {
                panel.gameObject.SetActive(false);
            }
        }
    }
}
