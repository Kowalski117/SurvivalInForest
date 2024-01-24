using UnityEngine;
using UnityEngine.UI;

public class RouletteScreen : ScreenUI
{
    [SerializeField] private Button _twistButton;

    private RouletteScrollHandler _scrollHandler;

    public Transform TwistButtonPoint => _twistButton.transform;

    private void Awake()
    {
        _scrollHandler = GetComponent<RouletteScrollHandler>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        _twistButton.onClick.AddListener(_scrollHandler.TwistButtonClick);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _twistButton.onClick.RemoveListener(_scrollHandler.TwistButtonClick);
    }

    protected override void ExitButtonClick()
    {
        base.ExitButtonClick();
        ToggleScreen();
    }
}
