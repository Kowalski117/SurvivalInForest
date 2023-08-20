using DG.Tweening;
using System.Collections;
using UnityEngine;

public class GardenBed : MonoBehaviour
{
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private float _spawnTime = 10f;

    private ObjectPickUp _currentItem;

    public bool StartGrowingSeed(InventoryItemData inventoryItemData)
    {
        if(_currentItem == null && inventoryItemData != null && inventoryItemData is SeedItemData seedItemData)
        {
            _currentItem = Instantiate(seedItemData.ObjectPickUp, _spawnPoint.position, Quaternion.identity, transform);
            _currentItem.gameObject.transform.localScale = new Vector3(0, 0, 0);
            _currentItem.TurnOff();
            StartCoroutine(SpawnOverTime(seedItemData.GrowthTime));
            return true;
        }
        return false;
    }

    private IEnumerator SpawnOverTime(float time)
    {
        _currentItem.transform.DOScale(new Vector3(1, 1, 1), time);
        yield return new WaitForSeconds(time);
        _currentItem.Enable();
        _currentItem = null;
    }
}
