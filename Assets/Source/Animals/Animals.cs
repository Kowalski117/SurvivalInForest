using System;
using System.Collections;
using BehaviorDesigner.Runtime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Collider))]
public abstract class Animals : MonoBehaviour, IDamagable
{
    [SerializeField] private ItemPickUp _meat;
    [SerializeField] private int _numberMeat;
    [SerializeField] private float _healh;
    [SerializeField] private float _armor;
    [SerializeField] private ParticleSystem _blood;

    private float _radiusSpawnLoots = 1;
    private float _spawnLootUp = 0.5f;
    private BehaviorTree _behaviorTree;
    private NavMeshAgent _agent;
    private bool _isDead;
    private Coroutine _coroutineOverTimeDamage;
    private Collider _collider;
    private float _maxHealth;
    private Rigidbody _rigidbody;

    public float Health => _healh;
    public float MaxHealth => _maxHealth;
    public bool IsDead => _isDead;

    public event Action Died;

    private void Start()
    {
        _behaviorTree = GetComponent<BehaviorTree>();
        _agent = GetComponent<NavMeshAgent>();
        _collider = GetComponent<Collider>();
        _rigidbody = GetComponent<Rigidbody>();
        _maxHealth = _healh;
    }

    public void TakeDamage(float damage, float overTimeDamage)
    {
        float currentDamage = damage - _armor;

        _behaviorTree.SendEvent("TakeDamage");

        if (currentDamage >= 0)
        {
            _healh -= currentDamage;
            if (_healh <= 0)
                Die();
            else
            {
                if (overTimeDamage > 0)
                {
                    if (_coroutineOverTimeDamage != null)
                    {
                        StopCoroutine(_coroutineOverTimeDamage);
                    }
                    _coroutineOverTimeDamage = StartCoroutine(TakeOverTimeDamage(overTimeDamage));
                }
            }
        }
    }

    public void Die()
    {
        _behaviorTree.enabled = false;
        _agent.enabled = false;
        SpawnItem(_meat, _radiusSpawnLoots, _spawnLootUp, _numberMeat);
        _isDead = true;

        Died?.Invoke();
        StartCoroutine(Precipice());
    }

    public void SpawnItem(ItemPickUp itemPickUp, float radius, float spawnPointUp, int count)
    {
        if (_isDead == false)
        {
            for (int i = 0; i < count; i++)
            {
                Vector3 position = transform.position + Random.insideUnitSphere * radius;
                SpawnLoots.Spawn(itemPickUp, position, transform, false, spawnPointUp, false);
            }
        }
    }

    IEnumerator Precipice()
    {
        float second = 10f;
        yield return new WaitForSeconds(second);
        _collider.enabled = false;
        yield return new WaitForSeconds(second);
        _rigidbody.isKinematic = false;
        yield return new WaitForSeconds(second / 5);
        Destroy(gameObject);
    }

    IEnumerator TakeOverTimeDamage(float overTimeDamage)
    {
        int duration = 5;
        float second = 1;
        
        _blood.Play();
        
        for (int i = duration; i > 0; i--)
        {
            yield return new WaitForSeconds(second);
            _healh -= overTimeDamage;
            _behaviorTree.SendEvent("TakeDamageOverTime");

            if (_healh <= 0)
            {
                Die();
                break;
            }
        }
        yield return new WaitForSeconds(duration);
        _blood.Stop();
    }
}