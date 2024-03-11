using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RewardHandler))]
public class DailyRewardsScreen : ScreenUI
{
    [SerializeField] private Button _claimButton;

    private RewardHandler _handler;

    protected override void Awake()
    {
        base.Awake();

        _handler = GetComponent<RewardHandler>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        _claimButton.onClick.AddListener(ClaimButtonClick);
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        _claimButton.onClick.RemoveListener(ClaimButtonClick);
    }

    protected override void ExitButtonClick()
    {
        base.ExitButtonClick();
        Toggle();
    }

    private void ClaimButtonClick()
    {
        _handler.Claim();
    }
}
