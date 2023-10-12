using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tree : Resource
{
    [SerializeField] private Transform _positionLeaves;
    [SerializeField] private ParticleSystem _leaves;
    [SerializeField] private bool _isTreeHasLeaves;
    [SerializeField] private List<ItemPickUp> _damageLoots;

    private float _radiusSpawn = 3;
    private float _spawnPointUp = 2;
    [SerializeField] private List<ItemPickUp> _currentDamageLoots;

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
    
    private void CreateParticleLeaves()
    {
        ParticleSystem leaves = Instantiate(_leaves, _positionLeaves.position, _positionLeaves.rotation, _positionLeaves);
        StartCoroutine(DeleteLeaves(leaves));
    }

    IEnumerator DeleteLeaves(ParticleSystem leaves)
    {
        float time = 3f;
        yield return new WaitForSeconds(time);
        Destroy(leaves.gameObject);
    }
}