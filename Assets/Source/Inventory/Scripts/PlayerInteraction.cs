using StarterAssets;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInteraction : Raycast
{
    [SerializeField] private PlayerInventoryHolder _inventory;
    [SerializeField] private HotbarDisplay _hotbarDisplay;
    [SerializeField] private ToolItemData _armItemData;
    [SerializeField] private PlayerInputHandler _playerInputHandler;
    [SerializeField] private LayerMask _creatureLayer;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private PlayerAnimatorHandler _playerAnimation;
    [SerializeField] private StarterAssetsInputs _starterAssetsInputs;

    private WeaponItemData _currentWeapon;
    private ToolItemData _currentTool;
    private InventoryItemData _currentItemData;
    private InventoryItemData _previousItemData;
    private Resource _currentResoure;
    private Animals _currentAnim;
    private BrokenObject _currentBrokenObject;
    private InventorySlot _currentInventorySlot;

    private float _nextFire;
    private bool _isEnable;

    public event UnityAction<InventoryItemData> OnUpdateItemData;
    public event UnityAction<float> OnValueChanged;
    public event UnityAction<float, float> OnEnableBarValue;
    public event UnityAction OnTurnOffBarValue;

    private void OnEnable()
    {
        _playerInputHandler.InteractionPlayerInput.OnAttack += UseItem;
    }

    private void OnDisable()
    {
        _playerInputHandler.InteractionPlayerInput.OnAttack -= UseItem;
    }

    private void Update()
    {
        if(_isEnable)
        {
            UpdateItemData();
            InitWeapon(_currentItemData);
            InitTool(_currentItemData);

            if (_currentWeapon != null && _currentWeapon.WeaponType == WeaponType.RangedWeapon)
            {
                Shoot();
            }
            else if (_currentWeapon != null && _currentWeapon.WeaponType == WeaponType.MeleeWeapon)
            {
                Hit();
            }
            else if (_currentTool != null && _currentWeapon == null)
            {
                InteractResource();          
            }
        }
    }

    public void UpdateItemData()
    {
        _previousItemData = _currentItemData;
        _currentItemData = _hotbarDisplay.GetInventorySlotUI().AssignedInventorySlot.ItemData;
        _currentInventorySlot = _hotbarDisplay.GetInventorySlotUI().AssignedInventorySlot;
        OnUpdateItemData?.Invoke(_currentItemData);

        //if (_currentItemData != _previousItemData)
        //{
        //    _playerAnimation.GetItem(_currentItemData);
        //}
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
                _currentWeapon = null;
            }
        }
        else
        {
            _currentWeapon = null;
        }
    }

    public void InitTool(InventoryItemData itemData)
    {
        if (itemData != null)
        {
            if (itemData is ToolItemData toolItemData)
            {
                _currentTool = toolItemData;
            }
            else
            {
                _currentTool = _armItemData;
            }
        }
        else
        {
            _currentTool = _armItemData;
        }
    }

    private void Shoot()
    {
        if (Time.time > _nextFire)
        {
            if (_inventory.RemoveInventory(_currentWeapon.Bullet.ItemData, 1))
            {
                _nextFire = Time.time + 1 / _currentWeapon.Speed;
                _playerAnimation.Hit();
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
                        _currentAnim = animals;
                        TakeDamageAnimal(_currentWeapon.Damage, _currentWeapon.OverTimeDamage);
                    }

                    if (_currentWeapon.Bullet != null)
                    {
                        ItemPickUp bullet = Instantiate(_currentWeapon.Bullet, spawnPoint, Quaternion.identity, hitInfo.collider.transform);
                        bullet.GenerateNewID();
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
            _playerAnimation.Hit();

            TakeDamageAnimal(_currentWeapon.Damage, _currentWeapon.OverTimeDamage);
            TakeDamageBrokenObject(_currentWeapon.Damage, 0);
        }
    }

    private void InteractResource()
    {
        if (Time.time > _nextFire)
        {
            _playerAnimation.Hit();
            _nextFire = Time.time + 1 / _currentTool.Speed;

            if (_currentTool != null)
            {
                //_audioSource.PlayOneShot(_currentTool.MuzzleSound);
                //_currentTool.MuzzleFlash.Play();

                TakeDamageResoure(_currentTool.DamageResources, 0);
                TakeDamageAnimal(_currentTool.DamageLiving, 0);
                TakeDamageBrokenObject(_currentTool.DamageResources, 0);
            }
        }
    }

    private void UpdateDurabilityItem()
    {
        if (_currentInventorySlot.Durability > 0)
        {
            _currentInventorySlot.LowerStrength(1);

            if (_currentInventorySlot.Durability <= 0)
            {
                _currentInventorySlot.UpdateDurabilityIfNeeded();
                _inventory.RemoveInventory(_currentInventorySlot, 1);
                _currentInventorySlot = null;
            }
        }
    }

    private void UseItem(bool isActive)
    {
        _isEnable = isActive;
    }

    private void TakeDamageAnimal(float damage, float overTimeDamage)
    {
        if (_currentAnim != null)
        {
            _currentAnim.TakeDamage(damage, overTimeDamage);
            UpdateDurabilityItem();

            OnValueChanged?.Invoke(_currentAnim.Health);

            if (_currentAnim.Health <= 0)
            {
                OnValueChanged?.Invoke(_currentAnim.Health);
                _currentAnim = null;
            }
        }
    }
    
    private void TakeDamageBrokenObject(float damage, float overTimeDamage)
    {
        if (_currentBrokenObject != null)
        {
            Debug.Log(damage);
            _currentBrokenObject.TakeDamage(damage, overTimeDamage);
            UpdateDurabilityItem();

            OnValueChanged?.Invoke(_currentBrokenObject.Endurance);

            if (_currentBrokenObject.Endurance <= 0)
            {
                OnValueChanged?.Invoke(_currentBrokenObject.Endurance);
                _currentBrokenObject = null;
            }
        }
    }

    private void TakeDamageResoure(float damage, float overTimeDamage)
    {
        if(_currentResoure != null)
        {
            if (_currentResoure.ExtractionType == _currentTool.ToolType)
            {
                _currentResoure.TakeDamage(damage, overTimeDamage);
                UpdateDurabilityItem();
            }
            else if (_currentTool.ToolType == ToolType.Arm)
            {
                _currentResoure.TakeDamage(damage, overTimeDamage);
            }

            OnValueChanged?.Invoke(_currentResoure.Health);

            if (_currentResoure.Health <= 0)
            {
                OnValueChanged?.Invoke(_currentResoure.Health);
                _currentResoure = null;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Animals animals))
        {
            if (animals != null)
            {
                _currentAnim = animals;
                OnEnableBarValue?.Invoke(_currentAnim.MaxHealth, _currentAnim.Health);
            }
        }

        if (other.TryGetComponent(out Resource resource))
        {
            if (resource != null)
            {
                _currentResoure = resource;
                OnEnableBarValue?.Invoke(_currentResoure.MaxHealth, _currentResoure.Health);
            }

        }

        if (other.TryGetComponent(out BrokenObject brokenObject))
        {
            if (brokenObject != null)
            {
                _currentBrokenObject = brokenObject;
                OnEnableBarValue?.Invoke(_currentBrokenObject.MaxEndurance, _currentBrokenObject.Endurance);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Resource resource))
        {
            _currentResoure = null;
            OnTurnOffBarValue?.Invoke();
        }

        if (other.TryGetComponent(out Animals animals))
        {
            _currentAnim = null;
            OnTurnOffBarValue?.Invoke();
        }

        if (other.TryGetComponent(out BrokenObject brokenObject))
        {
            _currentBrokenObject = null;
            OnTurnOffBarValue?.Invoke();
        }
    }
}
