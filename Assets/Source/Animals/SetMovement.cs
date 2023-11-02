using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(SetAnimation))]
[RequireComponent(typeof(NavMeshAgent))]
public abstract class SetMovement : MonoBehaviour
{
    [SerializeField] private float _speedRun;
    [SerializeField] private float _speedWalk;
    [SerializeField] private float _spawnPointRadius;
    [SerializeField] private float _fleeRadius;
    
    private NavMeshAgent _agent;
    private Vector3 _spawnPoint;
    private bool _isRuning;
    
    protected SetAnimation SetAnimation;

    public float FleeRadius => _fleeRadius;
    public NavMeshAgent Agent => _agent;
    public Vector3 SpawnPoint => _spawnPoint;
    public bool IsRuning => _isRuning;
    public float SpawnPointRadius => _spawnPointRadius;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        SetAnimation = GetComponent<SetAnimation>();
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
    
    public void TurnOffAnimations()
    {
        SetAnimation.TurnOffAnimations();
    }
}