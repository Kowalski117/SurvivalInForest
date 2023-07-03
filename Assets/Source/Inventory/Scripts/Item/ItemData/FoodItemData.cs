using UnityEngine;

[CreateAssetMenu(menuName = "Inventory System/Item Data/Food", order = 51)]
public class FoodItemData : InventoryItemData
{
    [SerializeField] private float _amountWater;
    [SerializeField] private float _amountSatiety;
    [SerializeField] private float _amountHealth;

    public float AmountWater => _amountWater;
    public float AmountSatiety => _amountSatiety;
    public float AmountHealth => _amountHealth;

    public void Eat()
    {

    }
}
