using UnityEngine;

public class EatEffectHandler : MonoBehaviour
{
    [SerializeField] private SurvivalHandler _survivalHandler;
    [SerializeField] private ColorRotation _colorRotation;

    private float _duration = 5f;

    private void OnEnable()
    {
        _survivalHandler.OnEatFoodEffect += PlayEatEffect;
    }

    private void OnDisable()
    {
        _survivalHandler.OnEatFoodEffect -= PlayEatEffect;
    }

    private void PlayEatEffect(FoodItemData foodItemData)
    {
        if(foodItemData.FoodTypeEffect == FoodTypeEffect.ColorRotation)
            _colorRotation.StartEffect(_duration);
    }
}
