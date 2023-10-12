using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class DoorRotate : MonoBehaviour
{
    [SerializeField] private Transform _doorRotate;
    [SerializeField] private float _offsetY;

    private float _delay = 2f;

    public event UnityAction<DoorRotate> OnTrigerEnter;
    public event UnityAction<DoorRotate> OnTrigerExit;

    public void RotateDoor(float offsetY)
    {
        //_tweenRotate.Kill();
        _doorRotate.DOLocalRotate(new Vector3(_doorRotate.rotation.x, offsetY, _doorRotate.rotation.y), _delay);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<PlayerHealth>() != null)
        {
            RotateDoor(_offsetY);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerHealth>() != null)
        {
            RotateDoor(0);
        }
    }
}
