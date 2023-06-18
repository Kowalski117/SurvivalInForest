using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(AudioSource))]
public class AnimalsSound : MonoBehaviour
{
 [SerializeField] private AudioClip[] _steps;

 private AudioSource _audioSource;
 private void Start()
 {
     _audioSource = GetComponent<AudioSource>();
 }

 public void StepPlay()
 {
     _audioSource.pitch = Random.Range(0.9f, 1.1f);
     _audioSource.PlayOneShot(_steps[Random.Range(0,_steps.Length)]);
 }
}
