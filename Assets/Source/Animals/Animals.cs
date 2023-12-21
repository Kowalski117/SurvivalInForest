using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using PixelCrushers.QuestMachine;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using Random = UnityEngine.Random;

[RequireComponent(typeof(BehaviorTree), typeof(Rigidbody), typeof(NavMeshAgent))]
public abstract class Animals : MonoBehaviour, IDamagable
{
    [SerializeField] private List<ItemPickUp> _loots;
    [SerializeField] private float _healh;
    [SerializeField] private float _armor;
    [SerializeField] private ParticleSystem _blood;
    [SerializeField] private string _name;

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
    private QuestControl _questControl;

    public event UnityAction DestroyAnimal;
    public event UnityAction Died;
    
    public float Health => _healh;
    public float MaxHealth => _maxHealth;
    public bool IsDead => _isDead;
    
    private void Start()
    {
        _behaviorTree = GetComponent<BehaviorTree>();
        _rigidbody = GetComponent<Rigidbody>();
        _agent = GetComponent<NavMeshAgent>();
        _questControl = GetComponentInParent<QuestControl>();
        _maxHealth = _healh;
    }

    public void TakeDamage(float damage, float overTimeDamage)
    {
        float currentDamage = damage - _armor;

        _behaviorTree.SendEvent(_takeDamage);

        if (_isDead)
        {
            SpawnLoot();
        }

        if (currentDamage >= 0 && _isDead != true)
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
        _isDead = true;
        Died?.Invoke();
        _questControl.SendToMessageSystem(MessageConstants.Dead + _name);
    }

    private void SpawnLoot()
    {
        if (_loots.Count > 0)
        {
            int randomIndex = Random.Range(0, _loots.Count);
            Vector3 position = transform.position + Random.insideUnitSphere * _radiusSpawnLoots;
            SpawnLoots.Spawn(_loots[randomIndex], position, transform, false, _spawnLootUp, false);
            _loots.RemoveAt(randomIndex);
        }

        if (_loots.Count == 0)
        {
            StartCoroutine(DestroyObject());
        }

    }

    private void CreateBlood()
    {
        if (_currentBlood == null)
        {
            _currentBlood = Instantiate(_blood, transform.position, quaternion.identity, transform);
        }
    }

    IEnumerator DestroyObject()
    {
        float second = 2f;
        _rigidbody.isKinematic = false;
        yield return new WaitForSeconds(second);
        DestroyAnimal?.Invoke();
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