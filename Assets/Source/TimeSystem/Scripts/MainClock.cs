using System;
using UnityEngine;

public class MainClock : MonoBehaviour
{
    [SerializeField] private int _dayCounter = 0;
    [SerializeField] private float _timeMultiplier;
    [SerializeField] private float _startHour;
    [SerializeField] private float _dayHour;
    [SerializeField] private float _nightHour;

    private DateTime _currentTime;
    private DateTime _lastDayUpdate = DateTime.Now;

    private bool _isEnable = false;

    public event Action<DateTime> OnTimeUpdated;
    public event Action<int> OnDayUpdated;

    public float TimeMultiplier => _timeMultiplier;
    public float CurrentHurts => _currentTime.Hour;
    public DateTime CurrentTime => _currentTime;
    public bool IsEnable => _isEnable;


    private void Start()
    {
        _currentTime = DateTime.Now.Date + TimeSpan.FromHours(_startHour);
        OnDayUpdated?.Invoke(_dayCounter);
    }

    private void Update()
    {
        if (_isEnable)
            UpdateTimeDay();
    }

    private void OnEnable()
    {
        SavingGame.OnGameSaved += Save;
        SavingGame.OnGameLoaded += Load;
        SavingGame.OnSaveDeleted += Delete;
    }

    private void OnDisable()
    {
        SavingGame.OnGameSaved -= Save;
        SavingGame.OnGameLoaded -= Load;
        SavingGame.OnSaveDeleted -= Delete;
    }

    public void AddTimeInSeconds(float time)
    {
        _currentTime = _currentTime.AddSeconds(time);
        OnTimeUpdated?.Invoke(_currentTime);
    }

    public void AddTimeInHours(float time)
    {
        _currentTime = _currentTime.AddHours(time);
        OnTimeUpdated?.Invoke(_currentTime);
    }

    public void ToggleEnable(bool isActive)
    {
        _isEnable = isActive;
    }

    private void UpdateTimeDay()
    {
        _currentTime = _currentTime.AddSeconds(Time.deltaTime * _timeMultiplier);
        OnTimeUpdated?.Invoke(_currentTime);

        if (_currentTime.Date > _lastDayUpdate.Date)
        {
            _lastDayUpdate = _currentTime;
            _dayCounter++;
            OnDayUpdated?.Invoke(_dayCounter);
        }
    }

    public void TimeScale()
    {
        if (_timeMultiplier == 60)
            _timeMultiplier = 6000;
        else
            _timeMultiplier = 60;
    }

    private void Save()
    {
        ES3.Save(SaveLoadConstants.Time, _currentTime);
    }

    private void Load()
    {
        if(ES3.KeyExists(SaveLoadConstants.Time))
           _currentTime = ES3.Load<DateTime>(SaveLoadConstants.Time, DateTime.Now.Date + TimeSpan.FromHours(_startHour));
    }

    private void Delete()
    {
        if (ES3.KeyExists(SaveLoadConstants.Time))
            ES3.DeleteKey(SaveLoadConstants.Time);
    }
}

