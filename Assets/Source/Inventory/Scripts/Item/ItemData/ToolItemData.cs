using UnityEngine;

[CreateAssetMenu(menuName = "Inventory System/Item Data/Tool", order = 51)]
public class ToolItemData : InventoryItemData
{
    [SerializeField] private ToolType _toolType;
    [SerializeField] private float _damageResources;
    [SerializeField] private float _damageLiving;
    [SerializeField] private float _speed; 
    [SerializeField] private ParticleSystem _muzzleFlash;
    [SerializeField] private ParticleSystem _hitEffect;
    [SerializeField] private AudioClip _muzzleSound;

    public ToolType ToolType => _toolType;
    public float DamageResources => _damageResources;
    public float DamageLiving => _damageLiving;
    public float Speed => _speed;
    public ParticleSystem MuzzleFlash => _muzzleFlash;
    public ParticleSystem HitEffect => _hitEffect;
    public AudioClip MuzzleSound => _muzzleSound;
}

public enum ToolType
{
    Axe,
    Pickaxe,
}
