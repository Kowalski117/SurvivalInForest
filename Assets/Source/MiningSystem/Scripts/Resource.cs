using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public abstract class Resource : MonoBehaviour, IDamagable
{
    [SerializeField] private float _maxHealth = 100;
    [SerializeField] private ToolType _extractionType;
    [SerializeField] private GameObject _loot;
    [SerializeField] private int _lootsCount;
    [SerializeField] private ParticleSystem _takeDamage;
    [SerializeField] private GameObject _parent;

    private float _curenntHealth;
    private float _radiusSpawnLoots = 1;
    private float _spawnLootUp = 1;
    private float _disappearanceTime = 5;
    private bool _isDead = false;
    private Rigidbody _rigidbody;
    private Collider _collider;
    public event Action Died;
    public float Health => _curenntHealth;
    public float MaxHealth => _maxHealth;
    public ToolType ExtractionType => _extractionType;

    public virtual void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();

    }

    public virtual void OnEnable()
    {
        _curenntHealth = _maxHealth;
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
        _rigidbody.isKinematic = false;
        SpawnLoot(_loot, _radiusSpawnLoots, _spawnLootUp, _lootsCount);
        _isDead = true;
        StartCoroutine(Precipice());
    }

    public virtual void SpawnLoot(GameObject gameObject, float radius, float spawnPointUp,int count)
    {
        if (_isDead == false)
        { 
            for (int i = 0; i < count; i++)
            {
              GameObject current = Instantiate(gameObject, transform.position + Random.insideUnitSphere * radius, Random.rotation, _parent.transform);
              current.transform.position = new Vector3(current.transform.position.x, transform.position.y + spawnPointUp, current.transform.position.z);
            }
        }
    }

    IEnumerator Precipice()
    {
        yield return new WaitForSeconds(_disappearanceTime/2);
        _collider.enabled = false;
        yield return new WaitForSeconds(_disappearanceTime/2);
        _rigidbody.isKinematic = true;
        _isDead = false;
        _collider.enabled = true;
        gameObject.SetActive(false);
        Died?.Invoke();
    }
}