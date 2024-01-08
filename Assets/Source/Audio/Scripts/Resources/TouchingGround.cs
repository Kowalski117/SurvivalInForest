using UnityEngine;

public class TouchingGround : MonoBehaviour
{
    [SerializeField] private int _groundLayer;
    [SerializeField] private AudioClip _dieAudioClips;
    [SerializeField] private AudioClip _groundClip;
    [SerializeField] private Resource _resource;

    private AudioSource _audioSource;
    private bool _isActive = true;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        _resource.Died += PlayDiedClip;
    }

    private void OnDisable()
    {
        _resource.Died -= PlayDiedClip;
    }

    private void PlayDiedClip()
    {
        _audioSource.PlayOneShot(_dieAudioClips);
        _isActive = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == _groundLayer && _isActive)
        {
            _audioSource.Stop();
            _audioSource.PlayOneShot(_groundClip);
            _isActive = false;  
        }
    }
}
