using UnityEngine;

[RequireComponent(typeof(UniqueID))]
public class ItemPickUp : MonoBehaviour
{
    [SerializeField] private InventoryItemData _itemData;
    [SerializeField] private ItemPickUpSaveData _itemSaveData;
    [SerializeField] private float _durability;

    private string _id;

    public InventoryItemData ItemData => _itemData;
    public float Durability => _durability;

    private void Awake()
    {
        _id = GetComponent<UniqueID>().Id;
        _durability = _itemData.Durability;
        SaveLoad.OnLoadData += LoadGame;
        _itemSaveData = new ItemPickUpSaveData(_itemData, transform.position, transform.rotation, _durability);
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
    [SerializeField] private float _durability; 

    public ItemPickUpSaveData(InventoryItemData itemData, Vector3 position, Quaternion rotation, float durability)
    {
        _itemData = itemData;
        _position = position;
        _rotation = rotation;
        _durability = durability; 
    }
}
