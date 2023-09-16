using UnityEngine;

public class Shrub : MonoBehaviour
{
    [SerializeField] private GameObject _loot;
    [SerializeField] private int _countLoots;
    
    private const float _radiusSpawn= 0.5f;
    private const float _spawnPointUp= 0.5f;

    private void Start()
    {
      SpawnLoots(_countLoots);
    }

    private void SpawnLoots(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject current = Instantiate(_loot, transform.position + Random.insideUnitSphere * _radiusSpawn, transform.rotation,transform);
            current.transform.position = new Vector3(current.transform.position.x, current.transform.position.y + _spawnPointUp, current.transform.position.z);
            current.GetComponent<ItemPickUp>().GenerateNewID();
        }
    }
}
