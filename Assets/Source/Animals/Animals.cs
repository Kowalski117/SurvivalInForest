using System;
using System.Collections;
using BehaviorDesigner.Runtime;
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
    [SerializeField] private GameObject _blood;

    private float _radiusSpawnLoots = 1;
    private float _spawnLootUp = 0.5f;
    private BehaviorTree _behaviorTree;
    private NavMeshAgent _agent;
    private bool _isDead = false;
    private Coroutine _coroutineOverTimeDamage;
    private Collider _collider;
    private float _maxHealth;

    public float Health => _healh;
    public float MaxHealth => _maxHealth;
    public bool IsDead => _isDead;

    public event Action Died;

    private void Start()
    {
        _behaviorTree = GetComponent<BehaviorTree>();
        _agent = GetComponent<NavMeshAgent>();
        _collider = GetComponent<Collider>();
        _maxHealth = _healh;
    }
    
    public void TakeDamage(float damage,float overTimeDamage)
    {
        float currentDamage = damage - _armor;

        _behaviorTree.SendEvent("TakeDamage");
        
        if (currentDamage >= 0)
        {
            _healh -= currentDamage;
            if(_healh<=0)
                Die();
            else
            {
                if (_coroutineOverTimeDamage == null && overTimeDamage > 0)
                    _coroutineOverTimeDamage = StartCoroutine(TakeOverTimeDamage(overTimeDamage));
            }
        }
    }

    public void Die()
    {
        _collider.enabled = false;
        _behaviorTree.enabled = false;
        _agent.enabled = false;
        SpawnItem(_meat,_radiusSpawnLoots,_spawnLootUp,_numberMeat);
        _isDead = true;

        Died?.Invoke();
        StartCoroutine(Precipice());
    }
    
    public void SpawnItem(ItemPickUp itemPickUp, float radius, float spawnPointUp,int count)
    {
        if (_isDead == false)
        {
            for (int i = 0; i < count; i++)
            {
                Vector3 position = transform.position + Random.insideUnitSphere * radius;
                SpawnLoots.Spawn(itemPickUp,position,transform,false,spawnPointUp,false);
            }
        }
    }

    IEnumerator Precipice()
    {
        yield return new WaitForSeconds(30f);
        Destroy(gameObject);
    }

    IEnumerator TakeOverTimeDamage(float overTimeDamage)
    {
        float second = 1;
        
        _behaviorTree.SendEvent("TakeDamageOverTime");
        _blood.SetActive(true);
        
        while (_healh > 0)
        {
            yield return new WaitForSeconds(second);

            _healh-= overTimeDamage;
        }
        Die();
    }
}