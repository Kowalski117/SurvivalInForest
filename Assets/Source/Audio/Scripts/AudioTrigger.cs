using System.Collections;
using UnityEngine;

public class AudioTrigger : MonoBehaviour
{
    private AudioTriggerHandler _audioTriggerHandler;
    private AudioSource _source;
    private bool _isActive = true;
    private float _delay = 180f;
    private WaitForSeconds _waitForSeconds;
    private Coroutine _coroutine;

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
        _waitForSeconds =  new WaitForSeconds(_delay);

        yield return _waitForSeconds;

        _isActive = true;
    }
}
