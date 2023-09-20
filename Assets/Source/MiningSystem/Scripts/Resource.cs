using System.Collections;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public abstract class Resource : MonoBehaviour, IDamagable
{
    [SerializeField] private float _maxHealth = 100;
    [SerializeField] private ToolType _extractionType;
    [SerializeField] private ItemPickUp _item;
    [SerializeField] private int _countItem;
    [SerializeField] private ParticleSystem _takeDamage;
    [SerializeField] private GameObject _parent;

    protected Collider Сollider;
    protected Rigidbody Rigidbody;

    private float _curenntHealth;
    private float _radiusSpawnLoots = 1;
    private float _spawnLootUp = 1;
    private float _disappearanceTime = 5;
    private bool _isDead = false;
    
    public event Action Died;
    public event Action Disappeared;

    public float Health => _curenntHealth;
    public float MaxHealth => _maxHealth;
    public ToolType ExtractionType => _extractionType;

    public virtual void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
        Сollider = GetComponent<Collider>();
    }

    public virtual void OnEnable()
    {
        _curenntHealth = _maxHealth;
        Сollider.enabled = true;
    }

    public virtual void TakeDamage(float damage, float overTimeDamage)
    {
        _takeDamage.Play();
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
        SpawnItem(_item, _radiusSpawnLoots, _spawnLootUp, _countItem);
        _isDead = true;
        StartCoroutine(Precipice());
    }

    public virtual void SpawnItem(ItemPickUp itemPickUp, float radius, float spawnPointUp,int count)
    {
        if (_isDead == false)
        { 
            for (int i = 0; i < count; i++)
            {
                Vector3 position = (transform.position + Random.insideUnitSphere * radius);
                SpawnLoots.Spawn(itemPickUp,position,transform,false,spawnPointUp,false);
            }
        }
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

    IEnumerator Precipice()
    {
        DiedEvent();
        yield return new WaitForSeconds(_disappearanceTime/2);
        Сollider.enabled = false;
        yield return new WaitForSeconds(_disappearanceTime/2);
        Rigidbody.isKinematic = true;
        gameObject.SetActive(false);
        DisappearedEvent();
    }
}