using Cinemachine;
using System.Collections;
using UnityEngine;

public class CameraShakerEffect : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;
    [SerializeField] private float _amplitudeGain = 1f;
    [SerializeField] private float _frequencyGain = 1f;
    [SerializeField] private float _delay = 0.5f;

    private float _defaultAmplitudeGain;
    private float _defaultFrequencyGain;
    private CinemachineBasicMultiChannelPerlin _multiChannelPerlin;
    private Coroutine _coroutine;

    private void Awake()
    {
        _multiChannelPerlin = _virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        _defaultAmplitudeGain = _multiChannelPerlin.m_AmplitudeGain;
        _defaultFrequencyGain = _multiChannelPerlin.m_FrequencyGain;
    }

    public void StartShake()
    {
        if(_coroutine != null)
        {
            StopCoroutine(_coroutine);
            _coroutine = null;
        }

        _coroutine = StartCoroutine(Shake(_delay));
    }

    private IEnumerator Shake(float duration)
    {
        _multiChannelPerlin.m_AmplitudeGain = _amplitudeGain;
        _multiChannelPerlin.m_FrequencyGain = _frequencyGain;

        yield return new WaitForSeconds(duration);

        _multiChannelPerlin.m_AmplitudeGain = _defaultAmplitudeGain;
        _multiChannelPerlin.m_FrequencyGain = _defaultFrequencyGain;
    }
}
