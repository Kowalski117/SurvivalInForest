using UnityEngine;
using UnityEngine.UI;

public class RouletteScreen : ScreenUI
{
    [SerializeField] private Button _twistButton;
    [SerializeField] private YandexAds _andexAds;

    private RouletteScrollHandler _scrollHandler;

    public Transform TwistButtonPoint => _twistButton.transform;

    private void Awake()
    {
        _scrollHandler = GetComponent<RouletteScrollHandler>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        _twistButton.onClick.AddListener(TwistButtonClick);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _twistButton.onClick.RemoveListener(TwistButtonClick);
    }

    private void TwistButtonClick()
    {
        _andexAds.ShowRewardAd(() => _scrollHandler.StartTwist());
    }

    protected override void ExitButtonClick()
    {
        base.ExitButtonClick();
        ToggleScreen();
    }
}
