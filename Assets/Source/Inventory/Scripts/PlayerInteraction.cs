using UnityEngine;

public class PlayerInteraction : Raycast
{
    [SerializeField] private PlayerInventoryHolder _inventory;
    [SerializeField] private HotbarDisplay _hotbarDisplay;
    [SerializeField] private WeaponItemData _armItemData;
    [SerializeField] private WeaponPlayerInput _weaponPlayerInput;
    [SerializeField] private LayerMask _creatureLayer;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private PlayerAnimation _playerAnimation;

    private WeaponItemData _currentWeapon;
    private ToolItemData _currentTool;
    private InventoryItemData _currentItemData;
    private InventoryItemData _previousItemData;
    private Resource _currentResoure;
    private Animals _currentAnim;

    private float _nextFire;

    private void Update()
    {
        UpdateItemData();
    }

    private void OnEnable()
    {
        _weaponPlayerInput.OnShoot += UseItem;
    }

    private void OnDisable()
    {
        _weaponPlayerInput.OnShoot -= UseItem;
    }

    public void UpdateItemData()
    {
        _previousItemData = _currentItemData; // Добавлено: сохраняем предыдущий выбранный предмет
        _currentItemData = _hotbarDisplay.GetInventorySlotUI().AssignedInventorySlot.ItemData;

        // Добавлено: проверяем, отличается ли текущий предмет от предыдущего
        if (_currentItemData != _previousItemData)
        {
            _playerAnimation.GetItem(_currentItemData);
        }
    }

    private void UseItem()
    {
        InitWeapon(_currentItemData);
        InitTool(_currentItemData);

        if (_currentWeapon != null && _currentWeapon.WeaponType == WeaponType.RangedWeapon && _currentTool == null)
        {
            Shoot();
        }
        else if (_currentWeapon != null && _currentWeapon.WeaponType == WeaponType.MeleeWeapon && _currentTool == null)
        {
            Hit();
        }
        else if(_currentTool != null)
        {
            InteractResource();
        }
    }

    public void InitWeapon(InventoryItemData itemData)
    {
        if (itemData != null)
        {
            if (itemData is WeaponItemData weaponItemData)
            {
                _currentWeapon = weaponItemData;
            }
            else
            {
                _currentWeapon = _armItemData;
            }
        }
        else
        {
            _currentWeapon = _armItemData;
        }
    }

    public void InitTool(InventoryItemData itemData)
    {
        if (itemData != null)
        {
            if (itemData is ToolItemData weaponItemData)
            {
                _currentTool = weaponItemData;
            }
            else
            {
                _currentTool = null;
            }
        }
        else
        {
            _currentTool = null;
        }
    }

    private void Shoot()
    {
        if (Time.time > _nextFire)
        {
            if (_inventory.RemoveInventory(_currentWeapon.Bullet.ItemData, 1))
            {
                _nextFire = Time.time + 1 / _currentWeapon.Speed;

                _audioSource.PlayOneShot(_currentWeapon.MuzzleSound);
                //_currentWeapon.MuzzleFlash.Play();

                if (IsRayHittingSomething(_creatureLayer, out RaycastHit hitInfo))
                {
                    Vector3 spawnPoint = hitInfo.collider.ClosestPointOnBounds(hitInfo.point);

                    if (hitInfo.collider.TryGetComponent(out Animals animals))
                    {
                        if (_currentWeapon.HitEffect != null)
                        {
                            ParticleSystem impact = Instantiate(_currentWeapon.HitEffect, spawnPoint, Quaternion.LookRotation(hitInfo.normal), hitInfo.collider.transform);
                            impact.Play();
                        }

                        animals.TakeDamage(_currentWeapon.Damage, 0);
                    }

                    if (_currentWeapon.Bullet != null)
                    {
                        ItemPickUp bullet = Instantiate(_currentWeapon.Bullet, spawnPoint, Quaternion.LookRotation(hitInfo.normal), hitInfo.collider.transform);
                    }
                }
            }
        }
    }

    private void Hit()
    {
        if (Time.time > _nextFire)
        {
            _nextFire = Time.time + 1 / _currentWeapon.Speed;
            _audioSource.PlayOneShot(_currentWeapon.MuzzleSound);
            _playerAnimation.Hit(_currentWeapon);

            if (_currentAnim != null)
                _currentAnim.TakeDamage(_currentWeapon.Damage, 0);
        }
    }

    private void InteractResource()
    {
        if (Time.time > _nextFire)
        {
            _nextFire = Time.time + 1 / _currentTool.Speed;
            _audioSource.PlayOneShot(_currentTool.MuzzleSound);
            _playerAnimation.Hit(_currentTool);
            //_currentTool.MuzzleFlash.Play();

            if (_currentTool != null && _currentResoure != null)
            {
                if (_currentResoure.ExtractionType == _currentTool.ToolType)
                {
                    _currentResoure.TakeDamage(_currentTool.DamageResources, 0);

                    if (_currentResoure.Health <= 0)
                    {
                        _currentItemData = null;
                        _currentResoure = null;
                    }
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Animals animals))
        {
            if (animals != null)
                _currentAnim = animals;
        }

        if (other.TryGetComponent(out Resource resource))
        {
            if (resource != null)
                _currentResoure = resource;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Resource resource))
        {
            if (resource != null)
                _currentResoure = null;
        }

        if (other.TryGetComponent(out Animals animals))
        {
            if (animals != null)
                _currentAnim = null;
        }
    }
}
