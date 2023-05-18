using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Create new Item", order = 51)]
public class Items : ScriptableObject
{
    [SerializeField] private string _id;
    [SerializeField] private string _itemName;
    [SerializeField] private string _description;
    [SerializeField] private Sprite _icon;
    [SerializeField] private GameObject _prefab;
    [SerializeField] private Types _type;
    [SerializeField] private Rarities _rarity;
    [SerializeField] private int _maxStack;
    [SerializeField] private float _weight;
    [SerializeField] private int _baseValue;

    public string Id => _id;
    public string Name => _itemName;
    public string Description => _description;
    public Sprite Icon => _icon;
    public GameObject Prefab => _prefab;
    public Types Type => _type;
    public Rarities Rarity => _rarity;
    public int MaxStack => _maxStack;
    public float Weight => _weight;
    public int BaseValue => _baseValue;

    public enum Types
    {
        craftingMaterial,
        equiment,
        missecellaneous
    }

    public enum Rarities
    {
        common,
        uncommon,
        rare,
        epic,
        legendary
    }
}
