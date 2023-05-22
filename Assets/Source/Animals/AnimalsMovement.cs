using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

[RequireComponent(typeof(AnimationAnimals))]
[RequireComponent(typeof(NavMeshAgent))]
public abstract class AnimalsMovement : MonoBehaviour
{
    [SerializeField] private Animals _animals;
    [SerializeField] private float _speedRun;
    [SerializeField] private float _speedWalk;
    [SerializeField] private float _detectionRadius;
    [SerializeField] private float _fleeRadius;
    [SerializeField] private float _spawnPointRadius;

    private NavMeshAgent _agent;
    private Vector3 _spawnPoint;
    private bool _isRuning = false;

    protected AnimationAnimals AnimationAnimals;

    public NavMeshAgent Agent => _agent;
    public Vector3 SpawnPoint => _spawnPoint;
    public bool IsRuning => _isRuning;
    public float DetectionRadius => _detectionRadius;
    public float FleeRadius => _fleeRadius;
    public float SpawnPointRadius => _spawnPointRadius;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        AnimationAnimals = GetComponent<AnimationAnimals>();
        _spawnPoint = transform.position;
    }

    private void OnEnable()
    {
        _animals.Died += Death;
    }

    private void OnDisable()
    {
        _animals.Died -= Death;
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

    public void Howl()
    {
        AnimationAnimals.Howl();
    }

    public void Eat()
    {
        AnimationAnimals.Eat();
    }

    public void Sit()
    {
        AnimationAnimals.Sit();
    }

    public void Death()
    {
        AnimationAnimals.Death();
    }
}