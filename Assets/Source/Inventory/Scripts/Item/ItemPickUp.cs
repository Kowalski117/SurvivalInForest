using PixelCrushers.DialogueSystem;
using PixelCrushers.QuestMachine.Wrappers;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(UniqueID))]
public class ItemPickUp : MonoBehaviour
{
    [SerializeField] private InventoryItemData _itemData;
    [SerializeField] private ItemPickUpSaveData _itemSaveData;
    [SerializeField] private float _durability;

    public event UnityAction DestroyItem;
    
    private QuestControl _questControl;
    private OutlineObject _outline;
    private Rigidbody _rigidbody;
    private UniqueID _uniqueID;

    public InventoryItemData ItemData => _itemData;
    public float Durability => _durability;
    public string Id => _uniqueID.Id;
    public Rigidbody Rigidbody => _rigidbody;

    private void Awake()
    {
        _outline = GetComponent<OutlineObject>();
        _questControl = GetComponent<QuestControl>();
        _uniqueID = GetComponent<UniqueID>();
        _durability = _itemData.Durability;
        _rigidbody = GetComponent<Rigidbody>();

        Load();
    }

    public void GenerateNewID()
    {
        _uniqueID.Generate();
    }

    public void UpdateDurability(float durability)
    {
        _durability = durability;
    }

    public void PickUp()
    {
        _questControl.SendToMessageSystem(MessageConstants.PickUp + _itemData.Name);
        ES3.Save(_uniqueID.Id, _uniqueID.Id);
        DestroyItem?.Invoke();
        Destroy(this.gameObject);
    }

    public void TurnOff()
    {
        _rigidbody.isKinematic = true;
        _outline.enabled = false;
        gameObject.layer = default;
        enabled = false;
    }

    private void Load()
    {
        if (ES3.KeyExists(_uniqueID.Id))
        {
            Destroy(gameObject);
            ES3.DeleteKey(_uniqueID.Id);
        }
    }
}

[System.Serializable]
public struct ItemPickUpSaveData
{
    [SerializeField] private int _id;
    [SerializeField] private Vector3 _position;
    [SerializeField] private Quaternion _rotation;
    [SerializeField] private float _durability;
    
    public int Id => _id;
    public Vector3 Position => _position;
    public Quaternion Rotation => _rotation;
    public float Durability => _durability;

    public ItemPickUpSaveData(int id, Vector3 position, Quaternion rotation, float durability)
    {
        _id = id;
        _position = position;
        _rotation = rotation;
        _durability = durability; 
    }
}
