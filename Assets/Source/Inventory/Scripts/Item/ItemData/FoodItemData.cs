using UnityEngine;

[CreateAssetMenu(menuName = "Inventory System/Food", order = 51)]
public class FoodItemData : InventoryItemData
{
    [SerializeField] private int _amountWater;
    [SerializeField] private int _amountSatiety;
    [SerializeField] private int _amountHealth;

    public int AmountWater => _amountWater;
    public int AmountSatiety => _amountSatiety;
    public int AmountHealth => _amountHealth;

    public void Eat()
    {

    }
}
