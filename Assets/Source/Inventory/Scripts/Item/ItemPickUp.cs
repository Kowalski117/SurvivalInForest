using PixelCrushers.QuestMachine.Wrappers;
using UnityEngine;

[RequireComponent(typeof(UniqueID))]
public class ItemPickUp : MonoBehaviour
{
    [SerializeField] private QuestControl _questControl;
    [SerializeField] private InventoryItemData _itemData;
    [SerializeField] private ItemPickUpSaveData _itemSaveData;
    [SerializeField] private float _durability;

    private UniqueID _uniqueID;
    private string _item = "Item_";

    public InventoryItemData ItemData => _itemData;
    public float Durability => _durability;

    private void Awake()
    {
        _questControl = GetComponent<QuestControl>();
        _uniqueID = GetComponent<UniqueID>();
        _durability = _itemData.Durability;
    }

    private void OnEnable()
    {
        SaveGame.OnSaveGame += Save;
        SaveGame.OnLoadData += Load;
    }

    private void OnDisable()
    {
        SaveGame.OnSaveGame -= Save;
        SaveGame.OnLoadData -= Load;
    }

    public void GenerateNewID()
    {
        _uniqueID.Generate();
    }

    public void UpdateDurability(float durability)
    {
        _durability = durability;
    }

    public void PicUp()
    {
        _questControl.SendToMessageSystem("Find:" + _itemData.name);

        if (ES3.KeyExists(_item + _uniqueID.Id))
            ES3.DeleteKey(_item + _uniqueID.Id);
        Destroy(this.gameObject);
    }

    private void Save()
    {
        ItemPickUpSaveData itemSaveData = new ItemPickUpSaveData(_itemData, transform.position, transform.rotation, _durability);
        ES3.Save(_item + _uniqueID.Id, itemSaveData);
    }

    private void Load()
    {
        if (ES3.KeyExists(_item + _uniqueID.Id))
        {
            ItemPickUpSaveData itemSaveData = ES3.Load<ItemPickUpSaveData>(_item + _uniqueID.Id, new ItemPickUpSaveData(_itemData, transform.position, transform.rotation, _durability));
            _itemData = itemSaveData.ItemData;
            transform.position = itemSaveData.Position;
            transform.rotation = itemSaveData.Rotation;
            _durability = itemSaveData.Durability;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}

[System.Serializable]
public struct ItemPickUpSaveData
{
    [SerializeField] private InventoryItemData _itemData;
    [SerializeField] private Vector3 _position;
    [SerializeField] private Quaternion _rotation;
    [SerializeField] private float _durability;
    
    public InventoryItemData ItemData => _itemData;
    public Vector3 Position => _position;
    public Quaternion Rotation => _rotation;
    public float Durability => _durability;

    public ItemPickUpSaveData(InventoryItemData itemData, Vector3 position, Quaternion rotation, float durability)
    {
        _itemData = itemData;
        _position = position;
        _rotation = rotation;
        _durability = durability; 
    }
}
