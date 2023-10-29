using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public enum TypeAnimals
{
    Mob,
    Enemy,
    Bear,
    Wolf,
    Deer,
    Rabbit
}

public class SpawnPointAnimals : MonoBehaviour
{
    [SerializeField] private TypeAnimals _animals;
    [SerializeField] private float _distanceToPlayer;
    [SerializeField] private float _timeRespawnAnimal;
    [SerializeField] private Animals _rabbit;
    [SerializeField] private Animals _deer;
    [SerializeField] private Animals _wolf;
    [SerializeField] private Animals _bear;

    private Animals _currentAnimal;
    private PlayerHealth _player;

    private void OnDisable()
    {
        _currentAnimal.DestroyAnimal -= DestroyAnimal;
    }

    public void Spawn(PlayerHealth playerHealth)
    {
        float secondWait = 10f;
        
        if (_player == null)
        {
            _player = playerHealth;
        }

        float distance = (transform.position - _player.transform.position).magnitude;

        if (distance > _distanceToPlayer)
        {
            SpawnAnimal();
        }
        else
        {
            StartCoroutine(Wait(secondWait));
        }
    }

    private void SpawnAnimal()
    {
        
        Animals currentAnimalsPrefab = null;
        int range;
        switch (_animals)
        {
            case TypeAnimals.Bear:
                currentAnimalsPrefab = _bear;
                break;
            case TypeAnimals.Wolf:
                currentAnimalsPrefab = _wolf;
                break;
            case TypeAnimals.Deer:
                currentAnimalsPrefab = _deer;
                break;
            case TypeAnimals.Rabbit:
                currentAnimalsPrefab = _rabbit;
                break;
            case TypeAnimals.Mob:
                range = Random.Range(0, 2);

                if (range == 0)
                {
                    currentAnimalsPrefab = _deer;
                }
                else
                {
                    currentAnimalsPrefab = _rabbit;
                }

                break;
            case TypeAnimals.Enemy:
                range = Random.Range(0, 2);

                if (range == 0)
                {
                    currentAnimalsPrefab = _wolf;
                }
                else
                {
                    currentAnimalsPrefab = _bear;
                }

                break;
        }

        _currentAnimal = Instantiate(currentAnimalsPrefab, gameObject.transform.position, quaternion.identity,
            gameObject.transform);
        _currentAnimal.DestroyAnimal += DestroyAnimal;
    }


    private void DestroyAnimal()
    {
        StartCoroutine(Wait(_timeRespawnAnimal));
    }

    private void OnDrawGizmos()
    {
        if (_animals == TypeAnimals.Bear || _animals == TypeAnimals.Wolf || _animals == TypeAnimals.Enemy)
        {
            Gizmos.color = Color.red;
        }
        else
        {
            Gizmos.color = Color.green;
        }

        Gizmos.DrawWireSphere(transform.position, _distanceToPlayer);
    }

    IEnumerator Wait(float timeSecond)
    {
        yield return new WaitForSeconds(timeSecond);
        Spawn(_player);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerHealth>())
        {
            if (other.GetComponent<PlayerHealth>().IsRespawned)
            {
                Destroy(_currentAnimal.gameObject);
                Spawn(_player);
            }
        }
    }
}