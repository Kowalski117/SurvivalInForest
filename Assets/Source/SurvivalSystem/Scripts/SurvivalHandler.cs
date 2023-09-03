using UnityEngine;
using UnityEngine.Events;

public class SurvivalHandler : MonoBehaviour
{
    [SerializeField] private HotbarDisplay _hotbarDisplay;
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

    public event UnityAction<FoodItemData> OnEatFoodEffect;

    private void OnEnable()
    {
        SaveGame.OnSaveGame += SaveSurvivalAttributes;
        SaveGame.OnLoadData += LoadSurvivalAttributes;

        _hotbarDisplay.ItemClicked += Eat;
        _health.OnRevived += Reborn;
    }

    private void OnDisable()
    {
        SaveGame.OnSaveGame -= SaveSurvivalAttributes;
        SaveGame.OnLoadData -= LoadSurvivalAttributes;

        _hotbarDisplay.ItemClicked -= Eat;
        _health.OnRevived -= Reborn;
    }

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

            _health.SetCanRestoreHealth(false);
        }
        else
        {
            if (_health.HealthPercent > 0)
            {
                _health.SetCanRestoreHealth(true);
                _health.RestoringHealth();
            }
            else
                _health.SetCanRestoreHealth(false);
        }
    }

    public void Eat(InventorySlot slot)
    {
        if (slot.ItemData is FoodItemData foodItemData)
        {
            _hunger.ReplenishValue(foodItemData.AmountSatiety);
            _thirst.ReplenishValue(foodItemData.AmountWater);
                
            if(foodItemData.FoodTypeEffect != FoodTypeEffect.None)
            {
                OnEatFoodEffect?.Invoke(foodItemData);
            }

            if (slot.Durability > 0)
            {
                slot.LowerStrength(1);

                if (slot.Durability <= 0)
                {
                    if (foodItemData.EmptyDishes != null)
                        _playerInventory.AddToInventory(foodItemData.EmptyDishes, 1);

                    _playerInventory.RemoveInventory(slot, 1);
                }
            }
            else
            {
                if (foodItemData.EmptyDishes != null)
                    _playerInventory.AddToInventory(foodItemData.EmptyDishes, 1);

                _playerInventory.RemoveInventory(slot, 1);
            }
        }
    }

    private void HandleTimeUpdate()
    {
        _hunger.LowerValue(_timeHandler.TimeMultiplier);
        _thirst.LowerValue(_timeHandler.TimeMultiplier);
        _sleep.LowerValue(_timeHandler.TimeMultiplier);
    }

    private void Reborn()
    {
        _hunger.SetValue(_hunger.MaxValueInSeconds * 30 / 100);
        _thirst.SetValue(_thirst.MaxValueInSeconds * 30 / 100);
        _sleep.SetValue(_sleep.MaxValueInSeconds * 30 / 100);
    }

    public void SaveSurvivalAttributes()
    {
        ES3.Save("Hunger", _hunger.CurrentAttribute);
        ES3.Save("Thirst", _thirst.CurrentAttribute);
        ES3.Save("Sleep", _sleep.CurrentAttribute);
    }

    public void LoadSurvivalAttributes()
    {
        _hunger.SetValue(ES3.Load<float>("Hunger", _hunger.MaxValueInSeconds));
        _thirst.SetValue(ES3.Load<float>("Thirst", _thirst.MaxValueInSeconds));
        _sleep.SetValue(ES3.Load<float>("Sleep", _sleep.MaxValueInSeconds));
    }
}
