using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(OutlineObject))]
public class BeingLiftedObject : MonoBehaviour
{
    [SerializeField] private int _layerMask = 6;
    [SerializeField] private ObjectItemsData _objectItemsData;
    [SerializeField] private bool _isDeleteAfterSelection = true;

    private UniqueID _uniqueID;
    private OutlineObject _outline;
    private Rigidbody _rigidbody;

    public event Action OnPickedUp;

    public ObjectItemsData ObjectItemsData => _objectItemsData;

    private void Awake()
    {
        _uniqueID = GetComponent<UniqueID>();
        _outline = GetComponent<OutlineObject>();
        _rigidbody= GetComponent<Rigidbody>();
        _rigidbody.isKinematic = true;

        if(!_uniqueID)
            _uniqueID = GetComponentInParent<UniqueID>();
    }

    public void Init(ObjectItemsData objectItemsData)
    {
        _objectItemsData = objectItemsData;
    }

    public void PickUp()
    {
        OnPickedUp?.Invoke();

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
}

[Serializable]
public struct InventoryItem
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
