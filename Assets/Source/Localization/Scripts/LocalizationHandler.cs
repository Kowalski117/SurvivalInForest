using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization.Settings;

public class LocalizationHandler : MonoBehaviour
{
    private int _languageIndex;

    public event UnityAction OnLanguageChanged;

    public int CurrentLanguageIndex => _languageIndex;

    private IEnumerator Start()
    {
        //Debug.Log("1");
        //if (ES3.KeyExists(ConstantsSDK.LanguageIndex))
        //    SetLanguageIndex(ES3.Load<int>(ConstantsSDK.LanguageIndex));

        yield return new WaitForSeconds(0.6f);

        if (ES3.KeyExists(ConstantsSDK.LanguageIndex))
            SetLanguageIndex(ES3.Load<int>(ConstantsSDK.LanguageIndex));
    }

    public void SetLanguageString(string value)
    {
        for (int i = 0; i < LocalizationSettings.AvailableLocales.Locales.Count; i++)
        {
            if (LocalizationSettings.AvailableLocales.Locales[i].Formatter.ToString() == value)
            {
                SetLanguageIndex(i);
                break;
            }
        }
    }

    public void SetLanguageIndex(int index)
    {
        if (LocalizationSettings.AvailableLocales.Locales.Count > index && _languageIndex != index)
        {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index];
            OnLanguageChanged?.Invoke();
            _languageIndex = index;
            Debug.Log(index);
            ES3.Save(ConstantsSDK.LanguageIndex, _languageIndex);
        }
    }
}
