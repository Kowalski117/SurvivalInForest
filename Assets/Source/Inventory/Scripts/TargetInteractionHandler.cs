using StarterAssets;
using System;
using UnityEngine;

public class TargetInteractionHandler : Raycast
{
    [SerializeField] private LayerMask _usingLayer;

    [SerializeField] private PlayerEquipmentHandler _distributorItem;
    [SerializeField] private StarterAssetsInputs _starterAssetsInputs;
    [SerializeField] private CameraShakerEffect _shakeEffect;
    [SerializeField] private PlayerAudioHandler _playerAudioHandler;

    private Resource _currentResoure;
    private Animals _currentAnim;
    private Box _currentBrokenObject;

    private Vector3 _particlePosition;

    public event Action<float> OnValueChanged;
    public event Action<float, float> OnEnableBarValue;
    public event Action OnTurnOffBarValue;

    public event Action OnTreeBroken;
    public event Action OnStoneBroken;
    public event Action OnTreasureBroken;

    public event Action OnBearKilled;
    public event Action OnWolfKilled;
    public event Action OnDeerKilled;
    public event Action OnHareKilled;

    private void Update()
    {
        if (IsRayHittingSomething(_usingLayer, out RaycastHit hitInfo))
        {
            _particlePosition = hitInfo.point;
            _currentAnim = hitInfo.collider.GetComponentInParent<Animals>();

            if (_currentAnim != null)
                ShowBar(_currentAnim.MaxHealth, _currentAnim.Health);

            if (hitInfo.collider.TryGetComponent(out Resource resource))
            {
                if (resource != null)
                {
                    _currentResoure = resource;
                    ShowBar(_currentResoure.MaxHealth, _currentResoure.Health);
                }
            }

            if (hitInfo.collider.TryGetComponent(out Box brokenObject))
            {
                if (brokenObject != null)
                {
                    _currentBrokenObject = brokenObject;
                    ShowBar(_currentBrokenObject.MaxEndurance, _currentBrokenObject.Endurance);
                }
            }

            if (_currentAnim || _currentResoure || _currentBrokenObject)
                _distributorItem.SetActiveGoal(true);
        }
        else
        {
            if(_currentAnim == null || _currentResoure == null || _currentBrokenObject == null)
            {
                _currentAnim = null;
                _currentResoure = null;
                _currentBrokenObject = null;
                _distributorItem.SetActiveGoal(false);
                OnTurnOffBarValue?.Invoke();
            }
        }
    }

    public void Hit()
    {
        if (_distributorItem.CurrentWeapon != null)
        {
            if (_currentAnim != null)
                TakeDamageAnimal(_currentAnim, _distributorItem.CurrentWeapon.Damage, _distributorItem.CurrentWeapon.OverTimeDamage);
            else if (_currentBrokenObject != null)
                TakeDamageBrokenObject(_distributorItem.CurrentWeapon.Damage, 0);
        }
    }

    public void InteractResource()
    {
        if (_distributorItem.CurrentTool != null)
        {
            if (_currentResoure != null)
                TakeDamageResoure(_distributorItem.CurrentTool.DamageResources, 0);
            else if (_currentAnim != null)
                TakeDamageAnimal(_currentAnim, _distributorItem.CurrentTool.DamageLiving, 0);
            else if (_currentBrokenObject != null)
                TakeDamageBrokenObject(_distributorItem.CurrentTool.DamageResources, 0);
        }
    }

    public void TakeDamageAnimal(Animals animals, float damage, float overTimeDamage)
    {
        if (animals != null)
        {
            animals.TakeDamage(damage, overTimeDamage);

            OnValueChanged?.Invoke(animals.Health);

            if (animals.Health <= 0)
            {
                OnValueChanged?.Invoke(animals.Health);
                IdentifyKilledAnimal(animals);
            }

            _distributorItem.UpdateDurabilityItem(_distributorItem.CurrentInventorySlot);
        }
    }

    private void TakeDamageBrokenObject(float damage, float overTimeDamage)
    {
        if (_currentBrokenObject != null)
        {
            _currentBrokenObject.TakeDamage(damage, overTimeDamage);
            PlayEffect(null, _currentBrokenObject.DamageClip);

            OnValueChanged?.Invoke(_currentBrokenObject.Endurance);

            if (_currentBrokenObject.Endurance <= 0)
            {
                OnValueChanged?.Invoke(_currentBrokenObject.Endurance);
                _currentBrokenObject = null;
            }

            _distributorItem.UpdateDurabilityItem(_distributorItem.CurrentInventorySlot);
        }
    }

    private void TakeDamageResoure(float damage, float overTimeDamage)
    {
        if (_currentResoure != null && _currentResoure.Health > 0)
        {
            if (_currentResoure.ExtractionType == _distributorItem.CurrentTool.ToolType)
            {
                if(!(_currentResoure is Stone stone && stone.ResourseType == _distributorItem.CurrentTool.ResourseType || _distributorItem.CurrentTool.ResourseType == ResourseType.All ))
                    return;

                if (_currentResoure.Health > 0)
                {
                    _currentResoure.TakeDamage(damage, overTimeDamage);
                    _distributorItem.UpdateDurabilityItem(_distributorItem.CurrentInventorySlot);
                }  
            }
            else if (_distributorItem.CurrentTool.ToolType == ToolType.Arm)
                _currentResoure.TakeDamage(damage, overTimeDamage);

            PlayEffect(_currentResoure.DamageDoneParticleParticle, _currentResoure.DamageDoneAudioClip);
            OnValueChanged?.Invoke(_currentResoure.Health);

            if (_currentResoure.Health <= 0)
            {
                IdentifyBrokenResource(_currentResoure);
                 OnValueChanged?.Invoke(_currentResoure.Health);
                _currentResoure = null;
            }
        }
    }

    private void ShowBar(float maxValue, float currentValue)
    {
        if(currentValue > 0) 
            OnEnableBarValue?.Invoke(maxValue, currentValue);
    }

    private void PlayEffect(ParticleSystem selectionParticle = null, AudioClip selectionAudioClip = null)
    {
        if (selectionParticle)
            Instantiate(selectionParticle, _particlePosition, Quaternion.identity);

        if (selectionAudioClip)
            _playerAudioHandler.PlayOneShot(selectionAudioClip);

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
        if (animals.Name == GameConstants.Deer)
            OnDeerKilled?.Invoke();
        else if (animals.Name == GameConstants.Hare)
            OnHareKilled?.Invoke();
        else if (animals.Name == GameConstants.Bear)
            OnBearKilled?.Invoke();
        else if (animals.Name == GameConstants.Wolf) 
            OnWolfKilled?.Invoke();
    }
}
