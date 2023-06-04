using System;
using System.Collections;
using BehaviorDesigner.Runtime;
using UnityEngine;
using UnityEngine.AI;

public abstract class Animals : MonoBehaviour, IDamagable
{
    [SerializeField] private float _healh;
    [SerializeField] private float _armor;
    private BehaviorTree _behaviorTree;
    private NavMeshAgent _agent;
    private bool _isDead = false;

    public bool IsDead => _isDead;

    public event Action Died;

    private void Start()
    {
        _behaviorTree = GetComponent<BehaviorTree>();
        _agent = GetComponent<NavMeshAgent>();
    }
    
    public void TakeDamage(float damage)
    {
        float currentDamage = damage - _armor;
        
        if (currentDamage > 0)
        {
            _healh -= currentDamage;
            if(_healh<=0)
                Die();
        }
    }

    public void Die()
    {
        _isDead = true;
        _behaviorTree.enabled = false;
        _agent.enabled = false;
        Died?.Invoke();
        StartCoroutine(Precipice());
    }

    IEnumerator Precipice()
    {
        yield return new WaitForSeconds(30f);
        Destroy(gameObject);
    }
}