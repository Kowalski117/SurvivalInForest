using IL3DN;
using System.Collections;
using UnityEngine;

public class PlayerAudioHandler : MonoBehaviour
{
    [SerializeField] private AudioClip[] _footstepSounds = default;   
    [SerializeField] private AudioClip _jumpSound = default;          
    [SerializeField] private AudioClip _landSound = default;

    private CharacterController _characterController;
    private AudioSource _audioSource;
    private AudioClip[] _footStepsOverride;
    private AudioClip _jumpSoundOverride;
    private AudioClip _landSoundOverride;
    private bool _isInSpecialSurface;
    private bool _isFootstepPlaying = false;
    private bool _isJumping = false;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _audioSource = GetComponent<AudioSource>();
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
            _isJumping = true;
        }
    }

    public void PlayFootStepAudio(bool isSprint)
    {
        if (!_isFootstepPlaying)
        {
            _isFootstepPlaying = true;

            if (isSprint)
                StartCoroutine(WaitForSoundToFinish(0.25f));
            else
                StartCoroutine(WaitForSoundToFinish(0.5f));

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

    private IEnumerator WaitForSoundToFinish(float duration)
    {
        yield return new WaitForSeconds(duration);

        _isFootstepPlaying = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out IL3DN_ChangeWalkingSound soundScript))
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
