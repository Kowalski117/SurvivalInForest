using UnityEngine;
using System.Collections;
using IL3DN;

public class PlayerAudioHandler : MonoBehaviour
{
    [SerializeField] private AudioClip[] _footstepSounds = default;
    [SerializeField] private AudioClip _jumpSound = default;
    [SerializeField] private AudioClip _landSound = default;

    [SerializeField] private AudioClip[] _eatingSounds;
    [SerializeField] private AudioClip[] _drinkingSounds;

    [SerializeField] private AudioClip[] _hitInAirClips;
    [SerializeField] private AudioClip _pickUpClip;

    private PlayerHealth _playerHealth;
    private AudioSource _audioSource;
    private AudioClip[] _footStepsOverride;
    private AudioClip _jumpSoundOverride;
    private AudioClip _landSoundOverride;
    private bool _isInSpecialSurface;
    private bool _isFootstepPlaying = false;
    private bool _isJumping = false;
    private Coroutine _coroutine;
    private float _longDelay = 0.5f;
    private float _shortDelay = 0.25f;
    private bool _isEnable;

    public AudioClip[] HitInAirClips => _hitInAirClips;
    public AudioClip PickUpClip => _pickUpClip;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _playerHealth = GetComponent<PlayerHealth>();
    }

    private void OnEnable()
    {
        _playerHealth.OnDied += TurnOffSpecialSurface;
    }

    private void OnDisable()
    {
        _playerHealth.OnDied -= TurnOffSpecialSurface;
    }

    public void SetEnable(bool enable)
    {
        _isEnable = enable;
    }

    public void PlayOneShot(AudioClip audioClip)
    {
        _audioSource.PlayOneShot(audioClip);
    }

    public void PlayHitInAirClip()
    {
        _audioSource.PlayOneShot(_hitInAirClips[Random.Range(0, _hitInAirClips.Length)]);
    }

    public void PlayItemPickUpClip()
    {
        _audioSource.PlayOneShot(_pickUpClip);
    }

    public void PlayEatingSound(float eatValue, float drinkValue)
    {
        if(eatValue > 0 || drinkValue > 0 && _isEnable)
        {
            if (eatValue >= drinkValue)
            {
                int n = Random.Range(0, _eatingSounds.Length);
                _audioSource.clip = _eatingSounds[n];
            }
            else if (drinkValue > eatValue)
            {
                int n = Random.Range(0, _drinkingSounds.Length);
                _audioSource.clip = _drinkingSounds[n];
            }
            _audioSource.PlayOneShot(_audioSource.clip);
            _isJumping = true;
            StartCoroutine(_longDelay);
        }
    }

    public void PlayLandingSound(bool isJumping)
    {
        if (_isJumping && !isJumping && _isEnable)
        {
            if (_isInSpecialSurface)
            {
                _audioSource.clip = _landSoundOverride;
            }
            else
            {
                _audioSource.clip = _landSound;
            }
            _audioSource.Play();
            _isJumping = false;
        }
    }

    public void PlayJumpSound(bool isJumping)
    {
        if (!_isJumping && isJumping && _isEnable)
        {
            if (_isInSpecialSurface)
            {
                _audioSource.clip = _jumpSoundOverride;
            }
            else
            {
                _audioSource.clip = _jumpSound;
            }
            _audioSource.Play();
            _isFootstepPlaying = true;
            StartCoroutine(_longDelay);
            _isJumping = true;
        }
    }

    public void PlayFootStepAudio(bool isSprint, bool isStealth)
    {
        if (!_isFootstepPlaying && _isEnable)
        {
            _isFootstepPlaying = true;

            if (isSprint && !isStealth)
                StartCoroutine(_shortDelay);
            else
                StartCoroutine(_longDelay);

            if (!_isInSpecialSurface)
            {
                int n = Random.Range(1, _footstepSounds.Length);
                _audioSource.clip = _footstepSounds[n];
                _audioSource.PlayOneShot(_audioSource.clip);
                _footstepSounds[n] = _footstepSounds[0];
                _footstepSounds[0] = _audioSource.clip;
            }
            else
            {
                int n = Random.Range(1, _footStepsOverride.Length);
                if (n >= _footStepsOverride.Length)
                {
                    n = 0;
                }
                _audioSource.clip = _footStepsOverride[n];
                _audioSource.PlayOneShot(_audioSource.clip);
                _footStepsOverride[n] = _footStepsOverride[0];
                _footStepsOverride[0] = _audioSource.clip;
            }
        }
    }

    private void StartCoroutine(float duration)
    {
        if(_coroutine != null)
        {
            StopCoroutine(_coroutine);
            _coroutine = null;
        }

        _coroutine = StartCoroutine(WaitForSoundToFinish(duration));
    }

    private IEnumerator WaitForSoundToFinish(float duration)
    {
        yield return new WaitForSeconds(duration);

        _isFootstepPlaying = false;
        _isJumping = false;
    }

    private void TurnOffSpecialSurface()
    {
        _isInSpecialSurface = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out IL3DN_ChangeWalkingSound soundScript) && !_isInSpecialSurface)
        {
            if (soundScript != null)
            {
                _footStepsOverride = soundScript.FootStepsOverride;
                _jumpSoundOverride = soundScript.JumpSound;
                _landSoundOverride = soundScript.LandSound;
                _isInSpecialSurface = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out IL3DN_ChangeWalkingSound soundScript))
        {
            _isInSpecialSurface = false;
        }
    }
}
