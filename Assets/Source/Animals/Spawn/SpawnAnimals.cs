using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnAnimals : MonoBehaviour
{
    [SerializeField] private Animals _rabbit;
    [SerializeField] private Animals _deer;
    [SerializeField] private Animals _wolf;
    [SerializeField] private Animals _bear;
    [SerializeField] private PlayerHealth _player;

    [SerializeField] private List<SpawnPointAnimals> _listSpawnPoint;
    
    private float _distanceToPlayer;

    private void Start()
    {
        for (int i = 0; i < gameObject.GetComponentsInChildren<SpawnPointAnimals>().Length; i++)
        {
            _listSpawnPoint.Add(GetComponentsInChildren<SpawnPointAnimals>()[i]);
        }
        for (int i = 0; i < _listSpawnPoint.Count; i++)
        {
            _distanceToPlayer = _listSpawnPoint[i].DistanceToPlayer;
            float distance = (_listSpawnPoint[i].transform.position - _player.transform.position).magnitude;
            
            if (distance > _distanceToPlayer)
            {
                Spawn(_listSpawnPoint[i].transform,_listSpawnPoint[i].GetComponent<SpawnPointAnimals>().Animals);
            }
        }
    }

    private void Spawn(Transform spawnPoint,TypeAnimals typeAnimals)
    {
        Animals currentAnimals = null;
        int range;
        switch (typeAnimals)
        {
            case TypeAnimals.Bear:
                currentAnimals = _bear;
                break;
            case TypeAnimals.Wolf:
                currentAnimals = _wolf;
                break;
            case TypeAnimals.Deer:
                currentAnimals = _deer;
                break;
            case TypeAnimals.Rabbit:
                currentAnimals = _rabbit;
                break;
            case TypeAnimals.Mob:
                range = Random.Range(0, 2);

                if (range == 0)
                {
                    currentAnimals = _deer;
                }
                else
                {
                    currentAnimals = _rabbit;
                }
                break;
            case TypeAnimals.Enemy:
                range = Random.Range(0, 2);

                if (range == 0)
                {
                    currentAnimals = _wolf;
                }
                else
                {
                    currentAnimals = _bear;
                }
                break;
        }
        
        Instantiate(currentAnimals, spawnPoint.position, quaternion.identity, spawnPoint);
    }
}
public enum TypeAnimals
{
    Mob,
    Enemy,
    Bear,
    Wolf,
    Deer,
    Rabbit
}
