using BehaviorDesigner.Runtime;
using UnityEngine;
using UnityEngine.AI;

public class TrueAndFalseComponentsDistanceToPlayer : MonoBehaviour
{
    [SerializeField] private Animals _animals;
    [SerializeField] private Animator _animator;
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private BehaviorTree _behaviour;
    [SerializeField] private SetAnimationAnimals _setAnimationAnimals;

    private void Awake()
    {
        ComponentsEnabledFalse();
    }
    
    public void ComponentsEnabledTrue()
    {
        if (_animals.IsDead!=true)
        {
            _setAnimationAnimals.enabled = true;
            _animator.enabled = true;
            _agent.enabled = true;
            _behaviour.enabled = true;
        }
    }

    public void ComponentsEnabledFalse()
    {
        _behaviour.enabled = false;
        _agent.enabled = false;
        _animator.enabled = false;
        _setAnimationAnimals.enabled = false;
    }
}
