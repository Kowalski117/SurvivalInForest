using UnityEngine;

public class EnemyMovement : SetMovement
{
    [SerializeField] private float _attackDistans;
    [SerializeField] private Animals _animals;
  
    public float AttackDistans => _attackDistans;
    
    private void OnEnable()
    {
        _animals.Died += Death;
    }

    private void OnDisable()
    {
        _animals.Died -= Death;
    }
    
    public void Death()
    {
        SetAnimation.Death();
    }
    
    public void Attack()
    {
        SetAnimation.Attack();
    }
}
