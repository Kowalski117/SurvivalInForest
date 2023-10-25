using BehaviorDesigner.Runtime.Tasks.Unity.UnityBehaviour;
using System;
using UnityEngine;
using UnityEngine.Events;

public class TimeHandler : MonoBehaviour
{
    [SerializeField] private int _dayCounter = 0;
    [SerializeField] private float _timeMultiplier;
    [SerializeField] private float _startHour;
    [SerializeField] private float _sunriseHour;
    [SerializeField] private float _dayHour;
    [SerializeField] private float _sunsetHour;
    [SerializeField] private float _nightHour;
    [SerializeField] private Light _sunLight;
    [SerializeField] private float _maxSunLightIntensity;
    [SerializeField] private Light _moonLight;
    [SerializeField] private float _maxMoonLightIntensity;
    [SerializeField] private Color _dayAmblientLight;
    [SerializeField] private Color _nightAmblientLight;
    [SerializeField] private Color _dayColorFog;
    [SerializeField] private Color _nightColorFog;
    [SerializeField] private Material _dayMaterial;
    [SerializeField] private Material _nightMaterial;
    [SerializeField] private AnimationCurve _sunLightIntensityCurve;
    [SerializeField] private AnimationCurve _moonLightIntensityCurve;
    [SerializeField] private AnimationCurve _ambientLightCurve;
    [SerializeField] private ParticleSystem _starsParticle;
    [SerializeField] private Camera _camera;

    private DateTime _currentTime;
    private TimeSpan _sunriseTime;
    private TimeSpan _sunsetTime;
    private DateTime _lastDayUpdate = DateTime.Now;

    private bool _isEnabled = false;

    public event UnityAction<DateTime> OnTimeUpdate;
    public event UnityAction<int> OnDayUpdate;

    public DateTime StartTime => _currentTime.Date + TimeSpan.FromHours(_startHour);
    public float TimeMultiplier => _timeMultiplier;


    private void Start()
    {
        _currentTime = DateTime.Now.Date + TimeSpan.FromHours(_startHour);
        _sunriseTime = TimeSpan.FromHours(_sunriseHour);
        _sunsetTime = TimeSpan.FromHours(_sunsetHour);
        OnDayUpdate?.Invoke(_dayCounter);
    }

    private void Update()
    {
        if (_isEnabled)
        {
            UpdateTimeDay();
            RotateSunAndMoon();
            UpdateLightSettings();
        }
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

    public void ToggleEnable(bool isActive)
    {
        _isEnabled = isActive;
    }

    public void SetEnable(bool isActive)
    {
        _isEnabled = isActive;
    }

    private void UpdateTimeDay()
    {
        _currentTime = _currentTime.AddSeconds(Time.deltaTime * _timeMultiplier);
        OnTimeUpdate?.Invoke(_currentTime);

        if (_currentTime.Date > _lastDayUpdate.Date)
        {
            _lastDayUpdate = _currentTime;
            _dayCounter++;
            OnDayUpdate?.Invoke(_dayCounter);
        }
    }

    private void RotateSunAndMoon()
    {
        float sunLightRotation;
        float moonRotation;

        if (_currentTime.TimeOfDay > _sunriseTime && _currentTime.TimeOfDay < _sunsetTime)
        {
            TimeSpan sunriseSunsetDuration = CalculateTimeDifference(_sunriseTime, _sunsetTime);
            TimeSpan timeSinceSunrise = CalculateTimeDifference(_sunriseTime, _currentTime.TimeOfDay);

            double percentage = timeSinceSunrise.TotalMinutes / sunriseSunsetDuration.TotalMinutes;

            sunLightRotation = Mathf.Lerp(0, 180, (float)percentage);
            moonRotation = Mathf.Lerp(180, 360, (float)percentage);

            _starsParticle.Stop();
        }
        else
        {
            TimeSpan sunsetSunsetDuration = CalculateTimeDifference(_sunsetTime, _sunriseTime);
            TimeSpan timeSinceSunset = CalculateTimeDifference(_sunsetTime, _currentTime.TimeOfDay);

            double percentage = timeSinceSunset.TotalMinutes / sunsetSunsetDuration.TotalMinutes;

            sunLightRotation = Mathf.Lerp(180, 360, (float)percentage);
            moonRotation = Mathf.Lerp(0, 180, (float)percentage);

            _starsParticle.Play();
        }

        _sunLight.transform.rotation = Quaternion.AngleAxis(sunLightRotation, Vector3.right);
        _moonLight.transform.rotation = Quaternion.AngleAxis(moonRotation, Vector3.right);

    }

    private void UpdateLightSettings()
    {
        float sunDotProduct = Vector3.Dot(_sunLight.transform.forward, Vector3.down);
        _sunLight.intensity = Mathf.Lerp(0, _maxSunLightIntensity, _sunLightIntensityCurve.Evaluate(sunDotProduct));

        float moonDotProduct = Vector3.Dot(_moonLight.transform.forward, Vector3.down);
        _moonLight.intensity = Mathf.Lerp(0, _maxMoonLightIntensity, _moonLightIntensityCurve.Evaluate(moonDotProduct));

        RenderSettings.ambientLight = Color.Lerp(_nightAmblientLight, _dayAmblientLight, _ambientLightCurve.Evaluate(sunDotProduct));
        RenderSettings.skybox.Lerp(_dayMaterial, _nightMaterial, _ambientLightCurve.Evaluate(moonDotProduct));
        RenderSettings.fogColor = Color.Lerp(_dayColorFog, _nightColorFog, _ambientLightCurve.Evaluate(moonDotProduct));
        _camera.backgroundColor = Color.Lerp(_dayColorFog, _nightColorFog,_ambientLightCurve.Evaluate(moonDotProduct));
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

    private void SaveTime()
    {
        ES3.Save("Time", _currentTime);
    }

    private void LoadTime()
    {
        _currentTime = ES3.Load<DateTime>("Time", DateTime.Now.Date + TimeSpan.FromHours(_startHour));
    }
}

