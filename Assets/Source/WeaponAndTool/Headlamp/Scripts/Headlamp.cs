using UnityEngine;

public class Headlamp : MonoBehaviour
{
    [SerializeField] private ClothesSlotsHandler _clothesSlotsHandler;
    [SerializeField] private Light _light;
    [SerializeField] private ClothesItemData _headlampData;

    private void Awake()
    {
        _light.enabled = false;
    }

    private void OnEnable()
    {
        _clothesSlotsHandler.OnClothesAdded += TurnOnLight;
        _clothesSlotsHandler.OnClothesRemoved += TurnOffLight;
    }

    private void OnDisable()
    {
        _clothesSlotsHandler.OnClothesAdded -= TurnOnLight;
        _clothesSlotsHandler.OnClothesRemoved -= TurnOffLight;
    }

    private void TurnOnLight(ClothesItemData clothes)
    {
        if(_headlampData == clothes)
            _light.enabled = true;
    }

    private void TurnOffLight(ClothesItemData clothes)
    {
        if (_headlampData == clothes)
            _light.enabled = false;
    }
}
