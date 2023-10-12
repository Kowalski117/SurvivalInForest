using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Transform _doorRotate;
    [SerializeField] private DoorRotate[] _triggers;

    private bool _isOpenDoor = false;
    private float _delay = 2f;
    private Tween _tweenRotate;

    private void OnEnable()
    {
        foreach (var trigger in _triggers) 
        {
            trigger.OnTrigerEnter += OpenDoor;
            trigger.OnTrigerExit += CloseDoor;
        }
    }

    private void OnDisable()
    {
        foreach (var trigger in _triggers)
        {
            trigger.OnTrigerEnter -= OpenDoor;
            trigger.OnTrigerExit -= CloseDoor;
        }
    }

    public void RotateDoor(float offsetY)
    {
        //_tweenRotate.Kill();
        _tweenRotate = _doorRotate.DORotate(new Vector3(_doorRotate.rotation.x, offsetY, _doorRotate.rotation.y), _delay);
    }

    private void OpenDoor(DoorRotate doorRotate)
    {
        if(!_isOpenDoor) 
        {
            Debug.Log("Open");
            foreach(var trigger in _triggers)
            {
                if(trigger != doorRotate)
                    trigger.enabled = false;
            }

            _isOpenDoor = true;
            //RotateDoor(doorRotate.OffSetY);
        }
    }

    private void CloseDoor(DoorRotate doorRotate)
    {
        if (_isOpenDoor)
        {
            Debug.Log("Close");
            _isOpenDoor = false;
            RotateDoor(0);

            foreach (var trigger in _triggers)
            {
                if (trigger != doorRotate)
                    trigger.enabled = true;
            }
        }
    }
}
