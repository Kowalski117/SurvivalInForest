using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(AnimationAnimals))]
[RequireComponent(typeof(NavMeshAgent))]
public abstract class AnimalsMovement : MonoBehaviour
{
    [SerializeField] private float _speedRun;
    [SerializeField] private float _speedWalk;
    
    private NavMeshAgent _agent;
    private Vector3 _spawnPoint;
    private bool _isRuning = false;
    private AnimationAnimals _animationAnimals;

    public NavMeshAgent Agent => _agent;
    public Vector3 SpawnPoint => _spawnPoint;
    public bool IsRuning => _isRuning;
    

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animationAnimals = GetComponent<AnimationAnimals>();
        _spawnPoint = transform.position;
    }
    
    public void Walk(Vector3 point)
    {
        _agent.speed = _speedWalk;
        _agent.SetDestination(point);
        _isRuning = false;
    }

    public void Run(Vector3 point)
    {
        _isRuning = true;
        _agent.speed = _speedRun;
        _agent.SetDestination(point);
    }

    public void Eat()
    {
        _animationAnimals.Eat();
    }

    public void Sit()
    {
        _animationAnimals.Sit();
    }

    public void Attack()
    {
        _animationAnimals.Attack();
    }
}
