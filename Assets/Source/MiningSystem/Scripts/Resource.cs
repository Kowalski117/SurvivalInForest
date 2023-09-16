using System.Collections;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using Action = System.Action;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public abstract class Resource : MonoBehaviour, IDamagable
{
    [SerializeField] private float _maxHealth = 100;
    [SerializeField] private ToolType _extractionType;
    [SerializeField] private GameObject _loot;
    [SerializeField] private int _lootsCount;
    [SerializeField] private ParticleSystem _takeDamage;
    [SerializeField] private GameObject _parent;

    protected Collider Сollider;
    protected Rigidbody Rigidbody;

    private float _curenntHealth;
    private float _radiusSpawnLoots = 1;
    private float _spawnLootUp = 1;
    private float _disappearanceTime = 5;
    private bool _isDead = false;
    
    public event Action Died;
    public event Action Disappeared;

    public float Health => _curenntHealth;
    public float MaxHealth => _maxHealth;
    public ToolType ExtractionType => _extractionType;

    public virtual void Start()
    {
        Rigidbody = GetComponent<Rigidbody>();
        Сollider = GetComponent<Collider>();
    }

    public virtual void OnEnable()
    {
        _curenntHealth = _maxHealth;
    }

    public virtual void TakeDamage(float damage, float overTimeDamage)
    {
        _takeDamage.Play();
        _curenntHealth -= (int) damage;
        
        if (_curenntHealth <= 0 & _isDead == false)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        _curenntHealth = 0;
        Rigidbody.isKinematic = false;
        SpawnLoot(_loot, _radiusSpawnLoots, _spawnLootUp, _lootsCount);
        _isDead = true;
        StartCoroutine(Precipice());
    }

    public virtual void SpawnLoot(GameObject gameObject, float radius, float spawnPointUp,int count)
    {
        if (_isDead == false)
        { 
            for (int i = 0; i < count; i++)
            {
                Debug.Log("spawn" + gameObject +"количество" + count);
              GameObject current = Instantiate(gameObject, transform.position + Random.insideUnitSphere * radius, Random.rotation, _parent.transform);
              current.transform.position = new Vector3(current.transform.position.x, transform.position.y + spawnPointUp, current.transform.position.z);
              Debug.Log("цикл"+i);
            }
        }
    }

    public void ToggleCollider(bool isActive)
    {
        Сollider.enabled = isActive;
    }

    public virtual void Enable() { }

    protected void DiedEvent()
    {
        Died?.Invoke();
    }
    
    protected void DisappearedEvent()
    {
        _isDead = false;
        Disappeared?.Invoke();
    }

    protected void DiedReset()
    {
        _curenntHealth = 0;
        Rigidbody.isKinematic = false;
        _isDead = true;
    }

    IEnumerator Precipice()
    {
        DiedEvent();
        yield return new WaitForSeconds(_disappearanceTime/2);
        Сollider.enabled = false;
        yield return new WaitForSeconds(_disappearanceTime/2);
        Rigidbody.isKinematic = true;
        Сollider.enabled = true;
        gameObject.SetActive(false);
        DisappearedEvent();
    }
}