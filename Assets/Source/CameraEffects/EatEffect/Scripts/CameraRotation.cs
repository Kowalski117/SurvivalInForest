using Cinemachine;
using System.Collections;
using UnityEngine;

public class CameraRotation : FoodEffect
{
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;
    [SerializeField] private float _amplitudeGain = 5f;
    [SerializeField] private float _frequencyGain = 5f;

    private float _defaultAmplitudeGain;
    private float _defaultFrequencyGain;
    private CinemachineBasicMultiChannelPerlin _multiChannelPerlin;

    private void Awake()
    {
        _multiChannelPerlin = _virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        _defaultAmplitudeGain = _multiChannelPerlin.m_AmplitudeGain;
        _defaultFrequencyGain = _multiChannelPerlin.m_FrequencyGain;
    }

    protected override IEnumerator RotateColors(float duration)
    {
        _multiChannelPerlin.m_AmplitudeGain = _amplitudeGain;
        _multiChannelPerlin.m_FrequencyGain= _frequencyGain;

        yield return new WaitForSeconds(duration);

        _multiChannelPerlin.m_AmplitudeGain = _defaultAmplitudeGain;
        _multiChannelPerlin.m_FrequencyGain = _defaultFrequencyGain;
    }
}
