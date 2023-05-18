using UnityEngine;

public class Enemy : Animals
{
    [SerializeField] private float _attack;
    [SerializeField] private GameObject _attackColider;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<IDamagable>() != null)
        {
            other.GetComponent<IDamagable>().TakeDamage(_attack);
        }
    }
}
