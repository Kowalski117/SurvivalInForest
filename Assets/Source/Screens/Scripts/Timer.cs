using System;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(UniqueID))]
public class Timer : MonoBehaviour
{
    [SerializeField] private TMP_Text _statusText;
    [SerializeField] private float _claimCooldown = 24f / 24 / 60 / 6 / 2;
    [SerializeField] private float _claimDeadline = 48f / 24 / 60 / 6 / 2;

    private UniqueID _uniqueId;
    private bool _isPlaying = false;
    
    public event Action OnTimerExpired;

    public bool IsClaimReward { get; private set; }

    public DateTime? LastClaimTime
    {
        get
        {
            string data = PlayerPrefs.GetString(SaveLoadConstants.LastClaimTime + _uniqueId.Id, null);

            if (!string.IsNullOrEmpty(data))
                return DateTime.Parse(data);

            return null;
        }
        private set
        {
            if (value != null)
                PlayerPrefs.SetString(SaveLoadConstants.LastClaimTime + _uniqueId.Id, value.ToString());
            else
                Delete();
        }
    }

    private void Awake()
    {
        _uniqueId = GetComponent<UniqueID>();
    }

    private void OnEnable()
    {
        SavingGame.OnSaveDeleted += Delete;
    }

    private void OnDisable()
    {
        SavingGame.OnSaveDeleted -= Delete;
    }

    public void UpdateRewardsUI()
    {
        if (IsClaimReward)
        {
            _statusText.text = GameConstants.DefaultTime;

            if(_isPlaying)
                OnTimerExpired?.Invoke();

            _isPlaying = false;
        }
        else
        {
            if (LastClaimTime.HasValue)
            {
                var nextClaimTime = LastClaimTime.Value.AddHours(_claimCooldown);
                var currentClaimCooldown = nextClaimTime - DateTime.UtcNow;

                _statusText.text = $"{currentClaimCooldown.Hours:D2}:{currentClaimCooldown.Minutes:D2}:{currentClaimCooldown.Seconds:D2}";

                _isPlaying = true;
            }
        }
    }

    public bool IsCheckState()
    {
        IsClaimReward = true;

        if (LastClaimTime.HasValue)
        {
            var timeSpan = DateTime.UtcNow - LastClaimTime.Value;

            if (timeSpan.TotalHours > _claimDeadline)
            {
                return true;

            }
            else if (timeSpan.TotalHours < _claimCooldown)
            {
                IsClaimReward = false;
                return false;
            }
        }

        return false;
    }

    public void SetLastClaimTime()
    {
        LastClaimTime = DateTime.UtcNow;
    }

    public void Clear()
    {
        LastClaimTime = null;
    }

    private void Delete()
    {
        if(PlayerPrefs.HasKey(SaveLoadConstants.LastClaimTime + _uniqueId.Id))
            PlayerPrefs.DeleteKey(SaveLoadConstants.LastClaimTime + _uniqueId.Id);
    }
}
