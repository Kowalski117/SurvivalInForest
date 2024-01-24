using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonFx : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private AudioClip _clickClip;

    private AudioSource _audioSource;
    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _audioSource = GetComponentInParent<AudioSource>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (_button.interactable)
        {
            _audioSource.clip = _clickClip;
            _audioSource.Play();
        }
    }
}
