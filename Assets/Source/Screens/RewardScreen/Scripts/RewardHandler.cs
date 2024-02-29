using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DailyRewardsScreen))]
public class RewardHandler : MonoBehaviour
{
    private const float UpdateStateDelay = 1f;

    [SerializeField] private RewardSlot[] _rewardSlots;
    [SerializeField] private DayRewardsData _dayRewardsData;
    [SerializeField] private PlayerInventoryHolder _playerInventoryHolder;
    [SerializeField] private Timer _timer;

    private DailyRewardsScreen _dailyRewardsScreen;

    private Coroutine _claimCoroutine;
    private WaitForSeconds _updateStateWait = new WaitForSeconds(UpdateStateDelay);

    public event Action<Dictionary<InventoryItemData, int>> OnBonusShown;

    private int _currentStreak
    {
        get => PlayerPrefs.GetInt(SaveLoadConstants.CurrentStreak, 0);
        set => PlayerPrefs.SetInt(SaveLoadConstants.CurrentStreak, value);
    }

    private void Awake()
    {
        _dailyRewardsScreen = GetComponent<DailyRewardsScreen>();
    }

    private void Start()
    {
        CreateSlots();
        StartCoroutine();
        SlotsUpdate(_currentStreak);
    }

    private void OnEnable()
    {
        _dailyRewardsScreen.OnScreenOpened += StartCoroutine;
        _dailyRewardsScreen.OnScreenClosed += StopCoroutine;
    }

    private void OnDisable()
    {
        _dailyRewardsScreen.OnScreenOpened -= StartCoroutine;
        _dailyRewardsScreen.OnScreenClosed -= StopCoroutine;
    }

    public void Claim()
    {
        if (!_timer.IsClaimReward)
            return;

        RewardSlot rewardSlot = _rewardSlots[_currentStreak];
        AddItem(rewardSlot);
        rewardSlot.Take();
        SlotsUpdate(_currentStreak + 1);

        _timer.SetLastClaimTime();
        _currentStreak = (_currentStreak + 1) % _dayRewardsData.DayRewards.Length;

        VerifyState();
    }

    public void StopCoroutine()
    {
        if (_claimCoroutine != null)
        {
            StopCoroutine(_claimCoroutine);
            _claimCoroutine = null;
        }
    }

    private void AddItem(RewardSlot rewardSlot)
    {
        Dictionary<InventoryItemData, int> items = new Dictionary<InventoryItemData, int>();
        items.Add(rewardSlot.Slot.AssignedInventorySlot.ItemData, rewardSlot.Slot.AssignedInventorySlot.Size);

        OnBonusShown?.Invoke(items);
    }

    private void StartCoroutine()
    {
        if(_claimCoroutine != null)
        {
            StopCoroutine(_claimCoroutine);
            _claimCoroutine = null;
        }

        _claimCoroutine = StartCoroutine(UpdateState());
    }

    private void CreateSlots()
    {
        for (int i = 0; i < _dayRewardsData.DayRewards.Length; i++)
        {
            foreach (var slot in _rewardSlots)
            {
                if (slot.IsEmpty)
                {
                    slot.Init(_dayRewardsData.DayRewards[i], i + 1);
                    break;
                }
            }
        }

        foreach (var slot in _rewardSlots)
        {
            if (slot.IsEmpty)
            {
                slot.gameObject.SetActive(false);
            }
        }
    }

    private IEnumerator UpdateState()
    {
        VerifyState();
        yield return _updateStateWait;

        StartCoroutine();
    }

    private void VerifyState()
    {
        if (_timer.IsCheckState())
        {
            _timer.Clear();
            ResetSlots();
            _currentStreak = 0;
            SlotsUpdate(_currentStreak);
        }

        _timer.UpdateRewardsUI();
    }

    private void SlotsUpdate(int index)
    {
        for (int i = 0; i < _rewardSlots.Length; i++)
        {
            if(i == index)
            {
                if(i - 1 >= 0)
                    _rewardSlots[i-1].Take();
            }
        }
    }

    private void ResetSlots()
    {
        foreach (var slot in _rewardSlots)
        {
            slot.Clear();
        } 
    }
}
