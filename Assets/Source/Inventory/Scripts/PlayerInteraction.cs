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

    private float _nextFireDelay;
    private float _maxDelayFire = 10f;
    private bool _isEnable;

    public event UnityAction<InventoryItemData> OnUpdateItemData;
    public event UnityAction<WeaponItemData> OnUpdateWeaponItemData;
    public event UnityAction<ToolItemData> OnUpdateToolItemData;
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
        UpdateItemData();
        InitWeapon(_currentInventorySlot.ItemData);
        InitTool(_currentInventorySlot.ItemData);

        if(_nextFireDelay <= _maxDelayFire)
            _nextFireDelay += Time.deltaTime;

        if (_isEnable)
        {
            if (_currentWeapon != null && _currentWeapon.WeaponType == WeaponType.MeleeWeapon)
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
        _currentInventorySlot = _hotbarDisplay.GetInventorySlotUI().AssignedInventorySlot;
        _currentItemData = _currentInventorySlot.ItemData;
        OnUpdateItemData?.Invoke(_currentItemData);
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

        OnUpdateWeaponItemData?.Invoke(_currentWeapon);
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

        OnUpdateToolItemData?.Invoke(_currentTool);
    }

    public bool IsShoot()
    {

        if (_nextFireDelay > _currentWeapon.Speed)
        {
            if (_inventory.RemoveInventory(_currentWeapon.Bullet.ItemData, 1))
            {
                _nextFireDelay = 0;
                _playerAnimation.Hit();
                _audioSource.PlayOneShot(_currentWeapon.MuzzleSound);
                //_currentWeapon.MuzzleFlash.Play();

                //if (IsRayHittingSomething(_creatureLayer, out RaycastHit hitInfo))
                //{
                //    Vector3 spawnPoint = hitInfo.collider.ClosestPointOnBounds(hitInfo.point);

                //    if (hitInfo.collider.TryGetComponent(out Animals animals))
                //    {
                //        if (_currentWeapon.HitEffect != null)
                //        {
                //            ParticleSystem impact = Instantiate(_currentWeapon.HitEffect, spawnPoint, Quaternion.LookRotation(hitInfo.normal), hitInfo.collider.transform);
                //            impact.Play();
                //        }
                //        _currentAnim = animals;
                //        TakeDamageAnimal(_currentWeapon.Damage, _currentWeapon.OverTimeDamage);
                //    }
                //}
                return true;
            }
        }
        return false;
    }

    public void UpdateDurabilityItem()
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

    private void Hit()
    {
        //_nextFireDelay += Time.deltaTime;

        if (_nextFireDelay > _currentWeapon.Speed)
        {
            _nextFireDelay = 0;
            _audioSource.PlayOneShot(_currentWeapon.MuzzleSound);
            _playerAnimation.Hit();

            TakeDamageAnimal(_currentAnim, _currentWeapon.Damage, _currentWeapon.OverTimeDamage);
            TakeDamageBrokenObject(_currentWeapon.Damage, 0);
        }
    }

    private void InteractResource()
    {
        //_nextFireDelay += Time.deltaTime;

        if (_nextFireDelay > _currentTool.Speed)
        {
            _nextFireDelay = 0;
            _playerAnimation.Hit();

            if (_currentTool != null)
            {
                //_audioSource.PlayOneShot(_currentTool.MuzzleSound);
                //_currentTool.MuzzleFlash.Play();

                TakeDamageResoure(_currentTool.DamageResources, 0);
                TakeDamageAnimal(_currentAnim, _currentTool.DamageLiving, 0);
                TakeDamageBrokenObject(_currentTool.DamageResources, 0);
            }
        }
    }

    private void UseItem(bool isActive)
    {
        _isEnable = isActive;
    }

    public void TakeDamageAnimal(Animals animals,float damage, float overTimeDamage)
    {
        if (animals != null)
        {
            animals.TakeDamage(damage, overTimeDamage);
            UpdateDurabilityItem();

            OnValueChanged?.Invoke(animals.Health);

            if (animals.Health <= 0)
            {
                OnValueChanged?.Invoke(animals.Health);
                animals = null;
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
