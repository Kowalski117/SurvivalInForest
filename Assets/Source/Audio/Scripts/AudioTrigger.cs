using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioTrigger : MonoBehaviour
{
    private const float Delay = 180f; 

    private AudioTriggerHandler _audioTriggerHandler;
    private AudioSource _source;
    private WaitForSeconds _delayWait = new WaitForSeconds(Delay);
    private Coroutine _coroutine;

    private bool _isActive = true;

    private void Awake()
    {
        _audioTriggerHandler = GetComponentInParent<AudioTriggerHandler>();
        _source = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerHealth>() && _isActive)
        {
             _source.PlayOneShot(_audioTriggerHandler.GetAudioClip());
            StartCoroutine();
        }
    }

    private void StartCoroutine()
    {
        if(_coroutine != null)
        {
            StopCoroutine(_coroutine);
            _coroutine = null;
        }

        _coroutine = StartCoroutine(WaitUntilRestarting());
    }

    private IEnumerator WaitUntilRestarting()
    {
        _isActive = false;

        yield return _delayWait;

        _isActive = true;
    }
}
