using UnityEngine;

public class SurvivalHandler : MonoBehaviour
{
    [SerializeField] private PlayerInventoryHolder _playerInventory;
    [SerializeField] private PlayerHealth _health;
    [SerializeField] private SurvivalAttribute _hunger;
    [SerializeField] private SurvivalAttribute _thirst;
    [SerializeField] private StaminaAttribute _stamina;
    [SerializeField] private SurvivalAttribute _sleep;
    [SerializeField] private TimeHandler _timeHandler;
    [SerializeField] private float _healthDamage = 5f;
    [SerializeField] private float _liftingDelay = 2f;

    private float _lookTimer = 0;
    public SurvivalAttribute Hunger => _hunger;
    public SurvivalAttribute Thirst => _thirst;
    public StaminaAttribute Stamina => _stamina;
    public SurvivalAttribute Sleep => _sleep;
    public TimeHandler TimeHandler => _timeHandler;

    private void Update()
    {
        _stamina.DecreaseStaminaValue();
        HandleTimeUpdate();

        if(_hunger.ValuePercent <= 0 || _thirst.ValuePercent <= 0 || _sleep.ValuePercent <= 0)
        {
            _lookTimer += Time.deltaTime;

            if (_lookTimer >= _liftingDelay)
            {
                _health.LowerHealth(_healthDamage);
                _lookTimer = 0;
            }
        }
    }

    public void Eat(InventorySlot slot)
    {
        if (slot.ItemData is FoodItemData foodItemData)
        {
            _hunger.ReplenishValue(foodItemData.AmountSatiety);
            _thirst.ReplenishValue(foodItemData.AmountWater);

            if (foodItemData.EmptyDishes != null)
                _playerInventory.AddToInventory(foodItemData.EmptyDishes, 1);
        }
        _playerInventory.RemoveInventory(slot.ItemData, 1);
    }

    private void HandleTimeUpdate()
    {
        _hunger.LowerValue(_timeHandler.TimeMultiplier);
        _thirst.LowerValue(_timeHandler.TimeMultiplier);
        _sleep.LowerValue(_timeHandler.TimeMultiplier);
    }
}
