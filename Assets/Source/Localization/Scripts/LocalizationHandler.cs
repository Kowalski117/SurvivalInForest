using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class LocalizationHandler : MonoBehaviour
{
    private string _currentLanguage;

    private Dictionary<string, string> _language = new()
    {
        { "ru", "Russian" },
        { "en", "English" },
        { "tr", "Turkish" },
    };

    public event UnityAction OnLanguageChanged;

    public int CurrentLanguageIndex
    {
        get
        {
            int index = 0;
            foreach (var pair in _language)
            {
                if (pair.Key == _currentLanguage)
                {
                    return index;
                }
                index++;
            }
            return -1;
        }
    }

    public void SetLanguageString(string value)
    {
        if (_language.ContainsKey(value))
            SetLanguage(value);
    }

    public void SetLanguageIndex(int index)
    {
        //if (_language.Count > index + 1)
            SetLanguage(_language.Keys.ElementAt(index));
    }

    private void SetLanguage(string value)
    {
        _currentLanguage = value;
        OnLanguageChanged?.Invoke();
        PlayerPrefs.SetString(ConstantsSDK.Language, _currentLanguage);
        PixelCrushers.DialogueSystem.DialogueManager.SetLanguage(value);
    }
}
