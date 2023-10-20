using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory System/Item Data/Food", order = 51)]
public class FoodItemData : InventoryItemData
{
    [SerializeField] private float _amountWater;
    [SerializeField] private float _amountSatiety;
    [SerializeField] private float _amountSleep;
    [SerializeField] private float _amountHealth;
    [SerializeField] private InventoryItemData _emptyDishes;
    [SerializeField] private FoodTypeEffect _foodTypeEffect;

    public float AmountWater => _amountWater;
    public float AmountSatiety => _amountSatiety;
    public float AmountSleep => _amountSleep;
    public float AmountHealth => _amountHealth;
    public InventoryItemData EmptyDishes => _emptyDishes;
    public FoodTypeEffect FoodTypeEffect => _foodTypeEffect;
}

public enum FoodTypeEffect
{
    None,
    ColorRotation,
}

#if UNITY_EDITOR
[CustomEditor(typeof(FoodItemData))]
public class FoodItemDataEditor : Editor
{
    public override Texture2D RenderStaticPreview(string assetPath, Object[] subAssets, int width, int height)
    {
        var data = (FoodItemData)target;

        if (data == null || data.Icon == null)
            return null;

        Texture2D texture = new Texture2D(width, height);
        EditorUtility.CopySerialized(data.Icon.texture, texture);
        return texture;
    }
}
#endif
