using UnityEngine;
using UnityEngine.AI;

public class AnimationMobs : MonoBehaviour
{
    NavMeshAgent _agent;
    private Animator _animator;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        _animator.SetFloat("Speed",(_agent.velocity.magnitude/10));
    }
}
