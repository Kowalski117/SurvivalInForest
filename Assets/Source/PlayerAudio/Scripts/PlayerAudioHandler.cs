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

    private CharacterController _characterController;
    private AudioSource _audioSource;
    private AudioClip[] _footStepsOverride;
    private AudioClip _jumpSoundOverride;
    private AudioClip _landSoundOverride;
    private bool _isInSpecialSurface;
    private bool _isFootstepPlaying = false;
    private bool _isJumping = false;
    private Coroutine _coroutine;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlayEatingSound(float eatValue, float drinkValue)
    {
        if(eatValue > 0 || drinkValue > 0)
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
            StartCoroutine(0.5f);
        }
    }

    public void PlayLandingSound(bool isJumping)
    {
        if (_isJumping && !isJumping)
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
        if (!_isJumping && isJumping)
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
            StartCoroutine(0.5f);
            _isJumping = true;
        }
    }

    public void PlayFootStepAudio(bool isSprint, bool isStealth)
    {
        if (!_isFootstepPlaying)
        {
            _isFootstepPlaying = true;

            if (isSprint && !isStealth)
                StartCoroutine(0.25f);
            else
                StartCoroutine(0.5f);

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IL3DN_ChangeWalkingSound soundScript))
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
