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
    [SerializeField] private ParticleSystem _selectionParticle;


    protected Collider Сollider;
    protected Rigidbody Rigidbody;

    private float _curenntHealth;
    private float _radiusSpawnLoots = 1;
    private float _spawnLootUp = 1;
    private bool _isDead;

    public event Action Died;
    public event Action Disappeared;

    public float Health => _curenntHealth;
    public float MaxHealth => _maxHealth;
    public ToolType ExtractionType => _extractionType;
    public ParticleSystem SelectionParticle => _selectionParticle;

    public virtual void Awake()
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

        for (int i = 0; i < _loots.Count; i++)
        {
            SpawnItem(_loots[i], _radiusSpawnLoots, _spawnLootUp);
        }
        
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
        yield return new WaitForSeconds(_disappearanceTime / 2);
        Сollider.enabled = false;
        yield return new WaitForSeconds(_disappearanceTime / 2);
        Rigidbody.isKinematic = true;
        gameObject.SetActive(false);
        DisappearedEvent();
    }
}