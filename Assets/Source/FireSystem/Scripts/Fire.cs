using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
public class Fire : MonoBehaviour
{
    [SerializeField] private CraftObject _craftObject;
    [SerializeField] private GameObject _fireParticle;
    [SerializeField] private bool _isRemoveAfterFire = false;
    [SerializeField] private InventoryItemData[] _campfireItems;
    [SerializeField] private AudioClip _gorenjeClip;

    private AudioSource _audioSource;
    private float _workingHours = 2f;
    private float _maxHours = 5f;
    private Building _building;
    private DateTime _currentTime;
    private DateTime _maxTimer;
    private TimeHandler _timeHandler;
    private bool _isFire = false;
    private bool _isEnable = true;
    private UniqueID _uniqueId;

    public event UnityAction<DateTime> OnCompletionTimeUpdate;
    public event UnityAction<bool> OnToggledFire;

    public bool IsFire => _isFire;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _uniqueId = GetComponentInParent<UniqueID>();
        _timeHandler = FindObjectOfType<TimeHandler>();
        _building = GetComponentInParent<Building>();
        _fireParticle.gameObject.SetActive(false);
        _maxTimer = _maxTimer + TimeSpan.FromHours(_maxHours);
    }

    private void Start()
    {
        Load();
    }

    private void OnEnable()
    {
        SleepPanel.OnStoppedTime += ToggleEnable;
        SleepPanel.OnSubtractTime += ReduceTime;
        _building.OnCompletedBuild += StartFire;

        SaveGame.OnSaveGame += Save;
    }

    private void OnDisable()
    {
        SleepPanel.OnStoppedTime -= ToggleEnable;
        SleepPanel.OnSubtractTime -= ReduceTime;
        _building.OnCompletedBuild -= StartFire;

        SaveGame.OnSaveGame -= Save;
    }

    private void Update()
    {
        if (_isFire && _isEnable)
        {
            UpdateTimer(_timeHandler.TimeMultiplier);
        }
    }

    public bool AddFire(InventorySlot slot)
    {
        if (slot.ItemData != null && slot.ItemData.GorenjeTime > 0 && slot.Size > 0)
        {
            if (_campfireItems.Contains(slot.ItemData) || _campfireItems.Length == 0)
            {
                return AddTime(slot.ItemData.GorenjeTime);
            }
        }

        return false;
    }

    private void UpdateTimer(float time)
    {
        if(_currentTime.TimeOfDay.TotalSeconds >= time)
        {
            _currentTime = _currentTime.AddSeconds(-Time.deltaTime * time);
            OnCompletionTimeUpdate?.Invoke(_currentTime);
        }
        else
        {
            _currentTime = DateTime.MinValue;
            OnCompletionTimeUpdate?.Invoke(_currentTime);
        }

        if (_currentTime == DateTime.MinValue)
        {
            _fireParticle.gameObject.SetActive(false);
            _craftObject.TurnOff();
            _isFire = false;
            OnToggledFire?.Invoke(_isFire);
            _audioSource.Stop();

            if (_isRemoveAfterFire)
                Destroy(gameObject);
        }
    }

    private void StartFire()
    {
        _currentTime = _currentTime + TimeSpan.FromHours(_workingHours);
        EnableParticle();
    }

    private void EnableParticle()
    {
        _craftObject.enabled = true;
        _fireParticle.gameObject.SetActive(true);
        _isFire = true;
        OnToggledFire?.Invoke(_isFire);
        _audioSource.clip = _gorenjeClip;
        _audioSource.Play();
    }

    private void ReduceTime(float time)
    {
        if (_currentTime.TimeOfDay.Hours >= time)
        {
            _currentTime = _currentTime.AddHours(-time);
            OnCompletionTimeUpdate?.Invoke(_currentTime);
            return;
        }
        else
        {
            _currentTime = DateTime.MinValue;
            OnCompletionTimeUpdate?.Invoke(_currentTime);
            return;
        }
    }

    private bool AddTime(float time)
    {
        if (_currentTime < _maxTimer)
        {
            _currentTime = _currentTime.AddHours(time);
            OnCompletionTimeUpdate?.Invoke(_currentTime);

            if (!_isFire)
            {
                EnableParticle();
            }

            return true;
        }
        return false;
    }

    private void ToggleEnable(bool value)
    {
        _isEnable = value;
    }

    private void Save()
    {
        FireSaveData fireSaveData = new FireSaveData(_currentTime);
        ES3.Save(_uniqueId.Id + SaveLoadConstants.FireSaveData, fireSaveData);
    }

    private void Load()
    {
        if (ES3.KeyExists(_uniqueId.Id + SaveLoadConstants.FireSaveData))
        {
            FireSaveData fireSaveData = ES3.Load<FireSaveData>(_uniqueId.Id + SaveLoadConstants.FireSaveData);
            _currentTime = fireSaveData.CurrentTime;
            EnableParticle();
        }
    }
}

[System.Serializable]
public struct FireSaveData
{
    [SerializeField] private DateTime _currentTime;

    public DateTime CurrentTime => _currentTime;

    public FireSaveData(DateTime currentTime)
    {
        _currentTime = currentTime;
    }
}
