using DG.Tweening;
using System;
using UnityEngine;

public class DoorRotate : MonoBehaviour
{
    [SerializeField] private Transform _doorTransform;
    [SerializeField] private float _offsetY;

    private float _delay = 0.75f;

    public event Action OnOpened;
    public event Action<float> OnClosed;

    public void Rotate(float offsetY)
    {
        _doorTransform.DOLocalRotate(new Vector3(_doorTransform.rotation.x, offsetY, _doorTransform.rotation.y), _delay);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<PlayerHealth>() != null)
        {
            Rotate(_offsetY);
            OnOpened?.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerHealth>() != null)
        {
            Rotate(0);
            OnClosed?.Invoke(_delay);
        }
    }
}
