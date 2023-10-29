using UnityEngine;

public class Shrub : MonoBehaviour
{
    [SerializeField] private ItemPickUp _item;
    [SerializeField] private int _countItems;

    private const float _radiusSpawn = 0.5f;
    private const float _spawnPointUp = 0.5f;

    private void Start()
    {
      SpawnItem(_countItems);
    }

    private void SpawnItem(int count)
    {
        for (int i = 0; i < count; i++)
        {
            Vector3 position = (transform.position + Random.insideUnitSphere * _radiusSpawn);
            SpawnLoots.Spawn(_item,position,transform,true,_spawnPointUp,true);
        }
    }
}
