using System;
using UnityEngine;
using UnityEngine.Events;

public class Fire : MonoBehaviour
{
    [SerializeField] private CraftObject _craftObject;
    [SerializeField] private ParticleSystem _fireParticle;

    private float _workingHours = 2f;
    private float _maxHours = 5f;
    private Building _building;
    private DateTime _currentTime;
    private DateTime _maxTimer;
    private TimeHandler _timeHandler;
    private bool _isFire = false;
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
        SleepPanel.OnSleepButton += ReduceTime;
        _building.OnCompletedBuild += StartFire;
    }

    private void OnDisable()
    {
        SleepPanel.OnSleepButton -= ReduceTime;
        _building.OnCompletedBuild -= StartFire;
    }

    private void Update()
    {
        if (_isFire)
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
        _currentTime = _currentTime.AddSeconds(-Time.deltaTime * time);

        OnCompletionTimeUpdate?.Invoke(_currentTime);

        if (_currentTime.TimeOfDay.TotalMilliseconds < 10000)
        {
            _fireParticle.gameObject.SetActive(false);
            _craftObject.enabled = false;
            _isFire = false;
            _collider.enabled = false;
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
        if(_currentTime.TimeOfDay.TotalMilliseconds > 10000)
        {
            _currentTime = _currentTime - TimeSpan.FromSeconds(time);
            OnCompletionTimeUpdate?.Invoke(_currentTime);
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
}
