using System.Collections;
using UnityEngine;

public class AudioTrigger : MonoBehaviour
{
    //[SerializeField] private TimeHandler _timeHandler;
    //[SerializeField] private AudioClip[] _dayClips;
    //[SerializeField] private AudioClip[] _nightClips;

    private AudioTriggerHandler _audioTriggerHandler;
    private AudioSource _source;
    //private Vector2 _day = new Vector2(4, 19);
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
            //if(_timeHandler.CurrentHurts > _day.x && _timeHandler.CurrentHurts < _day.y)
            //    _source.PlayOneShot(_dayClips[Random.Range(0, _dayClips.Length)]);
            //else
            //    _source.PlayOneShot(_nightClips[Random.Range(0, _nightClips.Length)]);
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
