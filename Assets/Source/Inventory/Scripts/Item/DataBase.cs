using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Inventory System/Item Database",order = 51)]
public class DataBase : ScriptableObject
{
    [SerializeField] private List<InventoryItemData> _itemDataBase;

    [ContextMenu("Set Ids")]
    public void SetItemIds()
    {
        _itemDataBase = new List<InventoryItemData>();

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
                _itemDataBase.Add(itemToAdd);
            }
            else if(index < noId.Count)
            {
                noId[index].SetId(i);
                itemToAdd = noId[index];
                index++;
                _itemDataBase.Add(itemToAdd);
            }
        }

        foreach (var item in hasIdNotInRange)
        {
            _itemDataBase.Add(item);
        }
    }

    public InventoryItemData GetItem(int id)
    {
        return _itemDataBase.Find(i => i.Id == id);
    }
}
