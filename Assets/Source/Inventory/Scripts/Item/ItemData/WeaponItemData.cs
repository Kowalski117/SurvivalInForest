using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory System/Item Data/Weapon", order = 51)]
public class WeaponItemData : InventoryItemData
{
    [SerializeField] private WeaponType _weaponType;
    [SerializeField] private float _damage;
    [SerializeField] private float _overTimeDamage;
    [SerializeField] private float _speed;
    [SerializeField] private ItemPickUp _bullet;
    [SerializeField] private ParticleSystem _muzzleFlash;
    [SerializeField] private ParticleSystem _hitEffect;
    [SerializeField] private AudioClip _muzzleSound;

    public WeaponType WeaponType => _weaponType;
    public float Damage => _damage;
    public float OverTimeDamage => _overTimeDamage;   
    public float Speed => _speed;
    public ItemPickUp Bullet => _bullet;
    public ParticleSystem MuzzleFlash => _muzzleFlash;
    public ParticleSystem HitEffect => _hitEffect;
    public AudioClip MuzzleSound => _muzzleSound;
}

public enum WeaponType
{
    RangedWeapon,
    MeleeWeapon
}

#if UNITY_EDITOR
[CustomEditor(typeof(WeaponItemData))]
public class WeaponItemDataEditor : Editor
{
    public override Texture2D RenderStaticPreview(string assetPath, Object[] subAssets, int width, int height)
    {
        var data = (WeaponItemData)target;

        if (data == null || data.Icon == null)
            return null;

        Texture2D texture = new Texture2D(width, height);
        EditorUtility.CopySerialized(data.Icon.texture, texture);
        return texture;
    }
}
#endif