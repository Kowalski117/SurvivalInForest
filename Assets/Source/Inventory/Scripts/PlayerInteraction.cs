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
    [SerializeField] private TimeHandler _timeHandler;

    private WeaponItemData _currentWeapon;
    private ToolItemData _currentTool;
    private InventoryItemData _currentItemData;
    private InventoryItemData _previousItemData;
    private Resource _currentResoure;
    private Animals _currentAnim;
    private Box _currentBox;
    private InventorySlot _currentInventorySlot;

    private float _maxDelayFire = 10f;
    private float _nextFireDelay;
    private bool _isEnable;
    private float _hourInSeconds = 3600;

    public event UnityAction<InventoryItemData> OnUpdateItemData;
    public event UnityAction<WeaponItemData> OnUpdateWeaponItemData;
    public event UnityAction<ToolItemData> OnUpdateToolItemData;
    public event UnityAction<float> OnValueChanged;
    public event UnityAction<float, float> OnEnableBarValue;
    public event UnityAction OnTurnOffBarValue;

    public InventorySlot CurrentInventorySlot => _currentInventorySlot;

    private void Awake()
    {
        _nextFireDelay = _maxDelayFire;
    }

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

        if (_nextFireDelay <= _maxDelayFire)
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
                _playerAnimation.Hit();
                _audioSource.PlayOneShot(_currentWeapon.MuzzleSound);
                //_currentWeapon.MuzzleFlash.Play();
                _nextFireDelay = 0;
                return true;
            }
        }
        return false;
    }

    public void UpdateDurabilityItem(InventorySlot inventorySlot)
    {
        if (inventorySlot.Durability > 0)
        {
            inventorySlot.LowerStrength(1);

            if (inventorySlot.Durability <= 0)
            {
                inventorySlot.UpdateDurabilityIfNeeded();
                _inventory.RemoveInventory(inventorySlot, 1);
                inventorySlot = null;
            }
        }
    }

    public void UpdateDurabilityWithGameTime(InventorySlot inventorySlot)
    {
        if (inventorySlot.Durability > 0)
        {
            inventorySlot.LowerStrength(_timeHandler.TimeMultiplier / _hourInSeconds * Time.deltaTime);

            if (inventorySlot.Durability <= 0)
            {
                inventorySlot.UpdateDurabilityIfNeeded();

                if(inventorySlot.ItemData is ClothesItemData clothesItemData && clothesItemData.DischargedItem != null)               
                    _inventory.AddToInventory(clothesItemData.DischargedItem, 1);
                
                _inventory.RemoveInventory(inventorySlot, 1);
                inventorySlot = null;
            }
        }
    }

    private void Hit()
    {
        if (_nextFireDelay > _currentWeapon.Speed)
        {
            _audioSource.PlayOneShot(_currentWeapon.MuzzleSound);
            _playerAnimation.Hit();
            TakeDamageAnimal(_currentAnim, _currentWeapon.Damage, _currentWeapon.OverTimeDamage);
            TakeDamageBrokenObject(_currentWeapon.Damage, 0);
            _nextFireDelay = 0;
        }
    }

    private void InteractResource()
    {
        if (_nextFireDelay > _currentTool.Speed)
        {
            _nextFireDelay = 0;
            _playerAnimation.Hit();

            if (_currentTool != null)
            {
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
            UpdateDurabilityItem(_currentInventorySlot);

            OnValueChanged?.Invoke(animals.Health);

            if (animals.Health <= 0)
            {
                OnValueChanged?.Invoke(animals.Health);
                animals = null;
            }
        }
    }

    private void CreateParticle(InventoryItemData inventoryItemData, ParticleSystem particle)
    {
        if(_playerAnimation.CurrentItemAnimation.ItemData == inventoryItemData)
        {
            if (_playerAnimation.CurrentItemAnimation.ParticleSpawnPoint != null && particle != null)
                Instantiate(particle, _playerAnimation.CurrentItemAnimation.ParticleSpawnPoint.position, Quaternion.identity);
        }
    }
    
    private void TakeDamageBrokenObject(float damage, float overTimeDamage)
    {
        if (_currentBox != null)
        {
            _currentBox.TakeDamage(damage, overTimeDamage);
            UpdateDurabilityItem(_currentInventorySlot);

            OnValueChanged?.Invoke(_currentBox.Endurance);

            if (_currentBox.Endurance <= 0)
            {
                OnValueChanged?.Invoke(_currentBox.Endurance);
                _currentBox = null;
            }
        }
    }

    private void TakeDamageResoure(float damage, float overTimeDamage)
    {
        if(_currentResoure != null)
        {
            if (_currentResoure.ExtractionType == _currentTool.ToolType)
            {
                CreateParticle(_currentTool, _currentTool.HitEffect);
                _currentResoure.TakeDamage(damage, overTimeDamage);
                UpdateDurabilityItem(_currentInventorySlot);
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
        _currentAnim = other.GetComponentInParent<Animals>();

        if (_currentAnim != null)
        {
            OnEnableBarValue?.Invoke(_currentAnim.MaxHealth, _currentAnim.Health);
        }

        if (other.TryGetComponent(out Resource resource))
        {
            if (resource != null)
            {
                _currentResoure = resource;
                OnEnableBarValue?.Invoke(_currentResoure.MaxHealth, _currentResoure.Health);
            }

        }

        if (other.TryGetComponent(out Box brokenObject))
        {
            if (brokenObject != null)
            {
                _currentBox = brokenObject;
                OnEnableBarValue?.Invoke(_currentBox.MaxEndurance, _currentBox.Endurance);
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

        if (other.TryGetComponent(out Box brokenObject))
        {
            _currentBox = null;
            OnTurnOffBarValue?.Invoke();
        }
    }
}
