using System;
using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using UnityEngine;
using UnityEngine.AI;

public abstract class Animals : MonoBehaviour, IDamagable
{
    [SerializeField] private List<GameObject> _loots;
    [SerializeField] private float _healh;
    [SerializeField] private float _armor;
    [SerializeField] private GameObject _blood;

    private BehaviorTree _behaviorTree;
    private NavMeshAgent _agent;
    private bool _isDead = false;
    private Coroutine _coroutineOverTimeDamage;

    public bool IsDead => _isDead;

    public event Action Died;

    private void Start()
    {
        _behaviorTree = GetComponent<BehaviorTree>();
        _agent = GetComponent<NavMeshAgent>();
        
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
        _isDead = true;
        _behaviorTree.enabled = false;
        _agent.enabled = false;
        
        
        foreach (var loot in _loots)
        {
           GameObject currentLoot = Instantiate(loot);
           currentLoot.transform.position = transform.position;
        }
        Died?.Invoke();
        StartCoroutine(Precipice());
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