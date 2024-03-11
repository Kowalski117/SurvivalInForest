using UnityEngine;
using System.Collections;
using IL3DN;

public class PlayerAudioHandler : MonoBehaviour
{
    private const float DamageClipDelay = 3f;

    [SerializeField] private AudioSource _audioSourceForSteps;
    [SerializeField] private AudioSource _audioSourceAllClips;

    [SerializeField] private AudioClip[] _footstepSounds = default;
    [SerializeField] private AudioClip _jumpSound = default;
    [SerializeField] private AudioClip _landSound = default;

    [SerializeField] private AudioClip[] _eatingSounds;
    [SerializeField] private AudioClip[] _drinkingSounds;

    [SerializeField] private AudioClip[] _hitInAirClips;
    [SerializeField] private AudioClip _pickUpClip;

    [SerializeField] private AudioClip[] _damageClips;

    private PlayerHealth _playerHealth;
    private AudioClip[] _footStepsOverride;
    private AudioClip _jumpSoundOverride;
    private AudioClip _landSoundOverride;
    private Coroutine _coroutine;
    private WaitForSeconds _damageClipWait = new WaitForSeconds(DamageClipDelay);

    private bool _isInSpecialSurface;
    private bool _isFootstepPlaying = false;
    private bool _isJumping = false;
    private float _longDelay = 0.5f;
    private float _shortDelay = 0.25f;
    private bool _isEnable;
    private bool _isDamageSoundPlaying = false;

    private void Awake()
    {
        _playerHealth = GetComponent<PlayerHealth>();
    }

    private void OnEnable()
    {
        _playerHealth.OnDied += TurnOffSpecialSurface;
        _playerHealth.OnDamageDone += PlayDamageClip;
    }

    private void OnDisable()
    {
        _playerHealth.OnDied -= TurnOffSpecialSurface;
        _playerHealth.OnDamageDone -= PlayDamageClip;
    }

    public void SetEnable(bool enable)
    {
        _isEnable = enable;
    }

    public void PlayOneShot(AudioClip audioClip)
    {
        _audioSourceAllClips.PlayOneShot(audioClip);
    }

    public void PlayHitInAirClip()
    {
        PlayOneShot(_hitInAirClips[Random.Range(0, _hitInAirClips.Length)]);
    }

    public void PlayDamageClip()
    {
        if (!_isDamageSoundPlaying && _isEnable)
        {
            _isDamageSoundPlaying = true;
            PlayOneShot(_damageClips[Random.Range(0, _damageClips.Length)]);
            StartCoroutine(WaitForDamageSoundToFinish());
        }
    }

    public void PlayPickUpClip()
    {
        PlayOneShot(_pickUpClip);
    }

    public void PlayEatingSound(float eatValue, float drinkValue)
    {
        drinkValue /= 2.5f;

        if (eatValue > 0 || drinkValue > 0 && _isEnable)
        {
            if (eatValue >= drinkValue)
            {
                int n = Random.Range(0, _eatingSounds.Length);
                _audioSourceForSteps.clip = _eatingSounds[n];
            }
            else if (drinkValue > eatValue)
            {
                int n = Random.Range(0, _drinkingSounds.Length);
                _audioSourceForSteps.clip = _drinkingSounds[n];
            }

            _audioSourceForSteps.PlayOneShot(_audioSourceForSteps.clip);
            _isJumping = true;
            StartCoroutine(_longDelay);
        }
    }

    public void PlayLandingSound(bool isJumping)
    {
        if (_isJumping && !isJumping && _isEnable)
        {
            if (_isInSpecialSurface)
                _audioSourceForSteps.clip = _landSoundOverride;
            else
                _audioSourceForSteps.clip = _landSound;

            _audioSourceForSteps.Play();
            _isJumping = false;
        }
    }

    public void PlayJumpSound(bool isJumping)
    {
        if (!_isJumping && isJumping && _isEnable)
        {
            if (_isInSpecialSurface)
                _audioSourceForSteps.clip = _jumpSoundOverride;
            else
                _audioSourceForSteps.clip = _jumpSound;

            _audioSourceForSteps.Play();
            _isFootstepPlaying = true;
            StartCoroutine(_longDelay);
            _isJumping = true;
        }
    }

    public void PlayFootStepSound(bool isSprint, bool isStealth)
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
                _audioSourceForSteps.clip = _footstepSounds[n];
                _audioSourceForSteps.PlayOneShot(_audioSourceForSteps.clip);
                _footstepSounds[n] = _footstepSounds[0];
                _footstepSounds[0] = _audioSourceForSteps.clip;
            }
            else
            {
                int n = Random.Range(1, _footStepsOverride.Length);

                if (n >= _footStepsOverride.Length)
                    n = 0;

                _audioSourceForSteps.clip = _footStepsOverride[n];
                _audioSourceForSteps.PlayOneShot(_audioSourceForSteps.clip);
                _footStepsOverride[n] = _footStepsOverride[0];
                _footStepsOverride[0] = _audioSourceForSteps.clip;
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

    private IEnumerator WaitForDamageSoundToFinish()
    {
        yield return _damageClipWait;

        _isDamageSoundPlaying = false;
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
        if (other.GetComponent<IL3DN_ChangeWalkingSound>())
            _isInSpecialSurface = false;
    }
}