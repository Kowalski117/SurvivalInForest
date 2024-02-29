using System;
using System.Collections;
using UnityEngine;

public class SurvivalHandler : MonoBehaviour
{
    private const float Delay = 1;

    [SerializeField] private HotbarDisplay _hotbarDisplay;
    [SerializeField] private PlayerInventoryHolder _playerInventory;
    [SerializeField] private PlayerHealth _health;
    [SerializeField] private SurvivalAttribute _hunger;
    [SerializeField] private SurvivalAttribute _thirst;
    [SerializeField] private StaminaAttribute _stamina;
    [SerializeField] private SurvivalAttribute _sleep;
    [SerializeField] private MainClock _timeHandler;
    [SerializeField] private PlayerAudioHandler _playerAudioHandler;
    [SerializeField] private BuildTool _buildTool;
    [SerializeField] private float _healthDamage = 5f;
    [SerializeField] private float _liftingDelay = 2f;

    private float _lookTimer = 0;
    private int _addAmount = 1;
    private bool _isEating = false;
    private bool _isEnable = false;
    private float _maximumDivisor = 30;
    private int _percent = 100;

    private Coroutine _eatCoroutine;
    private WaitForSeconds _waitForSeconds = new WaitForSeconds(Delay);

    public event Action<FoodItemData> OnEatFoodEffectPlaying;

    public PlayerHealth PlayerHealth => _health;
    public SurvivalAttribute Hunger => _hunger;
    public SurvivalAttribute Thirst => _thirst;
    public StaminaAttribute Stamina => _stamina;
    public SurvivalAttribute Sleep => _sleep;
    public MainClock TimeHandler => _timeHandler;

    private void OnEnable()
    {
        SavingGame.OnGameSaved += SaveSurvivalAttributes;
        SavingGame.OnGameLoaded += LoadSurvivalAttributes;

        _hotbarDisplay.OnItemClicked += Eat;
        _health.OnRevived += Reborn;
    }

    private void OnDisable()
    {
        SavingGame.OnGameSaved -= SaveSurvivalAttributes;
        SavingGame.OnGameLoaded -= LoadSurvivalAttributes;

        _hotbarDisplay.OnItemClicked -= Eat;
        _health.OnRevived -= Reborn;
    }

    private void Update()
    {
        if (_health.IsGodMode)
            return;

        _stamina.DecreaseValue();

        if (_health.HealthPercent > 0 && _isEnable)
            HandleTimeUpdate();

        if (_hunger.ValuePercent <= 0 || _thirst.ValuePercent <= 0 || _sleep.ValuePercent <= 0)
        {
            _lookTimer += Time.deltaTime;

            if (_lookTimer >= _liftingDelay)
            {
                _health.Lower(_healthDamage);
                _lookTimer = 0;
            }

            _health.SetCanRestoreHealth(false);
        }
        else
        {
            if (_health.HealthPercent > 0)
            {
                _health.SetCanRestoreHealth(true);
                _health.Restoring();
            }
            else
                _health.SetCanRestoreHealth(false);
        }
    }

    public void Eat(InventorySlot slot)
    {
        if (slot.ItemData is FoodItemData foodItemData && !_isEating && !_buildTool.IsMoveBuild && !_health.IsDied)
        {
            _isEating = true;
            _playerAudioHandler.PlayEatingSound(foodItemData.AmountSatiety, foodItemData.AmountWater);
            _hunger.ReplenishValue(foodItemData.AmountSatiety);
            _thirst.ReplenishValue(foodItemData.AmountWater);
            _sleep.ReplenishValue(foodItemData.AmountSleep);
            _health.Replenish(foodItemData.AmountHealth);
                
            if(foodItemData.FoodTypeEffect != FoodTypeEffect.None)
                OnEatFoodEffectPlaying?.Invoke(foodItemData);

            if (slot.Durability > 0)
            {
                slot.LowerStrength(_addAmount);

                if (slot.Durability <= 0)
                {
                    if (foodItemData.EmptyDishes != null)
                        _playerInventory.AddItem(foodItemData.EmptyDishes, _addAmount);

                    _playerInventory.RemoveSlot(slot, _addAmount);
                }
            }
            else
            {
                if (foodItemData.EmptyDishes != null)
                    _playerInventory.AddItem(foodItemData.EmptyDishes, _addAmount);

                _playerInventory.RemoveSlot(slot, _addAmount);
            }

            StartCoroutine();
        }
    }

    public void SetEnable(bool isActive)
    {
        _isEnable = isActive;
    }

    private void HandleTimeUpdate()
    {
        if (_health.IsGodMode)
            return;

        _hunger.LowerValue(_timeHandler.TimeMultiplier * Time.deltaTime);
        _thirst.LowerValue(_timeHandler.TimeMultiplier * Time.deltaTime);
        _sleep.LowerValue(_timeHandler.TimeMultiplier * Time.deltaTime);
    }

    private void Reborn()
    {
        if (_health.IsGodMode)
            return;

        _hunger.SetValue(_hunger.MaxValueInSeconds * _maximumDivisor / _percent);
        _thirst.SetValue(_thirst.MaxValueInSeconds * _maximumDivisor / _percent);
        _sleep.SetValue(_sleep.MaxValueInSeconds * _maximumDivisor / _percent);
    }

    private void SaveSurvivalAttributes()
    {
        ES3.Save(SaveLoadConstants.Hunger, _hunger.CurrentAttribute);
        ES3.Save(SaveLoadConstants.Thirst, _thirst.CurrentAttribute);
        ES3.Save(SaveLoadConstants.Sleep, _sleep.CurrentAttribute);
    }

    private void LoadSurvivalAttributes()
    {
        _hunger.SetValue(ES3.Load(SaveLoadConstants.Hunger, _hunger.MaxValueInSeconds));
        _thirst.SetValue(ES3.Load(SaveLoadConstants.Thirst, _thirst.MaxValueInSeconds));
        _sleep.SetValue(ES3.Load(SaveLoadConstants.Sleep, _sleep.MaxValueInSeconds));
    }

    private void StartCoroutine()
    {
        if (_eatCoroutine != null)
        {
            StopCoroutine(_eatCoroutine);
            _eatCoroutine = null;
        }

        _eatCoroutine = StartCoroutine(WaitForEatToFinish());
    }

    private IEnumerator WaitForEatToFinish()
    {
        yield return _waitForSeconds;

        _isEating = false;
    }
}
