using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SaveItemHandler : SaveHandler
{
    [SerializeField] private static List<InventoryItemData> _itemDataList;


    //private string _idsItems = "idsItems";

    //private ItemPickUp _currentItemPickUp;

    private void Awake()
    {
        SetItemIds();
    }

    //protected override void SaveBase()
    //{
    //    //ES3.Save(_idsItems, Ids);
    //}

    //protected override void LoadBase()
    //{
    //    //Ids = ES3.Load<List<string>>(_idsItems);

    //    //foreach (var itemData in Ids)
    //    //{
    //    //    if (ES3.KeyExists(itemData))
    //    //    {
    //    //        ItemPickUpSaveData itemSaveData = ES3.Load<ItemPickUpSaveData>(itemData);
    //    //        foreach (var data in _itemDataList)
    //    //        {
    //    //            if (data.Id == itemSaveData.Id)
    //    //            {
    //    //                if(data.ItemPrefab != null)
    //    //                {
    //    //                    _currentItemPickUp = Instantiate(data.ItemPrefab, itemSaveData.Position, itemSaveData.Rotation, Container);
    //    //                    _currentItemPickUp.Init(itemSaveData, itemData);
    //    //                    _currentItemPickUp = null;
    //    //                    break;
    //    //                }
    //    //            }
    //    //        }
    //    //    }
    //    //}
    //}

    [ContextMenu("Set Ids")]
    public void SetItemIds()
    {
        _itemDataList = new List<InventoryItemData>();

        var foundItems = Resources.LoadAll<InventoryItemData>("ItemData").OrderBy(i => i.Id).ToList();

        var hasIdInRange = foundItems.Where(i => i.Id != -1 && i.Id < foundItems.Count).OrderBy(i => i.Id).ToList();
        var hasIdNotInRange = foundItems.Where(i => i.Id != -1 && i.Id >= foundItems.Count).OrderBy(i => i.Id).ToList();
        var noId = foundItems.Where(i => i.Id <= -1).ToList();

        var index = 0;

        for (int i = 0; i < foundItems.Count; i++)
        {
            InventoryItemData itemToAdd;
            itemToAdd = hasIdInRange.Find(d => d.Id == i);

            if (itemToAdd != null)
            {
                _itemDataList.Add(itemToAdd);
            }
            else if (index < noId.Count)
            {
                noId[index].SetId(i);
                itemToAdd = noId[index];
                index++;
                _itemDataList.Add(itemToAdd);
            }
        }

        foreach (var item in hasIdNotInRange)
        {
            _itemDataList.Add(item);
        }
    }

    public static InventoryItemData GetItem(float id)
    {
        return _itemDataList.Find(i => i.Id == id);
    }

    protected override void LoadBase()
    {
        //throw new System.NotImplementedException();
    }

    protected override void SaveBase()
    {
        //throw new System.NotImplementedException();
    }
}
