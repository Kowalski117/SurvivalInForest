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

    public event UnityAction<DateTime> OnCompletionTimeUpdate;

    private void Awake()
    {
        _timeHandler = FindObjectOfType<TimeHandler>();
        _building = GetComponentInParent<Building>();
        _fireParticle.gameObject.SetActive(false);
        _maxTimer = _maxTimer + TimeSpan.FromHours(_maxHours);
    }

    private void OnEnable()
    {
        _building.OnCompletedBuild += StartFire;
    }

    private void OnDisable()
    {
        _building.OnCompletedBuild -= StartFire;
    }

    private void Update()
    {
        if (_isFire)
        {
            UpdateTimer();
        }
    }

    public bool AddTime(float time)
    {
        if(_currentTime < _maxTimer)
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

    private void UpdateTimer()
    {
        _currentTime = _currentTime.AddSeconds(-Time.deltaTime * _timeHandler.TimeMultiplier);

        OnCompletionTimeUpdate?.Invoke(_currentTime);

        if (_currentTime.TimeOfDay.TotalMilliseconds < 10000)
        {
            _fireParticle.gameObject.SetActive(false);
            _craftObject.enabled = false;
            _isFire = false;
        }
    }

    private void StartFire()
    {
        _currentTime = _currentTime + TimeSpan.FromHours(_workingHours);
        EnableParticle();
    }

    private void EnableParticle()
    {
        _craftObject.enabled = true;
        _fireParticle.gameObject.SetActive(true);
        _isFire = true;
    }
}
