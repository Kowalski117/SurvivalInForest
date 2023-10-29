using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnAnimals : MonoBehaviour
{
    [SerializeField] private PlayerHealth _player;
    [SerializeField] private List<SpawnPointAnimals> _listSpawnPoint;

    private void Start()
    {
        _listSpawnPoint = new List<SpawnPointAnimals>(GetComponentsInChildren<SpawnPointAnimals>());
        StartCoroutine(Wait());
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(3f);
        
        for (int i = 0; i < _listSpawnPoint.Count; i++)
        {
            _listSpawnPoint[i].Spawn(_player);
        }
    }
}