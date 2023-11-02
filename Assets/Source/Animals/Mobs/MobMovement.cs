using UnityEngine;

public class MobMovement : SetMovement
{
    [SerializeField] private Animals _animals;
    
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
}