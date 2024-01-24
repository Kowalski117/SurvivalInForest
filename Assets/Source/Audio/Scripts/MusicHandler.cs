using UnityEngine;

public class MusicHandler : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip[] _musicClips;

    private int _previousTrackIndex = -1;

    private void Start()
    {
        if (_audioSource != null && _musicClips != null && _musicClips.Length > 0)
            PlayRandomTrack();
    }

    private void PlayRandomTrack()
    {
        int randomIndex;
        do
        {
            randomIndex = Random.Range(0, _musicClips.Length);
        } while (randomIndex == _previousTrackIndex);

        _audioSource.clip = _musicClips[randomIndex];
        _audioSource.Play();
        _previousTrackIndex = randomIndex;
    }

    private void Update()
    {
        if (!_audioSource.isPlaying)
        {
            PlayRandomTrack();
        }
    }
}
