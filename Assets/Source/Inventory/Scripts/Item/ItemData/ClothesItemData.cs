using UnityEngine;

[CreateAssetMenu(menuName = "Inventory System/Item Data/Clothes", order = 51)]
public class ClothesItemData : InventoryItemData
{
    [SerializeField] private float _protection;
    [SerializeField] private float _boost;
    [SerializeField] private TypeOfClothingUse _typeOfClothingUse;
    [SerializeField] private ClothingType _clothingType = ClothingType.Torso;
    [SerializeField] private InventoryItemData _dischargedItem;

    public float Protection => _protection;
    public float Boost => _boost;
    public TypeOfClothingUse TypeOfClothingUse => _typeOfClothingUse;
    public ClothingType ClothingType => _clothingType;
    public InventoryItemData DischargedItem => _dischargedItem;
}

public enum ClothingType
{
    Headdress,
    Torso,
    Legs,
    Backpack,
}

public enum TypeOfClothingUse
{
    AmountOfTime,
    AmountOfDamage
}
