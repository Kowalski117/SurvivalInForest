using UnityEngine;

public class Enemy : Animals
{
    [SerializeField] private float _damage;
    [SerializeField] private float _overTimeDamage;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<IDamagable>() != null && (other.GetComponent<IDamagable>() == other.GetComponent<PlayerHealth>() || other.GetComponent<IDamagable>() == other.GetComponent<Mob>()))
        {
            other.GetComponent<IDamagable>().TakeDamage(_damage,_overTimeDamage);
        }
        else if (other.GetComponent<IDamagable>() != null)
        {
            other.GetComponent<IDamagable>().TakeDamage(0,0);
        }
    }
}