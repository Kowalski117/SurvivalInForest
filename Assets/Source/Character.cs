using UnityEngine;

public class Character : MonoBehaviour, IDamagable
{
    [SerializeField] private float _health;
    public void TakeDamage(float damage)
    {
        _health -= damage;
        if (_health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
