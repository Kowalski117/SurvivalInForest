using UnityEngine;

[CreateAssetMenu(menuName = "Inventory System/Item Data/Tool", order = 51)]
public class ToolItemData : InventoryItemData
{
    [SerializeField] private ToolType _toolType;
    [SerializeField] private float _damageResources;
    [SerializeField] private float _damageLiving;

    public ToolType ToolType => _toolType;
    public float DamageResources => _damageResources;
    public float DamageLiving => _damageLiving;
}

public enum ToolType
{
    Axe,
    Pickaxe,
}
