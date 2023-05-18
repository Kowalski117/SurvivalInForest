using System;
using BehaviorDesigner.Runtime;
using UnityEngine;
using UnityEngine.AI;

public abstract class Animals : MonoBehaviour, IDamagable
{
    [SerializeField] private float _healh;
    [SerializeField] private float _armor;
    private BehaviorTree _behaviorTree;
    private MeshCollider _collider;
    private NavMeshAgent _agent;

    public event Action Died;

    private void Start()
    {
        _behaviorTree = GetComponent<BehaviorTree>();
        _collider = GetComponent<MeshCollider>();
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
        _collider.enabled = false;
        _behaviorTree.enabled = false;
        _agent.enabled = false;
        Died?.Invoke();
    }
}