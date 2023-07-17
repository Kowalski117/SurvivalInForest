using System;
using UnityEngine;
using UnityEngine.Events;

public class TimeHandler : MonoBehaviour
{
    [SerializeField] private float _timeMultiplier;
    [SerializeField] private float _startHour;
    [SerializeField] private float _sunriseHour;
    [SerializeField] private float _sunsetHour;
    [SerializeField] private Light _sunLight;
    [SerializeField] private float _maxSunLightIntensity;
    [SerializeField] private Light _moonLight;
    [SerializeField] private float _maxMoonLightIntensity;
    [SerializeField] private Color _dayAmblientLight;
    [SerializeField] private Color _nightAmblientLight;
    [SerializeField] private AnimationCurve _lightChangeCurve;
    [SerializeField] private ParticleSystem _starsParticle;

    private DateTime _currentTime;
    private TimeSpan _sunriseTime;
    private TimeSpan _sunsetTime;

    public event UnityAction<DateTime> OnTimeUpdate;
    public DateTime StartTime => _currentTime.Date + TimeSpan.FromHours(_startHour);
    public float TimeMultiplier => _timeMultiplier;

    private void Start()
    {
        _currentTime = DateTime.Now.Date + TimeSpan.FromHours(_startHour);
        _sunriseTime = TimeSpan.FromHours(_sunriseHour);
        _sunsetTime = TimeSpan.FromHours(_sunsetHour);
    }

    private void Update()
    {
        UpdateTimeDay();
        RotateSun();
        UpdateLightSettings();
    }

    private void OnEnable()
    {
        SaveGame.OnSaveGame += SaveTime;
        SaveGame.OnLoadData += LoadTime;
    }

    private void OnDisable()
    {
        SaveGame.OnSaveGame -= SaveTime;
        SaveGame.OnLoadData -= LoadTime;
    }

    public void AddTime(float time)
    {
        _currentTime = _currentTime.AddSeconds(time);
        OnTimeUpdate?.Invoke(_currentTime);
    }

    public void AddTimeInHours(float time)
    {
        _currentTime = _currentTime.AddHours(time);
        OnTimeUpdate?.Invoke(_currentTime);
    }

    private void UpdateTimeDay()
    {
        _currentTime = _currentTime.AddSeconds(Time.deltaTime * _timeMultiplier);
        OnTimeUpdate?.Invoke(_currentTime);
    }

    private void RotateSun()
    {
        float sunLightRotation;

        if (_currentTime.TimeOfDay > _sunriseTime && _currentTime.TimeOfDay < _sunsetTime)
        {
            TimeSpan sunriseSunsetDuration = CalculateTimeDifference(_sunriseTime, _sunsetTime);
            TimeSpan timeSinceSunrise = CalculateTimeDifference(_sunriseTime, _currentTime.TimeOfDay);

            double percentage = timeSinceSunrise.TotalMinutes / sunriseSunsetDuration.TotalMinutes;

            sunLightRotation = Mathf.Lerp(0, 180, (float)percentage);

            _starsParticle.Stop();
        }
        else
        {
            TimeSpan sunsetSunsetDuration = CalculateTimeDifference(_sunsetTime, _sunriseTime);
            TimeSpan timeSinceSunset = CalculateTimeDifference(_sunsetTime, _currentTime.TimeOfDay);

            double percentage = timeSinceSunset.TotalMinutes / sunsetSunsetDuration.TotalMinutes;

            sunLightRotation = Mathf.Lerp(180, 360, (float)percentage);
        }

        _sunLight.transform.rotation = Quaternion.AngleAxis(sunLightRotation, Vector3.right);

        _starsParticle.Play();
    }

    private void UpdateLightSettings()
    {
        float dotProduct = Vector3.Dot(_sunLight.transform.forward, Vector3.down);
        _sunLight.intensity = Mathf.Lerp(0, _maxSunLightIntensity, _lightChangeCurve.Evaluate(dotProduct));
        _moonLight.intensity = Mathf.Lerp(_maxMoonLightIntensity, 0, _lightChangeCurve.Evaluate(dotProduct));
        RenderSettings.ambientLight = Color.Lerp(_nightAmblientLight, _dayAmblientLight, _lightChangeCurve.Evaluate(dotProduct));
    }

    private TimeSpan CalculateTimeDifference(TimeSpan fromTime, TimeSpan toTime)
    {
        TimeSpan difference = toTime - fromTime;

        if (difference.TotalSeconds < 0)
        {
            difference += TimeSpan.FromHours(24);
        }

        return difference;
    }

    public void TimeScale()
    {
        if (_timeMultiplier == 60)
            _timeMultiplier = 6000;
        else
            _timeMultiplier = 60;
    }

    public void SaveTime()
    {
        ES3.Save("Time", _currentTime);
    }

    public void LoadTime()
    {
        _currentTime = ES3.Load<DateTime>("Time", DateTime.Now.Date + TimeSpan.FromHours(_startHour));
    }
}