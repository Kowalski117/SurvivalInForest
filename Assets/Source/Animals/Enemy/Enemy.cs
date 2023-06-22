using UnityEngine;

public class Enemy : Animals
{
    [SerializeField] private float _damage;
    [SerializeField] private GameObject _attackColider;
    [SerializeField] private float _overTimeDamage;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<IDamagable>() != null && other.GetComponent<Enemy>() == null)
        {
            other.GetComponent<IDamagable>().TakeDamage(_damage,_overTimeDamage);
        }
    }
}