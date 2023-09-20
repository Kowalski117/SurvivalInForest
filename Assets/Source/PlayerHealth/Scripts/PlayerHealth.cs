using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : SurvivalAttribute, IDamagable
{
    [SerializeField] private Interactor _interactor;
    [SerializeField] private PlayerInputHandler _playerInputHandler;
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private ProtectionValue _protectionValue;
    [SerializeField] private float _recoveryRate = 0.1f;
    [SerializeField] private float _recoveryDelay = 3f;
    [SerializeField] private Transform _cameraRoot;

    private float _currentDelayCounter;
    private bool _isRestoringHealth;
    private bool _canRestoreHealth = true;
    private float _defoultPositionY;

    public event UnityAction<float> OnHealthChanged;
    public event UnityAction OnDamageDone;
    public event UnityAction OnRestoringHealth;
    public event UnityAction OnDied;
    public event UnityAction OnRevived;

    public float HealthPercent => CurrentValue / MaxValue;

    private void Start()
    {
        CurrentValue = MaxValue;
        _defoultPositionY = _cameraRoot.transform.localPosition.y;
    }

    public void LowerHealth(float value)
    {
        if(CurrentValue > 0)
        {
            CurrentValue -= value - (value / 100 * _protectionValue.Protection);

            OnDamageDone?.Invoke();
            OnHealthChanged?.Invoke(HealthPercent);

            if (CurrentValue <= 0)
            {
                Die();
                return;
            }

            _currentDelayCounter = 0;
        }
    }

    public void RestoringHealth()
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
                CurrentValue = Mathf.Clamp(CurrentValue, 0f, MaxValue);

                OnHealthChanged?.Invoke(HealthPercent);

                if (CurrentValue >= MaxValue)
                    _currentDelayCounter = 0;
            }
        }
    }

    public void TakeDamage(float damage,float overTimeDamage)
    {
        LowerHealth(damage);
    }

    public void Die()
    {
        _cameraRoot.DOLocalRotate(new Vector3(_cameraRoot.localRotation.x, _cameraRoot.localRotation.y, 90), 1f);
        _cameraRoot.DOLocalMoveY(0.5f, 1f);
        CurrentValue = 0;
        _playerInputHandler.SetCursorVisible(true);
        _playerInputHandler.FirstPersonController.enabled = false;
        _playerInputHandler.ToggleInventoryPanels(false);
        _characterController.enabled = false;
        _playerInputHandler.ToggleAllInput(false);
        OnDied?.Invoke();
        OnHealthChanged?.Invoke(HealthPercent);
    }

    public void Reborn()
    {
        OnRevived?.Invoke();

        //_cameraRoot.localPosition = new Vector3(_cameraRoot.localPosition.x, _defoultPositionY, _cameraRoot.localPosition.z);
        //_cameraRoot.localRotation = Quaternion.identity;
        transform.position = _interactor.SleepPointSaveData.Position;
        transform.rotation = _interactor.SleepPointSaveData.Rotation;
        _playerInputHandler.SetCursorVisible(false);
        _playerInputHandler.ToggleAllInput(true);
        _playerInputHandler.FirstPersonController.enabled = true;
        _playerInputHandler.ToggleInventoryPanels(true);
        _characterController.enabled = true;
        SetValue(MaxValue * 30 / 100);
        OnHealthChanged?.Invoke(HealthPercent);
        //_playerInputHandler.FirstPersonController.ResetPosition();
    }

    public void SetCanRestoreHealth(bool value)
    {
        _canRestoreHealth = value;
    }
}
