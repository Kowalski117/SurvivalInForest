using UnityEngine;

public class SurvivalHandler : MonoBehaviour
{
    [SerializeField] private SurvivalAttribute _hunger;
    [SerializeField] private SurvivalAttribute _thirst;
    [SerializeField] private StaminaAttribute _stamina;
    [SerializeField] private SurvivalAttribute _sleep;
    [SerializeField] private TimeHandler _timeHandler;

    public SurvivalAttribute Hunger => _hunger;
    public SurvivalAttribute Thirst => _thirst;
    public StaminaAttribute Stamina => _stamina;

    private void Update()
    {
        _stamina.DecreaseStaminaValue();
        HandleTimeUpdate();
    }

    private void HandleTimeUpdate()
    {
        _hunger.LowerValue(_timeHandler.TimeMultiplier);
        _thirst.LowerValue(_timeHandler.TimeMultiplier);
        _sleep.LowerValue(_timeHandler.TimeMultiplier);
    }
}
