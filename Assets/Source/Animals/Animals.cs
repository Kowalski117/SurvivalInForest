using System;
using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

[RequireComponent(typeof(BehaviorTree),typeof(Rigidbody),typeof(NavMeshAgent))]
public abstract class Animals : MonoBehaviour, IDamagable
{
    [SerializeField] private List<ItemPickUp> _loots;
    [SerializeField] private float _healh;
    [SerializeField] private float _armor;
    [SerializeField] private ParticleSystem _blood;

    private float _radiusSpawnLoots = 1;
    private float _spawnLootUp = 0.5f;
    private BehaviorTree _behaviorTree;
    private NavMeshAgent _agent;
    private bool _isDead;
    private Coroutine _coroutineOverTimeDamage;
    private float _maxHealth;
    private Rigidbody _rigidbody;
    private ParticleSystem _currentBlood;
    private string _takeDamage = "TakeDamage";
    private string _takeDamageOverTime = "TakeDamageOverTime";

    public float Health => _healh;
    public float MaxHealth => _maxHealth;
    public bool IsDead => _isDead;

    public event Action Died;

    private void Start()
    {
        _behaviorTree = GetComponent<BehaviorTree>();
        _rigidbody = GetComponent<Rigidbody>();
        _agent = GetComponent<NavMeshAgent>();
        _maxHealth = _healh;
    }

    public void TakeDamage(float damage, float overTimeDamage)
    {
        float currentDamage = damage - _armor;

        _behaviorTree.SendEvent(_takeDamage);

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

        for (int i = 0; i < _loots.Count; i++)
        {
            Vector3 position = transform.position + Random.insideUnitSphere * _radiusSpawnLoots;
            SpawnLoots.Spawn(_loots[i], position, transform, false, _spawnLootUp, false);
        }
        
        _isDead = true;
        Died?.Invoke();
        StartCoroutine(Precipice());
    }
    
    private void CreateBlood()
    {
        if (_currentBlood == null)
        {
            _currentBlood = Instantiate(_blood, transform.position, quaternion.identity, transform);
        }   
    }

    IEnumerator Precipice()
    {
        float second = 10f;
        yield return new WaitForSeconds(second);
        _rigidbody.isKinematic = false;
        yield return new WaitForSeconds(second / 5);
        Destroy(gameObject);
    }

    IEnumerator TakeOverTimeDamage(float overTimeDamage)
    {
        int duration = 5;
        float second = 1;
        CreateBlood();
        _currentBlood.Play();
        
        for (int i = duration; i > 0; i--)
        {
            yield return new WaitForSeconds(second);
            _healh -= overTimeDamage;
            _behaviorTree.SendEvent(_takeDamageOverTime);

            if (_healh <= 0 && _isDead == false)
            {
                Die();
                break;
            }
        }
        yield return new WaitForSeconds(duration);
        _currentBlood.Stop();
    }
}