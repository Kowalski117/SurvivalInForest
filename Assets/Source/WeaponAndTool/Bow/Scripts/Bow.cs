using System;
using UnityEngine;

public class Bow : MonoBehaviour
{
    [SerializeField] private Transform _shootPoint;
    [SerializeField] private Arrow[] _arrows;
    [SerializeField] private PlayerHandler _inputHandler;
    [SerializeField] private PlayerEquipmentHandler _playerEquipmentHandler;
    [SerializeField] private TargetInteractionHandler _targetInteractionHandler;
    [SerializeField] private PlayerAnimatorHandler _playerAnimatorHandler;
    [SerializeField] private InteractorConstruction  _interactor;
    [SerializeField] private float _arrowSpeed = 10f;

    private WeaponItemData _currentWeapon;
    private bool _isShoot = false;
    private bool _isEnable = false;
    private bool _isAim = false;

    public event Action OnInitialized;
    public event Action OnCleared;
    public event Action OnShooting;

    private void Start()
    {
        foreach (var arrow in _arrows)
        {
            arrow.gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        _playerEquipmentHandler.OnUpdateWeaponItemData += Init;

        _inputHandler.InteractionPlayerInput.OnUsed += Shoot;
        _inputHandler.InteractionPlayerInput.OnAimed += Aim;

        foreach (var arrow in _arrows)
        {
            arrow.OnColliderEntered += InstantiateArrow;
        }
    }

    private void OnDisable()
    {
        _playerEquipmentHandler.OnUpdateWeaponItemData -= Init;

        _inputHandler.InteractionPlayerInput.OnUsed -= Shoot;
        _inputHandler.InteractionPlayerInput.OnAimed -= Aim;

        foreach (var arrow in _arrows)
        {
            arrow.OnColliderEntered -= InstantiateArrow;
        }
    }

    private void Aim()
    {
        if (_isEnable)
        {
            if (_isAim)
                _isAim = false;
            else
                _isAim = true;

            _playerAnimatorHandler.Aim(_isAim);
        }
    }

    private void Shoot()
    {
        if (_isAim && _playerEquipmentHandler.IsShoot())
        {
            foreach (var arrow in _arrows)
            {
                if (arrow.IsFlying == false)
                {
                    _isShoot = false;
                    break;
                }
            }

            if (!_isShoot)
            {
                foreach (var arrow in _arrows)
                {
                    if(arrow.IsFlying == false)
                    {
                        arrow.gameObject.SetActive(true);
                        _playerAnimatorHandler.Hit(true);
                        arrow.Shoot(_arrowSpeed);
                        OnShooting?.Invoke();
                        break;
                    }
                }
            }
        }
    }

    private void InstantiateArrow(Arrow arrow, Transform parent, Animals animals)
    {
        if(UnityEngine.Random.Range(0, 2) == 0)
        {
            ItemPickUp arrowItem = Instantiate(_currentWeapon.Bullet, arrow.transform.position, arrow.transform.rotation);
            arrowItem.GenerateNewID();
            arrowItem.gameObject.transform.parent = parent;
            arrowItem.Rigidbody.isKinematic = true;
        }

        arrow.SetRope(transform);
        arrow.gameObject.SetActive(false);
        _isShoot = false;

        if (animals != null)
            _targetInteractionHandler.TakeDamageAnimal(animals, _currentWeapon.Damage, _currentWeapon.OverTimeDamage);
    }

    private void Init(WeaponItemData itemData)
    {
        if (itemData != null && itemData.WeaponType == WeaponType.RangedWeapon)
        {
            _currentWeapon = itemData;
            _isEnable = true;
            OnInitialized?.Invoke();
        }
        else
        {
            _currentWeapon = null;
            _isAim = false;
            _isEnable = false;
            OnCleared?.Invoke();
        }
    }
}
