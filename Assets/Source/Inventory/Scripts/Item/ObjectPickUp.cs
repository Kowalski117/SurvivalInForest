using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(UniqueID))]
public class ObjectPickUp : MonoBehaviour
{
    [SerializeField] private int _layerMask = 6;
    [SerializeField] private ObjectItemsData _objectItemsData;
    [SerializeField] private bool _isDeleteAfterSelection = true;

    private UniqueID _uniqueID;
    private OutlineObject _outline;
    private Rigidbody _rigidbody;
    private string _object = "Object_";

    public event UnityAction OnPickUp;

    public ObjectItemsData ObjectItemsData => _objectItemsData;

    private void Awake()
    {
        _uniqueID = GetComponent<UniqueID>();
        _outline = GetComponent<OutlineObject>();
        _rigidbody= GetComponent<Rigidbody>();
        _rigidbody.isKinematic = true;
    }

    public void Init(ObjectItemsData objectItemsData)
    {
        _objectItemsData = objectItemsData;
    }

    public void PicUp()
    {
        OnPickUp?.Invoke();

        if (_isDeleteAfterSelection )
            Destroy(this.gameObject);
        else
            gameObject.SetActive(false);
    }

    public void TurnOff()
    {
        _rigidbody.isKinematic = true;

        if (_outline)
            _outline.enabled = false;

        gameObject.layer = default;
        enabled = false;
    }

    public void Enable()
    {
        enabled = true;

        if (_outline)
            _outline.enabled = true;

        gameObject.layer = _layerMask;
    }

    private void Save()
    {
        ObjectPickUpSaveData itemSaveData = new ObjectPickUpSaveData(transform.position, transform.rotation);
        ES3.Save(_object + _uniqueID.Id, itemSaveData);
    }

    private void Load()
    {
        if (ES3.KeyExists(_object + _uniqueID.Id))
        {
            ObjectPickUpSaveData itemSaveData = ES3.Load<ObjectPickUpSaveData>(_object + _uniqueID.Id, new ObjectPickUpSaveData(transform.position, transform.rotation));
            transform.position = itemSaveData.Position;
            transform.rotation = itemSaveData.Rotation;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}

[Serializable]
public class InventoryItem
{
    [SerializeField] private InventoryItemData _itemData;
    [SerializeField] private int _amount;

    public InventoryItemData ItemData => _itemData;
    public int Amount => _amount;

    public InventoryItem(InventoryItemData itemData, int amount)
    {
        _itemData = itemData;
        _amount = amount;
    }
}

[System.Serializable]
public struct ObjectPickUpSaveData
{
    [SerializeField] private Vector3 _position;
    [SerializeField] private Quaternion _rotation;

    public Vector3 Position => _position;
    public Quaternion Rotation => _rotation;

    public ObjectPickUpSaveData(Vector3 position, Quaternion rotation)
    {
        _position = position;
        _rotation = rotation;
    }
}
