using UnityEngine;

public class DayCycleHandler : MonoBehaviour
{
    [Range(0, 1)]
    public float TimeOfDay;

    public AnimationCurve SunCurve;
    public AnimationCurve MoonCurve;
    public AnimationCurve SkyboxCurve;

    public Material DaySkybox;
    public Material NightSkybox;

    public ParticleSystem Stars;

    public Light Sun;
    public Light Moon;

    private TimeHandler _timeHandler;
    private float _sunIntensity;
    private float _moonIntensity;

    //private void Start()
    //{
    //    _timeHandler = FindObjectOfType<TimeHandler>();
    //    _sunIntensity = Sun.intensity;
    //    _moonIntensity = Moon.intensity;
    //}

    //private void OnEnable()
    //{
    //    _timeHandler.OnTimeOfDayUpdate += UpdateTimeOfDay;
    //}

    //private void OnDisable()
    //{
    //    _timeHandler.OnTimeOfDayUpdate -= UpdateTimeOfDay;

    //}

    //private void UpdateTimeOfDay(float timeOfDay)
    //{
    //    TimeOfDay = timeOfDay /*/ (24 * 60 * 60)*/;

    //    // Настройки освещения (skybox и основное солнце)
    //    RenderSettings.skybox.Lerp(NightSkybox, DaySkybox, SkyboxCurve.Evaluate(TimeOfDay));
    //    RenderSettings.sun = SkyboxCurve.Evaluate(TimeOfDay) > 0.1f ? Sun : Moon;
    //    DynamicGI.UpdateEnvironment();

    //    // Прозрачность звёзд
    //    var mainModule = Stars.main;
    //    //mainModule.startColor = new Color(1, 1, 1, 1 - SkyboxCurve.Evaluate(TimeOfDay));

    //    // Поворот луны и солнца
    //    Sun.transform.localRotation = Quaternion.Euler(TimeOfDay * 360f, 180, 0);
    //    Moon.transform.localRotation = Quaternion.Euler(TimeOfDay * 360f + 180f, 180, 0);

    //    // Интенсивность свечения луны и солнца
    //    Sun.intensity = _sunIntensity * SunCurve.Evaluate(TimeOfDay);
    //    Moon.intensity = _moonIntensity * MoonCurve.Evaluate(TimeOfDay);
    //}

    //private float CalculateTimeOfDay()
    //{
    //    float startTime = (float)_timeHandler.StartTime.TimeOfDay.TotalSeconds;
    //    float currentTime = (float)_timeHandler.С.TimeOfDay.TotalSeconds;
    //    float timePassed = currentTime - startTime;
    //    float timeOfDay = timePassed / (_timeHandler.TimeMultiplier * 60f * 60f);

    //    if (timeOfDay >= 1f)
    //        timeOfDay -= 1f;

    //    return timeOfDay;
    //}

}
