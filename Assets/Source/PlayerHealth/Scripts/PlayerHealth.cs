using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class PlayerHealth : SurvivalAttribute, IDamagable
{
    private const float DeathRotationZ = 90f;
    private const float AnimationDuration = 1f;
    private const float DeathPositionY = 0.5f;
    private const int HealthRecoveryPercentage = 30 / 100;
    private const float RespawnDelay = 1f;

    [SerializeField] private PlayerHandler _playerInputHandler;
    [SerializeField] private Protection _protectionValue;
    [SerializeField] private PlayerPositionSave _positionSave;
    [SerializeField] private float _recoveryRate = 0.1f;
    [SerializeField] private float _recoveryDelay = 3f;
    [SerializeField] private Transform _cameraRoot;

    private float _currentDelayCounter;
    private bool _isRestoringHealth;
    private bool _canRestoreHealth = true;
    private bool _isDied = false;
    private bool _isRespawned = false;
    private bool _isGodMode = false;

    private Tween _positionTween;
    private Tween _rotateTween;
    private Coroutine _coroutine;
    private WaitForSeconds _respawnWait = new WaitForSeconds(RespawnDelay);

    public event Action<float> OnHealthChanged;
    public event Action OnDamageDone;
    public event Action OnEnemyDamageDone;
    public event Action OnRestoringHealth;
    public event Action OnDied;
    public event Action OnRevived;

    public bool IsDied => _isDied;
    public bool IsRespawned => _isRespawned;
    public float HealthPercent => CurrentValue / MaxValue;
    public float MaxHealth => MaxValue;
    public Protection Protection => _protectionValue;
    public bool IsGodMode => _isGodMode;

    private void Start()
    {
        CurrentValue = MaxValue;
        _playerInputHandler.SetActiveCollider(false);
    }

    public void Lower(float value)
    {
        if(_isGodMode || CurrentValue <= 0)
            return;

        if (CurrentValue - value > 0)
            CurrentValue -= value;
        else
        {
            CurrentValue = 0;
            Die();
        }

        OnHealthChanged?.Invoke(HealthPercent);
        OnDamageDone?.Invoke();
        _currentDelayCounter = 0;
    }

    public void LowerDamage(float value)
    {
        Lower(value * _protectionValue.GetPercent());
    }

    public void Restoring()
    {
        if (CurrentValue < MaxValue && _canRestoreHealth)
        {
            if (_currentDelayCounter < _recoveryDelay)
            {
                _currentDelayCounter += Time.deltaTime;
                _isRestoringHealth = false;
            }
            else
            {
                if (!_isRestoringHealth)
                {
                    OnRestoringHealth?.Invoke();
                    _isRestoringHealth = true;
                }

                CurrentValue += _recoveryRate * Time.deltaTime;
                CurrentValue = Mathf.Clamp(CurrentValue, 0, MaxValue);

                OnHealthChanged?.Invoke(HealthPercent);

                if (CurrentValue >= MaxValue)
                    _currentDelayCounter = 0;
            }
        }
    }

    public void Replenish(float value)
    {
        if(CurrentValue + value > MaxValue)
            CurrentValue = MaxValue;
        else
            CurrentValue += value;

        OnHealthChanged?.Invoke(HealthPercent);
    }

    public void TakeDamage(float damage, float overTimeDamage)
    {
        LowerDamage(damage);
        OnEnemyDamageDone?.Invoke();
    }

    public void Die()
    {
        _isDied = true;

        OnDied?.Invoke();

        _rotateTween = _cameraRoot.DOLocalRotate(new Vector3(_cameraRoot.localRotation.x, _cameraRoot.localRotation.y, DeathRotationZ), AnimationDuration);
        _positionTween = _cameraRoot.DOLocalMoveY(DeathPositionY,AnimationDuration);
        _playerInputHandler.FirstPersonController.enabled = false;
        _playerInputHandler.ToggleInventoryPanels(false);
        _playerInputHandler.SetActiveCollider(false);
        _playerInputHandler.ToggleAllInput(false);
        _playerInputHandler.SetCursorVisible(true);

        OnHealthChanged?.Invoke(HealthPercent);
    }

    public void Reborn()
    {
        _isRespawned = true;
        StartCoroutine();

        _rotateTween.Kill();
        _positionTween.Kill();

        OnRevived?.Invoke();

        _isDied = false;
        _positionSave.TeleportSpawnPoint();
        _playerInputHandler.SetCursorVisible(false);
        _playerInputHandler.ToggleAllInput(true);
        _playerInputHandler.FirstPersonController.enabled = true;
        _playerInputHandler.ToggleInventoryPanels(true);
        _playerInputHandler.SetActiveCollider(true);
        SetValue(MaxValue * HealthRecoveryPercentage);

        OnHealthChanged?.Invoke(HealthPercent);
    }

    public void SetCanRestoreHealth(bool value)
    {
        _canRestoreHealth = value;
    }

    public void SetGodMode(bool isActive)
    {
        _isGodMode = isActive;
    }

    private void StartCoroutine()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
            _coroutine = null;
        }

        _coroutine = StartCoroutine(DisableRespawn());
    }

    private IEnumerator DisableRespawn()
    {
        yield return _respawnWait;
        _isRespawned = false;
    }
}
