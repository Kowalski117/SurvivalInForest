using UnityEditor.Media;
using UnityEditor.PackageManager;
using UnityEngine;

public class Weapon : Raycast
{
    [SerializeField] private PlayerInventoryHolder _inventory;
    [SerializeField] private HotbarDisplay _hotbarDisplay;
    [SerializeField] private WeaponPlayerInput _weaponPlayerInput;
    [SerializeField] private LayerMask _creatureLayer;
    [SerializeField] private AudioSource _audioSource;

    private WeaponItemData _currentWeapon;

    private Animals _currentAnim;
    private float _nextFire;

    private void OnEnable()
    {
        _weaponPlayerInput.OnShoot += UseWeapon;
        _hotbarDisplay.ItemClicked += Init;
    }

    private void OnDisable()
    {
        _weaponPlayerInput.OnShoot -= UseWeapon;
        _hotbarDisplay.ItemClicked -= Init;
    }

    public void Init(InventoryItemData itemData)
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
                return;
            }
        }
        else
        {
            _currentWeapon = null;
            return;
        }
    }

    private void UseWeapon()
    {
        var itemData = _hotbarDisplay.GetInventorySlotUI().AssignedInventorySlot.ItemData;

        Init(itemData);

        if (_currentWeapon != null && _currentWeapon.WeaponType == WeaponType.RangedWeapon)
        {
            Shoot();
        }
        else if (_currentWeapon != null && _currentWeapon.WeaponType == WeaponType.MeleeWeapon)
        {
            Hit();
        }
    }

    private void Shoot()
    {
        if (Time.time > _nextFire)
        {
            if (_inventory.InventorySystem.RemoveItemsInventory(_currentWeapon.Bullet.ItemData, 1))
            {
                _nextFire = Time.time + 1 / _currentWeapon.Speed;

                _audioSource.PlayOneShot(_currentWeapon.MuzzleSound);
                //_currentWeapon.MuzzleFlash.Play();

                if (IsRayHittingSomething(_creatureLayer, out RaycastHit hitInfo))
                {
                    Vector3 spawnPoint = hitInfo.collider.ClosestPointOnBounds(hitInfo.point);

                    if (hitInfo.collider.TryGetComponent(out Animals animals))
                    {
                        if(_currentWeapon.HitEffect != null)
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

            if (_currentAnim != null)
                _currentAnim.TakeDamage(_currentWeapon.Damage, 0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Animals animals))
        {
            if (animals != null)
                _currentAnim = animals;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Animals animals))
        {
            if (animals != null)
                _currentAnim = null;
        }
    }
}