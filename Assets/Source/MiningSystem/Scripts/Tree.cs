using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CapsuleCollider))]
public class Tree : Resource
{
    [SerializeField] private ItemPickUp _stick;
    [SerializeField] private int _numberStick;
    [SerializeField] private Transform _positionLeaves;
    [SerializeField] private ParticleSystem _leaves;

    private float _radiusSpawnStick = 2;
    private float _spawnPointUpStick = 2;
    private int _currentNumberStick;

    public override void OnEnable()
    {
        base.OnEnable();
        _currentNumberStick = _numberStick;
    }

    public override void TakeDamage(float damage, float overTimeDamage)
    {
        ParticleSystem leaves = Instantiate(_leaves, _positionLeaves.position, _positionLeaves.rotation, _positionLeaves);
        StartCoroutine(DeleteLeaves(leaves));

        int number = Random.Range(0, _currentNumberStick + 1);
        _currentNumberStick -= number;
        base.SpawnItem(_stick, _radiusSpawnStick, _spawnPointUpStick, number);
        base.TakeDamage(damage, overTimeDamage);
    }

    IEnumerator DeleteLeaves(ParticleSystem leaves)
    {
        float time = 3f;
        yield return new WaitForSeconds(time);
        Destroy(leaves.gameObject);
    }
}