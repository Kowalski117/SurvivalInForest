using System;
using UnityEngine;

[RequireComponent(typeof(MainClock))]
public class LightSource : MonoBehaviour
{
    [SerializeField] private Light _sunLight;
    [SerializeField] private float _maxSunLightIntensity;
    [SerializeField] private Light _moonLight;
    [SerializeField] private float _maxMoonLightIntensity;

    [SerializeField] private float _sunriseHour;
    [SerializeField] private float _sunsetHour;

    [SerializeField] private Color _dayAmblientLight;
    [SerializeField] private Color _nightAmblientLight;
    [SerializeField] private Color _dayColorFog;
    [SerializeField] private Color _nightColorFog;
    [SerializeField] private Vector2 _fogDensity;
    [SerializeField] private AnimationCurve _sunLightIntensityCurve;
    [SerializeField] private AnimationCurve _moonLightIntensityCurve;
    [SerializeField] private AnimationCurve _ambientLightCurve;

    [SerializeField] private Camera _camera;

    private MainClock _mainClock;
    private TimeSpan _sunriseTime;
    private TimeSpan _sunsetTime;
    private Vector2 _upperHemisphere = new Vector2(0, 180);
    private Vector2 _lowerHemisphere = new Vector2(180, 360);

    private float _sunRotation;
    private float _moonRotation;

    private void Awake()
    {
        _mainClock = GetComponent<MainClock>();
        _sunriseTime = TimeSpan.FromHours(_sunriseHour);
        _sunsetTime = TimeSpan.FromHours(_sunsetHour);
    }

    private void Update()
    {
        if (_mainClock.IsEnable)
        {
            Rotate();
            UpdateSettings();
        }
    }

    private void Rotate()
    {
        if (_mainClock.CurrentTime.TimeOfDay > _sunriseTime && _mainClock.CurrentTime.TimeOfDay < _sunsetTime)
            CalculateRotations(_sunriseTime, _sunsetTime, _upperHemisphere, _lowerHemisphere);
        else
            CalculateRotations(_sunsetTime, _sunriseTime, _lowerHemisphere, _upperHemisphere);

        _sunLight.transform.rotation = Quaternion.AngleAxis(_sunRotation, Vector3.right);
        _moonLight.transform.rotation = Quaternion.AngleAxis(_moonRotation, Vector3.right);
    }

    private void UpdateSettings()
    {
        float sunDotProduct = Vector3.Dot(_sunLight.transform.forward, Vector3.down);
        _sunLight.intensity = Mathf.Lerp(0, _maxSunLightIntensity, _sunLightIntensityCurve.Evaluate(sunDotProduct));

        float moonDotProduct = Vector3.Dot(_moonLight.transform.forward, Vector3.down);
        _moonLight.intensity = Mathf.Lerp(0, _maxMoonLightIntensity, _moonLightIntensityCurve.Evaluate(moonDotProduct));

        RenderSettings.ambientLight = Color.Lerp(_nightAmblientLight, _dayAmblientLight, _ambientLightCurve.Evaluate(sunDotProduct));
        RenderSettings.fogColor = Color.Lerp(_dayColorFog, _nightColorFog, _ambientLightCurve.Evaluate(moonDotProduct));
        RenderSettings.fogDensity = Mathf.Lerp(_fogDensity.x, _fogDensity.y, _ambientLightCurve.Evaluate(moonDotProduct));

        _camera.backgroundColor = Color.Lerp(_dayColorFog, _nightColorFog, _ambientLightCurve.Evaluate(moonDotProduct));
    }

    private void CalculateRotations(TimeSpan fromTime, TimeSpan toTime, Vector2 hemisphereSun, Vector2 hemisphereMoon)
    {
        TimeSpan sunsetSunsetDuration = CalculateTimeDifference(fromTime, toTime);
        TimeSpan timeSinceSunset = CalculateTimeDifference(fromTime, _mainClock.CurrentTime.TimeOfDay);

        double percentage = timeSinceSunset.TotalMinutes / sunsetSunsetDuration.TotalMinutes;

        _sunRotation = Mathf.Lerp(hemisphereSun.x, hemisphereSun.y, (float)percentage);
        _moonRotation = Mathf.Lerp(hemisphereMoon.x, hemisphereMoon.y, (float)percentage);
    }

    private TimeSpan CalculateTimeDifference(TimeSpan fromTime, TimeSpan toTime)
    {
        TimeSpan difference = toTime - fromTime;

        if (difference.TotalSeconds < 0)
            difference += TimeSpan.FromHours(24);

        return difference;
    }
}
