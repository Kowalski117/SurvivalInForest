using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class DoorAudioHandler: MonoBehaviour
{
    [SerializeField] private AudioClip _openDoor;
    [SerializeField] private AudioClip _closeDoor;

    private AudioSource _audioSource;
    private DoorRotate _doorRotate;
    private Coroutine _coroutine;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _doorRotate = GetComponentInChildren<DoorRotate>();
    }

    private void OnEnable()
    {
        _doorRotate.OnOpened += PlayOpenDoorSound;
        _doorRotate.OnClosed += PlayCloseDoorSound;
    }

    private void OnDisable()
    {
        _doorRotate.OnOpened += PlayOpenDoorSound;
        _doorRotate.OnClosed -= PlayCloseDoorSound;
    }

    private void PlayOpenDoorSound()
    {
        StartCoroutine(0, _openDoor);
    }

    private void PlayCloseDoorSound(float duration)
    {
        StartCoroutine(duration / 2, _closeDoor);
    }

    private void StartCoroutine(float duration, AudioClip audioClip)
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
            _coroutine = null;
        }

        _coroutine = StartCoroutine(WaitForSoundToFinish(duration, audioClip));
    }

    private IEnumerator WaitForSoundToFinish(float duration, AudioClip audioClip)
    {
        yield return new WaitForSeconds(duration);
        _audioSource.PlayOneShot(audioClip);
    }
}
