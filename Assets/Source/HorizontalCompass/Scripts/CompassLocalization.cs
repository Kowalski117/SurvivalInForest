using UnityEngine;
using UnityEngine.UI;

public class CompassLocalization : MonoBehaviour
{
    [SerializeField] private LocalizationHandler _localizationHandler;
    [SerializeField] private RawImage _compassImage;
    [SerializeField] private Texture[] _textures;

    private void OnEnable()
    {
        _localizationHandler.OnLanguageChanged += ChangeLanguage;
    }

    private void OnDisable()
    {
        _localizationHandler.OnLanguageChanged -= ChangeLanguage;
    }

    private void ChangeLanguage()
    {
        _compassImage.texture = _textures[_localizationHandler.CurrentLanguageIndex];
    }
}
