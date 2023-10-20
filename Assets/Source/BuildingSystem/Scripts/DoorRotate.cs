using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class DoorRotate : MonoBehaviour
{
    [SerializeField] private Transform _doorRotate;
    [SerializeField] private float _offsetY;

    private float _delay = 0.75f;

    public event UnityAction OnOpenDoor;
    public event UnityAction<float> OnCloseDoor;

    public void RotateDoor(float offsetY)
    {
        _doorRotate.DOLocalRotate(new Vector3(_doorRotate.rotation.x, offsetY, _doorRotate.rotation.y), _delay);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<PlayerHealth>() != null)
        {
            RotateDoor(_offsetY);
            OnOpenDoor?.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerHealth>() != null)
        {
            RotateDoor(0);
            OnCloseDoor?.Invoke(_delay);
        }
    }
}
