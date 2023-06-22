using UnityEngine;

public class EnemyMovement : AnimalsMovement
{
    [SerializeField] private float _attackDistans;

    public float AttackDistans => _attackDistans;

    public void Attack()
    {
        AnimationAnimals.Attack();
    }
}
