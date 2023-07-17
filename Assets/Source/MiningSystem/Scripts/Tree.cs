using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public class Tree : Resource
{
    [SerializeField] private GameObject _stick;
    [SerializeField] private int _numberStick;

    private float _radiusSpawnStick = 2;
    private float _spawnPointUpStick = 2;
    private int _currentNumberStick;

    public override void OnEnable()
    {
        _currentNumberStick = _numberStick;
        base.OnEnable();
    }

    public override void TakeDamage(float damage, float overTimeDamage)
    {
        int number = Random.Range(0, _currentNumberStick+1);
        _currentNumberStick -= number;
        base.SpawnLoot(_stick,_radiusSpawnStick,_spawnPointUpStick,number);
        base.TakeDamage(damage, overTimeDamage);
    }
}
