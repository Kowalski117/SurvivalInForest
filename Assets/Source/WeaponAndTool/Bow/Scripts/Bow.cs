using UnityEngine;
using UnityEngine.Events;

public class Bow : MonoBehaviour
{
    [SerializeField] private Transform _shootPoint;
    [SerializeField] private Arrow[] _arrows;
    [SerializeField] private PlayerHandler _inputHandler;
    [SerializeField] private PlayerInteraction _playerInteraction;
    [SerializeField] private PlayerAnimatorHandler _playerAnimatorHandler;
    [SerializeField] private Interactor  _interactor;
    [SerializeField] private float _arrowSpeed = 10f;

    private WeaponItemData _currentWeapon;
    private bool _isShoot = false;
    private bool _isEnable = false;
    private bool _isAim = false;

    public event UnityAction OnInitialized;
    public event UnityAction OnCleared;
    public event UnityAction OnShooting;

    private void Start()
    {
        foreach (var arrow in _arrows)
        {
            arrow.gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        _playerInteraction.OnUpdateWeaponItemData += Init;

        _inputHandler.InteractionPlayerInput.OnUse += Shoot;
        _inputHandler.InteractionPlayerInput.OnAim += Aim;

        foreach (var arrow in _arrows)
        {
            arrow.OnEnteredCollider += InstantiateArrow;
        }
    }

    private void OnDisable()
    {
        _playerInteraction.OnUpdateWeaponItemData -= Init;

        _inputHandler.InteractionPlayerInput.OnUse -= Shoot;
        _inputHandler.InteractionPlayerInput.OnAim -= Aim;

        foreach (var arrow in _arrows)
        {
            arrow.OnEnteredCollider -= InstantiateArrow;
        }
    }

    public void Aim()
    {
        if (_isEnable)
        {
            if (_isAim)
            {
                _isAim = false;
            }
            else
            {
                _isAim = true;
            }
            _playerAnimatorHandler.Aim(_isAim);
        }
    }

    public void Shoot()
    {
        if (_isAim && _playerInteraction.IsShoot())
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

    public void InstantiateArrow(Arrow arrow, Transform parent, Animals animals)
    {
        if(Random.Range(0, 2) == 0)
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
        {
            _playerInteraction.TakeDamageAnimal(animals, _currentWeapon.Damage, _currentWeapon.OverTimeDamage);
        }
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
