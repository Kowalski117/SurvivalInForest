using System.Collections;
using UnityEngine;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public abstract class Resource : MonoBehaviour, IDamagable
{
    [SerializeField] private float _maxHealth = 100;
    [SerializeField] private ToolType _extractionType;
    [SerializeField] private float _disappearanceTime = 10;
    [SerializeField] private List<ItemPickUp> _loots;
    [SerializeField] private ParticleSystem _damageDoneParticle;
    [SerializeField] private AudioClip[] _damageDoneAudioClips;

    protected Collider Сollider;
    protected Rigidbody Rigidbody;

    private float _curenntHealth;
    private float _radiusSpawnLoots = 1;
    private float _spawnLootUp = 1;
    private bool _isDead;
    private float _divider = 2;

    public event Action Died;
    public event Action Disappeared;

    public float Health => _curenntHealth;
    public float MaxHealth => _maxHealth;
    public ToolType ExtractionType => _extractionType;
    public ParticleSystem DamageDoneParticleParticle => _damageDoneParticle;
    public AudioClip DamageDoneAudioClip => _damageDoneAudioClips[Random.Range(0, _damageDoneAudioClips.Length)];

    protected virtual void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
        Сollider = GetComponent<Collider>();
    }

    public virtual void OnEnable()
    {
        _curenntHealth = _maxHealth;
    }

    public virtual void TakeDamage(float damage, float overTimeDamage)
    {
        _curenntHealth -= (int) damage;

        if (_curenntHealth <= 0 & _isDead == false)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        _curenntHealth = 0;
        Rigidbody.isKinematic = false;
        SpawnLoot();
        _isDead = true;
        StartCoroutine(Precipice());
    }

    public virtual void SpawnItem(ItemPickUp itemPickUp, float radius, float spawnPointUp)
    {
        if (_isDead == false)
        {
            Vector3 position = (transform.position + Random.insideUnitSphere * radius);
            SpawnLoots.Spawn(itemPickUp, position, transform, false, spawnPointUp, false);
        }
    }
    
    public virtual void SpawnLoot()
    {
        for (int i = 0; i < _loots.Count; i++)
        {
            SpawnItem(_loots[i], _radiusSpawnLoots, _spawnLootUp);
        }
    }

    public void EnableCollider()
    {
        Сollider.enabled = true;
    }

    protected void DiedEvent()
    {
        Died?.Invoke();
    }

    protected void DisappearedEvent()
    {
        _isDead = false;
        Disappeared?.Invoke();
    }

    public virtual IEnumerator Precipice()
    {
        DiedEvent();
        yield return new WaitForSeconds(_disappearanceTime / _divider);
        Сollider.enabled = false;
        yield return new WaitForSeconds(_disappearanceTime / _divider);
        Rigidbody.isKinematic = true;
        gameObject.SetActive(false);
        DisappearedEvent();
    }
}