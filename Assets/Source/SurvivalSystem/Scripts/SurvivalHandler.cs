using System.Collections;
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
    [SerializeField] private PlayerAudioHandler _playerAudioHandler;
    [SerializeField] private BuildTool _buildTool;
    [SerializeField] private float _healthDamage = 5f;
    [SerializeField] private float _liftingDelay = 2f;

    private float _lookTimer = 0;
    private int _addAmount = 1;
    private Coroutine _eatCoroutine;
    private bool _isEating = false;
    private bool _isEnable = false;

    public PlayerHealth PlayerHealth => _health;
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

        if(_health.HealthPercent > 0 && _isEnable)
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
        if (slot.ItemData is FoodItemData foodItemData && !_isEating && !_buildTool.IsMoveBuild)
        {
            _isEating = true;
            _playerAudioHandler.PlayEatingSound(foodItemData.AmountSatiety, foodItemData.AmountWater);
            _hunger.ReplenishValue(foodItemData.AmountSatiety);
            _thirst.ReplenishValue(foodItemData.AmountWater);
            _health.ReplenishHealth(foodItemData.AmountHealth);
                
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
                        _playerInventory.AddToInventory(foodItemData.EmptyDishes, _addAmount);

                    _playerInventory.RemoveInventory(slot, _addAmount);
                }
            }
            else
            {
                if (foodItemData.EmptyDishes != null)
                    _playerInventory.AddToInventory(foodItemData.EmptyDishes, _addAmount);

                _playerInventory.RemoveInventory(slot, _addAmount);
            }

            StartCoroutine(_addAmount);
        }
    }

    public void SetEnable(bool isActive)
    {
        _isEnable = isActive;
    }

    private void HandleTimeUpdate()
    {
        _hunger.LowerValue(_timeHandler.TimeMultiplier * Time.deltaTime);
        _thirst.LowerValue(_timeHandler.TimeMultiplier * Time.deltaTime);
        _sleep.LowerValue(_timeHandler.TimeMultiplier * Time.deltaTime);
    }

    private void Reborn()
    {
        _hunger.SetValue(_hunger.MaxValueInSeconds * 30 / 100);
        _thirst.SetValue(_thirst.MaxValueInSeconds * 30 / 100);
        _sleep.SetValue(_sleep.MaxValueInSeconds * 30 / 100);
    }

    private void SaveSurvivalAttributes()
    {
        ES3.Save("Hunger", _hunger.CurrentAttribute);
        ES3.Save("Thirst", _thirst.CurrentAttribute);
        ES3.Save("Sleep", _sleep.CurrentAttribute);
    }

    private void LoadSurvivalAttributes()
    {
        _hunger.SetValue(ES3.Load<float>("Hunger", _hunger.MaxValueInSeconds));
        _thirst.SetValue(ES3.Load<float>("Thirst", _thirst.MaxValueInSeconds));
        _sleep.SetValue(ES3.Load<float>("Sleep", _sleep.MaxValueInSeconds));
    }

    private void StartCoroutine(float duration)
    {
        if (_eatCoroutine != null)
        {
            StopCoroutine(_eatCoroutine);
            _eatCoroutine = null;
        }

        _eatCoroutine = StartCoroutine(WaitForEatToFinish(duration));
    }

    private IEnumerator WaitForEatToFinish(float duration)
    {
        yield return new WaitForSeconds(duration);

        _isEating = false;
    }
}
