using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class LocalizationHandler : MonoBehaviour
{
    private string _currentLanguage;

    public static event Action OnLanguageChanged;

    private Dictionary<string, string> _language = new()
    {
        { "ru", "Russian" },
        { "en", "English" },
        { "tr", "Turkish" },
    };

    public void SetLanguageString(string value)
    {
        if (_language.ContainsKey(value))
            SetLanguage(value);
    }

    public void SetLanguageIndex(int index)
    {
        if (_language.Count > index + 1)
            SetLanguage(_language.Values.ElementAt(index));
    }

    private void SetLanguage(string value)
    {
        PixelCrushers.DialogueSystem.DialogueManager.SetLanguage(value);
        _currentLanguage = value;
        OnLanguageChanged?.Invoke();
        PlayerPrefs.SetString(ConstantsSDK.Language, _currentLanguage);
    }
}
