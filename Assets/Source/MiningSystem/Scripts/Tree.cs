using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tree : Resource
{
    [SerializeField] private Transform _positionLeaves;
    [SerializeField] private ParticleSystem _leaves;
    [SerializeField] private bool _isTreeHasLeaves;
    [SerializeField] private List<ItemPickUp> _damageLoots;
    [SerializeField] private List<ItemPickUp> _currentDamageLoots;

    private float _radiusSpawn = 3;
    private float _spawnPointUp = 2;
    private float _delay = 3;

    public override void OnEnable()
    {
        base.OnEnable();
        _currentDamageLoots.AddRange(_damageLoots);
    }

    public override void TakeDamage(float damage, float overTimeDamage)
    {
        int count = Random.Range(0, _currentDamageLoots.Count+1);
        
        if (_isTreeHasLeaves)
        {
            CreateParticleLeaves();
        }

        for (int i = 0; i < count; i++)
        {
            int number = Random.Range(0, _currentDamageLoots.Count);
            base.SpawnItem(_currentDamageLoots[number], _radiusSpawn, _spawnPointUp);
            _currentDamageLoots.RemoveAt(number);
        }
        
        base.TakeDamage(damage, overTimeDamage);
    }

    public override void Die()
    {
        for (int i = 0; i < _currentDamageLoots.Count; i++)
        {
            base.SpawnItem(_currentDamageLoots[i],_radiusSpawn,_spawnPointUp);
        }
        
        base.Die();
    }

    private void CreateParticleLeaves()
    {
        ParticleSystem leaves = Instantiate(_leaves, _positionLeaves.position, _positionLeaves.rotation, _positionLeaves);
        StartCoroutine(DeleteLeaves(leaves));
    }

    IEnumerator DeleteLeaves(ParticleSystem leaves)
    {
        yield return new WaitForSeconds(_delay);

        if (leaves != null)
        {
            Destroy(leaves.gameObject);
        }
    }
}