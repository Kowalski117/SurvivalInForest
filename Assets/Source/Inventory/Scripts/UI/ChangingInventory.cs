using UnityEngine;
using System.Collections.Generic;

public class ChangingInventory : MonoBehaviour
{
    [SerializeField] private PlayerInventoryHolder _inventoryHolder;
    [SerializeField] private MouseItemData _mouseItemData;
    [SerializeField] private List<ViewInventoryNotifier> _views = new List<ViewInventoryNotifier>();
    [SerializeField] private float _notificationDuration = 2f;
    [SerializeField] private float _delay;

    private Queue<NotificationData> _notificationQueue = new Queue<NotificationData>();
    private List<ViewInventoryNotifier> _usedViews = new List<ViewInventoryNotifier>();

    private void Awake()
    {
        foreach (var view in _views)
        {
            view.gameObject.SetActive(true);
            view.Close();
            view.gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        _inventoryHolder.OnItemDataChanged += ShowNotification;
        _mouseItemData.OnItemDataChanged += ShowNotification;
    }

    private void OnDisable()
    {
        _inventoryHolder.OnItemDataChanged -= ShowNotification;
        _mouseItemData.OnItemDataChanged -= ShowNotification;
    }

    private void Update()
    {
        if(_notificationQueue.Count > 0)
        {
            if (_views.Count <= 0)
            {
                foreach (var view in _usedViews)
                {
                    _views.Add(view);
                }

                _usedViews.Clear();
            }

            foreach (var view in _views)
            {
                if (!view.gameObject.activeInHierarchy)
                {
                    NotificationData notificationData = _notificationQueue.Dequeue();
                    view.gameObject.SetActive(true);
                    view.Init(notificationData.ItemData, notificationData.Amount);
                    view.Open();
                    _usedViews.Add(view);
                    _views.Remove(view);
                    break;
                }
            }
        }
    }

    private void ShowNotification(InventoryItemData itemData, int amount)
    {
        if (itemData != null)
        {
            var notificationData = new NotificationData(itemData, amount);
            _notificationQueue.Enqueue(notificationData);
        }
    }
}

public struct NotificationData
{
    public InventoryItemData ItemData;
    public int Amount;

    public NotificationData(InventoryItemData itemData, int amount)
    {
        ItemData = itemData;
        Amount = amount;
    }
}