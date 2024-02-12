using StarterAssets;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInteraction : Raycast
{
    [SerializeField] private PlayerInventoryHolder _inventory;
    [SerializeField] private HotbarDisplay _hotbarDisplay;
    [SerializeField] private ToolItemData _armItemData;
    [SerializeField] private PlayerHandler _playerInputHandler;
    [SerializeField] private LayerMask _creatureLayer;
    [SerializeField] private PlayerAudioHandler _playerAudioHandler;
    [SerializeField] private PlayerAnimatorHandler _playerAnimation;
    [SerializeField] private StarterAssetsInputs _starterAssetsInputs;
    [SerializeField] private TimeHandler _timeHandler;
    [SerializeField] private LayerMask _usingLayer;
    [SerializeField] private ShakeEffect _shakeEffect;

    private WeaponItemData _currentWeapon;
    private ToolItemData _currentTool;
    private InventoryItemData _currentItemData;
    private InventoryItemData _previousItemData;
    private Resource _currentResoure;
    private Animals _currentAnim;
    private Box _currentBrokenObject;
    private InventorySlot _currentInventorySlot;

    private bool _isActiveGoal = false;
    private float _maxDelayFire = 10f;
    private float _nextFireDelay;
    private bool _isEnable;
    private float _hourInSeconds = 3600;
    private Vector3 _particlePosition;
    private ParticleSystem _selectionParticle;
    private AudioClip _selectionAudioClip;

    private string[] _animalsName = { "Deer", "Hare", "Bear", "Wolf" };

    public event UnityAction<InventoryItemData> OnUpdateItemData;
    public event UnityAction<WeaponItemData> OnUpdateWeaponItemData;
    public event UnityAction<ToolItemData> OnUpdateToolItemData;
    public event UnityAction<float> OnValueChanged;
    public event UnityAction<float, float> OnEnableBarValue;
    public event UnityAction OnTurnOffBarValue;

    public event UnityAction OnTreeBroken;
    public event UnityAction OnStoneBroken;
    public event UnityAction OnTreasureBroken;

    public event UnityAction OnBearKilled;
    public event UnityAction OnWolfKilled;
    public event UnityAction OnDeerKilled;
    public event UnityAction OnHareKilled;

    public InventorySlot CurrentInventorySlot => _currentInventorySlot;

    protected override void Awake()
    {
        base.Awake();
        _nextFireDelay = _maxDelayFire;
    }

    private void OnEnable()
    {
        _playerInputHandler.InteractionPlayerInput.OnAttack += UseItem;
        _hotbarDisplay.OnItemSwitched += UpdateItemData;
    }

    private void OnDisable()
    {
        _playerInputHandler.InteractionPlayerInput.OnAttack -= UseItem;
        _hotbarDisplay.OnItemSwitched -= UpdateItemData;
    }

    private void Update()
    {
        if (_nextFireDelay <= _maxDelayFire)
            _nextFireDelay += Time.deltaTime;

        if (_isEnable)
        {
            if (_currentWeapon != null && _currentWeapon.WeaponType == WeaponType.MeleeWeapon)
            {
                if (_nextFireDelay > _currentWeapon.Speed)
                {
                    _playerAnimation.Hit(_isActiveGoal);

                    _nextFireDelay = 0;
                }
            }
            else if (_currentTool != null && _currentWeapon == null)
            {
                if (_nextFireDelay > _currentTool.Speed)
                {
                    _playerAnimation.Hit(_isActiveGoal);

                    _nextFireDelay = 0;
                }
            }
        }

        if (IsRayHittingSomething(_usingLayer, out RaycastHit hitInfo))
        {
            _particlePosition = hitInfo.point;
            _currentAnim = hitInfo.collider.GetComponentInParent<Animals>();

            if (_currentAnim != null)
            {
                OnEnableBarValue?.Invoke(_currentAnim.MaxHealth, _currentAnim.Health);
            }

            if (hitInfo.collider.TryGetComponent(out Resource resource))
            {
                if (resource != null)
                {
                    _currentResoure = resource;
                    _selectionParticle = _currentResoure.DamageDoneParticleParticle;
                    _selectionAudioClip = _currentResoure.DamageDoneAudioClip;
                    OnEnableBarValue?.Invoke(_currentResoure.MaxHealth, _currentResoure.Health);
                }
            }

            if (hitInfo.collider.TryGetComponent(out Box brokenObject))
            {
                if (brokenObject != null)
                {
                    _currentBrokenObject = brokenObject;
                    _selectionAudioClip = _currentBrokenObject.DamageClip;
                    OnEnableBarValue?.Invoke(_currentBrokenObject.MaxEndurance, _currentBrokenObject.Endurance);
                }
            }

            if (_currentAnim || _currentResoure || _currentBrokenObject)
                _isActiveGoal = true;
        }
        else
        {
            if(_currentAnim == null || _currentResoure == null || _currentBrokenObject == null)
            {
                _currentAnim = null;
                _currentResoure = null;
                _currentBrokenObject = null;
                _selectionParticle = null;
                _selectionAudioClip = null;
                _isActiveGoal = false;
                OnTurnOffBarValue?.Invoke();
            }
        }
    }

    public void UpdateItemData(InventorySlotUI slotUI)
    {
        _nextFireDelay = _maxDelayFire;
        _previousItemData = _currentItemData;
        _currentInventorySlot = slotUI.AssignedInventorySlot;
        _currentItemData = _currentInventorySlot.ItemData;
        OnUpdateItemData?.Invoke(_currentItemData);
        InitWeapon(_currentItemData);
        InitTool(_currentItemData);
    }

    public void InitWeapon(InventoryItemData itemData)
    {
        if (itemData != null)
        {
            if (itemData is WeaponItemData weaponItemData)          
                _currentWeapon = weaponItemData;           
            else           
                _currentWeapon = null;
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
                _currentTool = toolItemData;
            else
                _currentTool = _armItemData;
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
                _playerAnimation.Hit(true);
                _playerAudioHandler.PlayOneShot(_currentWeapon.MuzzleSound);
                _nextFireDelay = 0;
                return true;
            }
        }
        return false;
    }

    public void UpdateDurabilityItem(InventorySlot inventorySlot)
    {
        if (_hotbarDisplay.CurrentSlot.AssignedInventorySlot.ItemData is FoodItemData foodItemData && inventorySlot == _hotbarDisplay.CurrentSlot.AssignedInventorySlot)
            return;

        if (inventorySlot.Durability > 0)
        {
            if (inventorySlot.ItemData is ToolItemData toolItemData && toolItemData.ToolType == ToolType.Torch)
                inventorySlot.LowerStrength(1 / 5);
            else
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

    public void Hit()
    {
        if (_currentWeapon != null)
        {
            if (_currentAnim != null)
                TakeDamageAnimal(_currentAnim, _currentWeapon.Damage, _currentWeapon.OverTimeDamage);
            else if(_currentBrokenObject != null)
                TakeDamageBrokenObject(_currentWeapon.Damage, 0);
            PlayEffect();
        }
    }

    public void InteractResource()
    {
        if (_currentTool != null)
        {
            if (_currentResoure != null)
                TakeDamageResoure(_currentTool.DamageResources, 0);
            else if (_currentAnim != null)
                TakeDamageAnimal(_currentAnim, _currentTool.DamageLiving, 0);
            else if (_currentBrokenObject != null)
                TakeDamageBrokenObject(_currentTool.DamageResources, 0);
            PlayEffect();
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

            OnValueChanged?.Invoke(animals.Health);

            if (animals.Health <= 0)
            {
                OnValueChanged?.Invoke(animals.Health);
                IdentifyKilledAnimal(animals);
                animals = null;
            }

            UpdateDurabilityItem(_currentInventorySlot);
        }
    }

    private void TakeDamageBrokenObject(float damage, float overTimeDamage)
    {
        if (_currentBrokenObject != null)
        {
            _currentBrokenObject.TakeDamage(damage, overTimeDamage);

            OnValueChanged?.Invoke(_currentBrokenObject.Endurance);

            if (_currentBrokenObject.Endurance <= 0)
            {
                OnValueChanged?.Invoke(_currentBrokenObject.Endurance);
                _currentBrokenObject = null;
            }

            UpdateDurabilityItem(_currentInventorySlot);
        }
    }

    private void TakeDamageResoure(float damage, float overTimeDamage)
    {
        if (_currentResoure != null && _currentResoure.Health > 0)
        {
            if (_currentResoure.ExtractionType == _currentTool.ToolType)
            {
                if(!(_currentResoure is Stone stone && stone.ResourseType == _currentTool.ResourseType || _currentTool.ResourseType == ResourseType.All ))
                    return;

                if (_currentResoure.Health > 0)
                {
                    _currentResoure.TakeDamage(damage, overTimeDamage);
                    UpdateDurabilityItem(_currentInventorySlot);
                }  
            }
            else if (_currentTool.ToolType == ToolType.Arm)
            {
                _currentResoure.TakeDamage(damage, overTimeDamage);
            }

            OnValueChanged?.Invoke(_currentResoure.Health);

            if (_currentResoure.Health <= 0)
            {
                IdentifyBrokenResource(_currentResoure);
                 OnValueChanged?.Invoke(_currentResoure.Health);
                _currentResoure = null;
            }
        }
    }

    private void PlayEffect()
    {
        if (_selectionParticle)
            Instantiate(_selectionParticle, _particlePosition, Quaternion.identity);

        if (_selectionAudioClip)
            _playerAudioHandler.PlayOneShot(_selectionAudioClip);

        if(_shakeEffect)
            _shakeEffect.StartShake();
    }

    private void IdentifyBrokenResource(Resource resource)
    {
        if (resource is Tree tree)
            OnTreeBroken?.Invoke();
        else if (resource is Stone stone)
            OnStoneBroken?.Invoke();
        else if (resource is Treasure treasure)
            OnTreasureBroken?.Invoke();
    }

    private void IdentifyKilledAnimal(Animals animals)
    {
        if (animals.Name == _animalsName[0])
            OnDeerKilled?.Invoke();
        else if (animals.Name == _animalsName[1])
            OnHareKilled?.Invoke();
        else if (animals.Name == _animalsName[2])
            OnBearKilled?.Invoke();
        else if (animals.Name == _animalsName[3]) 
            OnWolfKilled?.Invoke();

    }
}
