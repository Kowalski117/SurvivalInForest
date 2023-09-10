using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Bell : MonoBehaviour
{
    private AudioSource _audioSource;
    private Collider _collider;
    private bool _audioPlaying;

    private void Start()
    {
        _collider = GetComponent<Collider>();
        _audioSource = GetComponent<AudioSource>();
        _audioPlaying = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_audioPlaying == false)
        {
            if (other.gameObject.GetComponent<PlayerHealth>() != null)
            {
                _audioSource.Play();
                _audioPlaying = true; 
            }
        }
    }

}
