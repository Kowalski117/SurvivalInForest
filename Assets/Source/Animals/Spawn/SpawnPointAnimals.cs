using System;
using UnityEngine;

public class SpawnPointAnimals : MonoBehaviour
{
    [SerializeField] private TypeAnimals _animals;
    [SerializeField] private float _distanceToPlayer = 30;
    
    public TypeAnimals Animals => _animals;

    public float DistanceToPlayer => _distanceToPlayer;



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
}

