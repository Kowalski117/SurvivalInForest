using System.Collections;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PlayerHealth : SurvivalAttribute, IDamagable
{
    [SerializeField] private Interactor _interactor;
    [SerializeField] private PlayerInputHandler _playerInputHandler;
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private ProtectionValue _protectionValue;
    [SerializeField] private float _recoveryRate = 0.1f;
    [SerializeField] private float _recoveryDelay = 3f;
    [SerializeField] private Transform _cameraRoot;
    [SerializeField] private PlayerPosition _playerPositionLastScene;
    [SerializeField] private bool _isTransitionLastPosition;

    private float _currentDelayCounter;
    private bool _isRestoringHealth;
    private bool _canRestoreHealth = true;
    private bool _isDied = false;
    private bool _isRespawned = false;
    private Tween _positionTween;
    private Tween _rotateTween;

    public event UnityAction<float> OnHealthChanged;
    public event UnityAction OnDamageDone;
    public event UnityAction OnRestoringHealth;
    public event UnityAction OnDied;
    public event UnityAction OnRevived;

    public bool IsDied => _isDied;
    public bool IsRespawned => _isRespawned;
    public float HealthPercent => CurrentValue / MaxValue;
    public float MaxHealth => MaxValue;
    public ProtectionValue ProtectionValue => _protectionValue;

    private void Start()
    {
        CurrentValue = MaxValue;
        SetActiveCollider(false);
    }

    private void OnEnable()
    {
        SaveGame.OnSaveGame += Save;
        SaveGame.OnLoadData += Load;
    }

    private void OnDisable()
    {
        SaveGame.OnSaveGame -= Save;
        SaveGame.OnLoadData -= Load;
    }

    public void LowerHealth(float value)
    {
        if (CurrentValue > 0)
        {
            if (value >= 0)
            {
                CurrentValue -= value;

                OnHealthChanged?.Invoke(HealthPercent);
                OnDamageDone?.Invoke();
                if (CurrentValue <= 0)
                {
                    Die();
                    return;
                }

                _currentDelayCounter = 0;
            }
        }
    }

    public void LowerHealthDamage(float value)
    {
        LowerHealth(value - _protectionValue.Protection);
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

    public void ReplenishHealth(float value)
    {
        CurrentValue += value;

        if (CurrentValue > MaxValue)
            CurrentValue = MaxValue;

        OnHealthChanged?.Invoke(HealthPercent);
    }

    public void TakeDamage(float damage, float overTimeDamage)
    {
        LowerHealthDamage(damage);
    }

    public void Die()
    {
        OnDied?.Invoke();
        _isDied = true;
        _rotateTween = _cameraRoot.DOLocalRotate(new Vector3(_cameraRoot.localRotation.x, _cameraRoot.localRotation.y, 90), 1f);
        _positionTween = _cameraRoot.DOLocalMoveY(0.5f, 1f);
        CurrentValue = 0;
        _playerInputHandler.FirstPersonController.enabled = false;
        _playerInputHandler.ToggleInventoryPanels(false);
        SetActiveCollider(false);
        _playerInputHandler.ToggleAllInput(false);
        _playerInputHandler.SetCursorVisible(true);
        OnHealthChanged?.Invoke(HealthPercent);
    }

    public void Reborn()
    {
        _isRespawned = true;
        StartCoroutine(DisableRespawn());
        
        _rotateTween.Kill();
        _positionTween.Kill();

        OnRevived?.Invoke();

        _isDied = false;
        transform.position = _interactor.SleepPointSaveData.Position;
        transform.rotation = _interactor.SleepPointSaveData.Rotation;
        _playerInputHandler.SetCursorVisible(false);
        _playerInputHandler.ToggleAllInput(true);
        _playerInputHandler.FirstPersonController.enabled = true;
        _playerInputHandler.ToggleInventoryPanels(true);
        SetActiveCollider(true);
        SetValue(MaxValue * 30 / 100);

        OnHealthChanged?.Invoke(HealthPercent);
    }

    public void SetCanRestoreHealth(bool value)
    {
        _canRestoreHealth = value;
    }

    public void SetActiveCollider(bool isActive)
    {
        _characterController.enabled = isActive;
    }

    private void Save()
    {
        PlayerSaveData playerSaveData = new PlayerSaveData(transform.position, transform.rotation);

        ES3.Save(SaveLoadConstants.SceneIndex, SceneManager.GetActiveScene().buildIndex);
        ES3.Save(SaveLoadConstants.PlayerSaveData + SceneManager.GetActiveScene().buildIndex, playerSaveData);
    }

    private void Load()
    {
        _isRespawned = true;
        StartCoroutine(DisableRespawn());
            
        if (ES3.KeyExists(SaveLoadConstants.LastSceneIndex))
        {
            int lastSceneIndex = ES3.Load<int>(SaveLoadConstants.LastSceneIndex);
            int nextSceneIndex = ES3.Load<int>(SaveLoadConstants.NextSceneIndex);

            if (ES3.KeyExists(SaveLoadConstants.PlayerSaveData + SceneManager.GetActiveScene().buildIndex) &&
                _isTransitionLastPosition || lastSceneIndex == 0)
            {
                PlayerSaveData playerSaveData =
                    ES3.Load<PlayerSaveData>(SaveLoadConstants.PlayerSaveData + SceneManager.GetActiveScene().buildIndex);
                transform.position = playerSaveData.Position;
                transform.rotation = playerSaveData.Rotation;
                return;
            }
            else
            {
                foreach (var lastScene in _playerPositionLastScene.LastScenePosition)
                {
                    if (lastSceneIndex == lastScene.LastSceneIndex && nextSceneIndex == lastScene.NextSceneIndex)
                    {
                        transform.position = lastScene.Position;
                        transform.rotation = lastScene.Rotation;
                        return;
                    }
                }
            }
        }
    }

    IEnumerator DisableRespawn()
    {
        float secondWait = 1f;
        yield return new WaitForSeconds(secondWait);
        _isRespawned = false;
    }

    [System.Serializable]
    public struct PlayerSaveData
    {
        [SerializeField] private Vector3 _position;
        [SerializeField] private Quaternion _rotation;

        public Vector3 Position => _position;
        public Quaternion Rotation => _rotation;

        public PlayerSaveData(Vector3 position, Quaternion rotation)
        {
            _position = position;
            _rotation = rotation;
        }
    }
}
