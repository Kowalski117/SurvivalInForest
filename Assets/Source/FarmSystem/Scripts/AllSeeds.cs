using UnityEngine;

[CreateAssetMenu(menuName = "Inventory System/All Seeds", order = 51)]
public class AllSeeds : ScriptableObject
{
    [SerializeField] private SeedItemData[] _items;

    public SeedItemData[] Items => _items;
}
