using UnityEngine;

[RequireComponent(typeof(SphereCollider), typeof(UniqueID))]
public class ItemPickUp : MonoBehaviour
{
    [SerializeField] private float pickUpRadius = 1f;
    [SerializeField] private InventoryItemData _itemData;

    [SerializeField] private ItemPickUpSaveData _itemSaveData;

    private string _id;

    private InventoryHolder _currentInventoryHolder;
    private SphereCollider _myCollider;
    private SelectionPlayerInput _playerInput;

    public InventoryItemData ItemData => _itemData;

    private void Awake()
    {
        _playerInput = FindObjectOfType<SelectionPlayerInput>();
        _id = GetComponent<UniqueID>().Id;
        SaveLoad.OnLoadData += LoadGame;
        _itemSaveData = new ItemPickUpSaveData(_itemData, transform.position, transform.rotation);
        _myCollider = GetComponent<SphereCollider>();
        _myCollider.isTrigger = true;
        _myCollider.radius = pickUpRadius;
    }

    private void Start()
    {
        _id = GetComponent<UniqueID>().Id;
        SaveGameHandler.Data.ActiveItems.Add(_id, _itemSaveData);
    }

    private void OnEnable()
    {
        _playerInput.PickUp += PickUp;
    }

    private void OnDisable()
    {
        _playerInput.PickUp -= PickUp;
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Interactor interactor))
        {
            if (!interactor)
                return;

            _currentInventoryHolder = interactor.PlayerInventoryHolder;
        }
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.TryGetComponent(out InteractionPoint inventory))
    //    {
    //        if (!inventory)
    //            return;

    //        _currentInventoryHolder = inventory.InventoryHolder;
    //    }
    //}

    private void PickUp()
    {
        if (_currentInventoryHolder != null && _currentInventoryHolder.InventorySystem.AddToInventory(_itemData, 1))
        {
            Destroy(this.gameObject);
        }
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
