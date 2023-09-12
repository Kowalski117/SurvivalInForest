using UnityEngine;

[CreateAssetMenu(menuName = "Inventory System/Item Data/Clothes", order = 51)]
public class ClothesItemData : InventoryItemData
{
    [SerializeField] private float _protection;
    [SerializeField] private float _boost;
    [SerializeField] private TypeOfClothingUse _typeOfClothingUse;
    [SerializeField] private ClothingType _clothingType = ClothingType.Torso;

    public float Protection => _protection;
    public float Boost => _boost;
    public TypeOfClothingUse TypeOfClothingUse => _typeOfClothingUse;
    public ClothingType ClothingType => _clothingType;
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
