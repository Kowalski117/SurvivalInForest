using UnityEngine;

public class Weapon : Raycast
{
    [SerializeField] private WeaponPlayerInput _weaponPlayerInput;
    [SerializeField] private LayerMask _creatureLayer;
    [SerializeField] private float _damage;
    [SerializeField] private float _speedShoot;
    [SerializeField] private ItemPickUp _bullet;
    [SerializeField] private ParticleSystem _muzzleFlash;
    [SerializeField] private ParticleSystem _hitEffect;
    [SerializeField] private AudioClip _muzzleSound;
    [SerializeField] private AudioSource _audioSource;

    private float _nextFire;

    private void OnEnable()
    {
        _weaponPlayerInput.OnShoot += Shoot;
    }

    private void OnDisable()
    {
        _weaponPlayerInput.OnShoot -= Shoot;
    }

    private void Shoot()
    {
        if(Time.time > _nextFire)
        {
            _nextFire = Time.time + 1 / _speedShoot;

            Debug.Log("ВЫСТРЕЛ");
            _audioSource.PlayOneShot(_muzzleSound);
            _muzzleFlash.Play();

            if (IsRayHittingSomething(_creatureLayer, out RaycastHit hitInfo))
            {
                Vector3 spawnPoint = hitInfo.collider.ClosestPointOnBounds(hitInfo.point);

                if (_bullet != null)
                {
                    ItemPickUp bullet = Instantiate(_bullet, spawnPoint, Quaternion.LookRotation(hitInfo.normal), hitInfo.collider.transform);
                }

                if (hitInfo.collider.TryGetComponent(out Animals animals))
                {
                    ParticleSystem impact = Instantiate(_hitEffect, spawnPoint, Quaternion.LookRotation(hitInfo.normal), hitInfo.collider.transform);
                    impact.Play();
                    Debug.Log("Попал в животное");
                    animals.TakeDamage(_damage, 0);
                }
            }
        }
    }
}
