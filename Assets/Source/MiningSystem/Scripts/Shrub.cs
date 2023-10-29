using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Shrub : MonoBehaviour
{
    [SerializeField] private ItemPickUp _item;
    [SerializeField] private int _countItems;
    [SerializeField] private float _timeRespawnItem;
    
    private const float _radiusSpawn= 0.5f;
    private const float _spawnPointUp= 0.5f;

    private int _currentCountItem=0;
    
    private void Start()
    {
      SpawnItem(_countItems);
    }

    private void OnDisable()
    {
        List <ItemPickUp> listChildren =new List<ItemPickUp>(GetComponentsInChildren<ItemPickUp>().ToList());
        
        for (int i = 0; i < listChildren.Count; i++)
        {
            listChildren[i].DestroyItem -= DestroyItem;
        }
    }

    private void SpawnItem(int count)
    {
        ItemPickUp itemPickUp;
        for (int i = 0; i < count; i++)
        {
            Vector3 position = (transform.position + Random.insideUnitSphere * _radiusSpawn);
            itemPickUp = SpawnLoots.Spawn(_item,position,transform,true,_spawnPointUp,true);
            itemPickUp.DestroyItem += DestroyItem;
        }
    }

    private void DestroyItem()
    {
        _currentCountItem++;
        
        if (_countItems == _currentCountItem)
        {
            _currentCountItem = 0;
            StartCoroutine(Wait());
        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(_timeRespawnItem);
       SpawnItem(_countItems);
    }
}
