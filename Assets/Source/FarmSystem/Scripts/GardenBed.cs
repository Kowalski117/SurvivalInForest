using DG.Tweening;
using System.Collections;
using UnityEngine;

public class GardenBed : MonoBehaviour
{
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private float _spawnTime = 10f;

    private ItemPickUp _currentItem;

    public void Init(InventoryItemData inventoryItemData)
    {
        if(_currentItem == null && inventoryItemData != null)
        {
            _currentItem = Instantiate(inventoryItemData.ItemPrefab, _spawnPoint.position, Quaternion.identity, transform);
            _currentItem.gameObject.transform.localScale = new Vector3(0, 0, 0);
            _currentItem.TurnOff();
            StartCoroutine(SpawnOverTime());
        }
    }

    IEnumerator SpawnOverTime()
    {
        _currentItem.transform.DOScale(new Vector3(1, 1, 1), _spawnTime);
        yield return new WaitForSeconds(_spawnTime);
        _currentItem.Enable();
        _currentItem = null;
    }
}
