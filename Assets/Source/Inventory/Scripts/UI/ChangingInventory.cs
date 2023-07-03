using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class ChangingInventory : MonoBehaviour
{
    [SerializeField] private PlayerInventoryHolder _inventoryHolder;
    [SerializeField] private ViewInventoryNotifier[] _views;
    [SerializeField] private float _notificationDuration = 2f;

    private Queue<NotificationData> _notificationQueue = new Queue<NotificationData>();

    private void OnEnable()
    {
        _inventoryHolder.OnItemDataChanged += ShowNotification;
    }

    private void OnDisable()
    {
        _inventoryHolder.OnItemDataChanged -= ShowNotification;
    }

    private void ShowNotification(InventoryItemData itemData, int amount)
    {
        var notificationData = new NotificationData(itemData, amount);

        _notificationQueue.Enqueue(notificationData);

        ProcessNotificationQueue();
    }

    private void ProcessNotificationQueue()
    {
        StartCoroutine(ShowNotificationsCoroutine());
    }

    private IEnumerator ShowNotificationsCoroutine()
    {
        foreach (var view in _views)
        {
            if (!view.gameObject.activeInHierarchy && _notificationQueue.Count > 0)
            {
                var notificationData = _notificationQueue.Dequeue();
                view.gameObject.SetActive(true);
                view.Init(notificationData.ItemData, notificationData.Amount);
                yield return new WaitForSeconds(_notificationDuration);
                view.gameObject.SetActive(false);
            }
        }
    }

    private struct NotificationData
    {
        public InventoryItemData ItemData;
        public int Amount;

        public NotificationData(InventoryItemData itemData, int amount)
        {
            ItemData = itemData;
            Amount = amount;
        }
    }
}