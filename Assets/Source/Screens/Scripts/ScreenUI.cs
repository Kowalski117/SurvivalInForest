using UnityEngine;

public class ScreenUI : MonoBehaviour
{
    [SerializeField] private CanvasGroup _panel;

    public void OpenScreen()
    {
        _panel.blocksRaycasts = true;
        _panel.alpha = Mathf.Lerp(0, 1, 1);
    }

    public void CloseScreen()
    {
        _panel.blocksRaycasts = false;
        _panel.alpha = _panel.alpha = Mathf.Lerp(1, 0, 1);
    }
}
