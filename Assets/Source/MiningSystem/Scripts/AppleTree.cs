using UnityEngine;

public class AppleTree : Tree
{
    [SerializeField] private ItemPickUp _apple;
    [SerializeField] private int _numberApple;

    private float _radiusSpawnApple = 3;
    private float _spawnPointUp = 2;
    private int _currentNumberApple;

    public override void OnEnable()
    {
        _currentNumberApple = _numberApple;
        base.OnEnable();
    }

    public override void TakeDamage(float damage, float overTimeDamage)
    {
        int number = Random.Range(0, _currentNumberApple+1);
        _currentNumberApple -= number;
        base.SpawnItem(_apple,_radiusSpawnApple,_spawnPointUp);
        base.TakeDamage(damage, overTimeDamage);
    }
}
