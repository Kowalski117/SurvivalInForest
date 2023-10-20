using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Light))]
public class LightOn : MonoBehaviour
{
    private Light _light;
    private float _maxIntensityValue;
    private float _minIntensityValue = 0;
    private float _duration = 3;
    private void Start()
    {
        _light = GetComponent<Light>();
        _maxIntensityValue = _light.intensity;
        _light.intensity = _minIntensityValue;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerHealth>())
        {
            _light.DOIntensity(_maxIntensityValue,_duration);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerHealth>())
        {
            _light.DOIntensity(_minIntensityValue,_duration);
        }
    }
    
}
