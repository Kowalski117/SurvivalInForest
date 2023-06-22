using UnityEngine;

[RequireComponent(typeof(UniqueID))]
public class ItemPickUp : MonoBehaviour
{
    [SerializeField] private float pickUpRadius = 1f;
    [SerializeField] private InventoryItemData _itemData;
    [SerializeField] private ItemPickUpSaveData _itemSaveData;

    private string _id;

    public InventoryItemData ItemData => _itemData;

    private void Awake()
    {
        _id = GetComponent<UniqueID>().Id;
        SaveLoad.OnLoadData += LoadGame;
        _itemSaveData = new ItemPickUpSaveData(_itemData, transform.position, transform.rotation);
    }

    private void Start()
    {
        SaveGameHandler.Data.ActiveItems.Add(_id, _itemSaveData);
    }

    public void PicUp()
    {
        SaveGameHandler.Data.CollectedItems.Add(_id);
        Destroy(this.gameObject); // переделать для оптимизации
    }

    private void LoadGame(SaveData data)
    {
        if (data.CollectedItems.Contains(_id))
            Destroy(this.gameObject);  //переделать для оптимизации
    }

    private void OnDestroy()
    {
        if (SaveGameHandler.Data.ActiveItems.ContainsKey(_id))
            SaveGameHandler.Data.ActiveItems.Remove(_id);
        SaveLoad.OnLoadData -= LoadGame;
    }
}

[System.Serializable]
public struct ItemPickUpSaveData
{
    [SerializeField] private InventoryItemData _itemData;
    [SerializeField] private Vector3 _position;
    [SerializeField] private Quaternion _rotation;

    public ItemPickUpSaveData(InventoryItemData itemData, Vector3 position, Quaternion rotation)
    {
        _itemData = itemData;
        _position = position;
        _rotation = rotation;
    }
}
