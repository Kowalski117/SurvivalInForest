using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class RewardHandler : MonoBehaviour
{
    [SerializeField] private RewardSlot[] _rewardSlots;
    [SerializeField] private DayRewardsData _dayRewardsData;
    [SerializeField] private PlayerInventoryHolder _playerInventoryHolder;
    [SerializeField] private Timer _timer;

    private DailyRewardsScreen _dailyRewardsScreen;

    private Coroutine _claimCoroutine;

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
        CreateRewardSlots();
        StartCoroutine();
        SlotsUpdate(_currentStreak);
    }

    private void OnEnable()
    {
        _dailyRewardsScreen.OnOpenScreen += StartCoroutine;
        _dailyRewardsScreen.OnCloseScreen += StopCoroutine;
    }

    private void OnDisable()
    {
        _dailyRewardsScreen.OnOpenScreen -= StartCoroutine;
        _dailyRewardsScreen.OnCloseScreen -= StopCoroutine;
    }

    public void ClaimReward()
    {
        if (!_timer.IsClaimReward)
            return;

        RewardSlot rewardSlot = _rewardSlots[_currentStreak];
        _playerInventoryHolder.AddToInventory(rewardSlot.Slot.AssignedInventorySlot.ItemData, rewardSlot.Slot.AssignedInventorySlot.Size, rewardSlot.Slot.AssignedInventorySlot.Durability);
        rewardSlot.TakeSlot();
        SlotsUpdate(_currentStreak + 1);

        _timer.SetLastClaimTime();
        _currentStreak = (_currentStreak + 1) % _dayRewardsData.DayRewards.Length;

        UpdateRewardsState();
    }

    public void StopCoroutine()
    {
        if (_claimCoroutine != null)
        {
            StopCoroutine(_claimCoroutine);
            _claimCoroutine = null;
        }
    }

    private void StartCoroutine()
    {
        if(_claimCoroutine != null)
        {
            StopCoroutine(_claimCoroutine);
            _claimCoroutine = null;
        }

        _claimCoroutine = StartCoroutine(RewardsStateUpdate());
    }

    private void CreateRewardSlots()
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

    private IEnumerator RewardsStateUpdate()
    {
        UpdateRewardsState();
        yield return new WaitForSeconds(1f);

        StartCoroutine();
    }

    private void UpdateRewardsState()
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
                    _rewardSlots[i-1].ToggleSlot(true);
            }
            else
            {
                _rewardSlots[i].ToggleSlot(false);
            }
        }
    }

    private void ResetSlots()
    {
        foreach (var slot in _rewardSlots)
        {
            slot.ResetSlot();
        } 
    }
}
