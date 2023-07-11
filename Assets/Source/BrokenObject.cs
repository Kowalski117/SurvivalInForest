using UnityEngine;

public class BrokenObject : MonoBehaviour, IDamagable
{
    [SerializeField] private float _endurance;
    [SerializeField] private GameObject _brokenObject;

    private BoxCollider _collider;
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _collider = GetComponent<BoxCollider>();
    }

    public void Die()
    {
        _endurance = 0;
        _brokenObject.SetActive(false);
        _collider.enabled = false;
    }

    public void TakeDamage(float damage, float overTimeDamage)
    {
        _endurance -= damage;

        if (_endurance <= 0)
            Die();
    }
}
