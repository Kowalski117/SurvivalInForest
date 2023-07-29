using UnityEngine;

public class BrokenObject : MonoBehaviour, IDamagable
{
    [SerializeField] private float _maxEndurance;
    [SerializeField] private GameObject _brokenObject;

    private float _currentEndurance;
    private BoxCollider _collider;
    private Animator _animator;

    public float MaxEndurance => _maxEndurance;
    public float Endurance => _currentEndurance;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _collider = GetComponent<BoxCollider>();
        _currentEndurance = _maxEndurance;
    }

    public void Die()
    {
        _currentEndurance = 0;
        _brokenObject.SetActive(false);
        _collider.enabled = false;
    }

    public void TakeDamage(float damage, float overTimeDamage)
    {
        _currentEndurance -= damage;

        if (_currentEndurance <= 0)
            Die();
    }
}
