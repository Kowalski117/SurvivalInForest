using UnityEngine;

public class EatEffectHandler : MonoBehaviour
{
    [SerializeField] private SurvivalHandler _survivalHandler;
    [SerializeField] private ColorRotation _colorRotation;

    private float _duration = 5f;

    private void OnEnable()
    {
        _survivalHandler.OnEatFoodEffectPlaying += Play;
    }

    private void OnDisable()
    {
        _survivalHandler.OnEatFoodEffectPlaying -= Play;
    }

    private void Play(FoodItemData foodItemData)
    {
        if(foodItemData.FoodTypeEffect == FoodTypeEffect.ColorRotation)
            _colorRotation.StartRotate(_duration);
    }
}
