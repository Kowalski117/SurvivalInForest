using StarterAssets;
using UnityEngine;

public class SurvivalHandler : MonoBehaviour
{
    [SerializeField] private SurvivalAttribute _hunger;
    [SerializeField] private SurvivalAttribute _thirst;
    [SerializeField] private StaminaAttribute _stamina;

    public StaminaAttribute Stamina => _stamina;

    private void Update()
    {
        _hunger.DecreaseValue();
        _thirst.DecreaseValue();
        _stamina.DecreaseStaminaValue();
    }
}
