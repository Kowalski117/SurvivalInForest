using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonFx : MonoBehaviour, IPointerDownHandler
{
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
            _audioSource.Play();
    }
}
