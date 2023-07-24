using PixelCrushers;
using System;
using UnityEngine;
using UnityEngine.Events;

public class Fire : MonoBehaviour
{
    [SerializeField] private CraftObject _craftObject;
    [SerializeField] private GameObject _fireParticle;
    [SerializeField] private bool _isRemoveAfterFire = false;

    private float _workingHours = 2f;
    private float _maxHours = 5f;
    private Building _building;
    private DateTime _currentTime;
    private DateTime _maxTimer;
    private TimeHandler _timeHandler;
    private bool _isFire = false;
    private bool _isEnable = true;
    private SphereCollider _collider;

    public event UnityAction<DateTime> OnCompletionTimeUpdate;

    private void Awake()
    {
        _collider = GetComponent<SphereCollider>();
        _timeHandler = FindObjectOfType<TimeHandler>();
        _building = GetComponentInParent<Building>();
        _fireParticle.gameObject.SetActive(false);
        _maxTimer = _maxTimer + TimeSpan.FromHours(_maxHours);
    }

    private void OnEnable()
    {
        SleepPanel.OnStoppedTime += ToggleEnable;
        SleepPanel.OnSubtractTime += ReduceTime;
        _building.OnCompletedBuild += StartFire;
    }

    private void OnDisable()
    {
        SleepPanel.OnStoppedTime -= ToggleEnable;
        SleepPanel.OnSubtractTime -= ReduceTime;
        _building.OnCompletedBuild -= StartFire;
    }

    private void Update()
    {
        if (_isFire && _isEnable)
        {
            UpdateTimer(_timeHandler.TimeMultiplier);
        }
    }

    public bool AddFire(InventorySlot slot)
    {
        if (slot.ItemData != null && slot.ItemData.GorenjeTime > 0 && slot.Size > 0)
        {
            if (AddTime(slot.ItemData.GorenjeTime))
            {
                return true;
            }
        }

        return false;
    }

    private void UpdateTimer(float time)
    {
        if(_currentTime.TimeOfDay.TotalSeconds >= time)
        {
            _currentTime = _currentTime.AddSeconds(-Time.deltaTime * time);
            OnCompletionTimeUpdate?.Invoke(_currentTime);
        }
        else
        {
            _currentTime = DateTime.MinValue;
            OnCompletionTimeUpdate?.Invoke(_currentTime);
        }


        if (_currentTime == DateTime.MinValue)
        {
            _fireParticle.gameObject.SetActive(false);
            _craftObject.TurnOff();
            _isFire = false;

            if(_isRemoveAfterFire)
                Destroy(gameObject);
        }
    }

    private void StartFire()
    {
        _currentTime = _currentTime + TimeSpan.FromHours(_workingHours);
        EnableParticle();
    }

    private void EnableParticle()
    {
        _collider.enabled = true;
        _craftObject.enabled = true;
        _fireParticle.gameObject.SetActive(true);
        _isFire = true;
    }

    private void ReduceTime(float time)
    {
        if (_currentTime.TimeOfDay.Hours >= time)
        {
            _currentTime = _currentTime.AddHours(-time);
            OnCompletionTimeUpdate?.Invoke(_currentTime);
            return;
        }
        else
        {
            _currentTime = DateTime.MinValue;
            OnCompletionTimeUpdate?.Invoke(_currentTime);
            return;
        }
    }

    private bool AddTime(float time)
    {
        if (_currentTime < _maxTimer)
        {
            _currentTime = _currentTime.AddHours(time);
            OnCompletionTimeUpdate?.Invoke(_currentTime);

            if (!_isFire)
            {
                EnableParticle();
            }

            return true;
        }
        return false;
    }

    private void ToggleEnable(bool value)
    {
        _isEnable = value;
    }
}
