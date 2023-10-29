using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class RewardHandler : MonoBehaviour
{
    [SerializeField] private RewardSlot[] _rewardSlots;
    [SerializeField] private DayRewardsData _dayRewardsData;
    [SerializeField] private PlayerInventoryHolder _playerInventoryHolder;

    [SerializeField] private TMP_Text _statusText;

    private DailyRewardsScreen _dailyRewardsScreen;

    private bool _isClaimReward;
    private float _claimCooldown = 24f / 24/ 60 / 6/ 2;
    private float _claimDeadline = 48f / 24 / 60 / 6 / 2;

    private Coroutine _claimCoroutine;

    private int _currentStreak
    {
        get => PlayerPrefs.GetInt(SaveLoadConstants.CurrentStreak, 0);
        set => PlayerPrefs.SetInt(SaveLoadConstants.CurrentStreak, value);
    }

    private DateTime? _lastClaimTime
    {
        get
        {
            string data = PlayerPrefs.GetString(SaveLoadConstants.LastClaimTime, null);

            if (!string.IsNullOrEmpty(data))
                return DateTime.Parse(data);

            return null;
        }
        set
        {
            if (value != null)
                PlayerPrefs.SetString(SaveLoadConstants.LastClaimTime, value.ToString());
            else
                PlayerPrefs.DeleteKey(SaveLoadConstants.LastClaimTime);
        }
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
        if (!_isClaimReward)
            return;

        RewardSlot rewardSlot = _rewardSlots[_currentStreak];
        _playerInventoryHolder.AddToInventory(rewardSlot.Slot.AssignedInventorySlot.ItemData, rewardSlot.Slot.AssignedInventorySlot.Size, rewardSlot.Slot.AssignedInventorySlot.Durability);
        rewardSlot.TakeSlot();
        SlotsUpdate(_currentStreak + 1);

        _lastClaimTime = DateTime.UtcNow;
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
        _isClaimReward = true;

        if(_lastClaimTime.HasValue)
        {
            var timeSpan = DateTime.UtcNow - _lastClaimTime.Value;

            if(timeSpan.TotalHours > _claimDeadline)
            {
                _lastClaimTime = null;
                ResetSlots();
                _currentStreak = 0;
                SlotsUpdate(_currentStreak);

            }
            else if(timeSpan.TotalHours < _claimCooldown)
                _isClaimReward = false;
        }

        UpdateRewardsUI();
    }

    private void UpdateRewardsUI()
    {
        if (_isClaimReward)
            _statusText.text = "Получи награду!";
        else
        {
            var nextClaimTime = _lastClaimTime.Value.AddHours(_claimCooldown);
            var currentClaimCooldown = nextClaimTime - DateTime.UtcNow;

            string cd = $"{currentClaimCooldown.Hours:D2}:{currentClaimCooldown.Minutes:D2}:{currentClaimCooldown.Seconds:D2}";

            _statusText.text = $"До получения подарка осталось {cd}";
        }
    }

    private void SlotsUpdate(int index)
    {
        for (int i = 0; i < _rewardSlots.Length; i++)
        {
            if(i == index)
            {
                _rewardSlots[i].ToggleSlot(true);
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
