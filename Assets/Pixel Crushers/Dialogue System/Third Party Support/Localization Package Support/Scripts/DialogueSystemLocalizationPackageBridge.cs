using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace PixelCrushers.DialogueSystem.LocalizationPackageSupport
{

    /// <summary>
    /// Reads localized actor display names and dialogue entry text from
    /// Localization Package string table.
    /// </summary>
    [AddComponentMenu("Pixel Crushers/Dialogue System/UI/Misc/Dialogue System Localization Package Bridge")]
    public class DialogueSystemLocalizationPackageBridge : MonoBehaviour
    {

        public List<LocalizedStringTable> localizedStringTables;
        public Locale defaultLocale;
        public string uniqueFieldTitle = "Guid";

        [Tooltip("When Dialogue System attempts to localize non-dialogue text, use localizsed string tables instead of Dialogue System's default behavior of using Text Table assets.")]
        public bool replaceGetLocalizedText = false;

        private List<UnityEngine.Localization.Tables.StringTable> tables = new List<UnityEngine.Localization.Tables.StringTable>();

        private IEnumerator Start()
        {
            yield return LocalizationSettings.InitializationOperation;
            yield return new WaitForEndOfFrame();
            CacheStringTables();
            UpdateActorDisplayNames();
            Localization.language = LocalizationSettings.SelectedLocale.Identifier.Code;
            LocalizationSettings.SelectedLocaleChanged += OnSelectedLocaleChanged;

            if (replaceGetLocalizedText && DialogueManager.instance.overrideGetLocalizedText == null)
            {
                DialogueManager.instance.overrideGetLocalizedText = GetLocalizedTextFromStringTables;
            }
        }

        private void OnDestroy()
        {
            LocalizationSettings.SelectedLocaleChanged -= OnSelectedLocaleChanged;
        }

        public void CacheStringTables()
        {
            tables.Clear();
            foreach (var table in localizedStringTables)
            {
                if (table != null)
                {
                    tables.Add(table.GetTable());
                }
            }
        }

        private void OnSelectedLocaleChanged(Locale obj)
        {
            if (!Application.isPlaying) return;
            CacheStringTables();
            UpdateActorDisplayNames();
            Localization.language = LocalizationSettings.SelectedLocale.Identifier.Code;
        }

        public void UpdateActorDisplayNames()
        {
            var locale = LocalizationSettings.SelectedLocale;
            Localization.language = locale.Identifier.Code;
            foreach (var actor in DialogueManager.masterDatabase.actors)
            {
                var guid = actor.LookupValue(uniqueFieldTitle);
                if (!string.IsNullOrEmpty(guid))
                {
                    foreach (var table in tables)
                    {
                        var stringTableEntry = table[guid];
                        if (stringTableEntry != null)
                        {
                            var fieldTitle = (locale == defaultLocale) ? "Display Name" : ("Display Name " + locale.Identifier.Code);
                            DialogueLua.SetActorField(actor.Name, fieldTitle, stringTableEntry.LocalizedValue);
                            break;
                        }
                    }
                }
            }
        }

        public void OnBarkLine(Subtitle subtitle)
        {
            LocalizeSubtitle(subtitle);
        }

        public void OnConversationLine(Subtitle subtitle)
        {
            LocalizeSubtitle(subtitle);
        }

        public void LocalizeSubtitle(Subtitle subtitle)
        {
            if (string.IsNullOrEmpty(subtitle.formattedText.text)) return;
            var guid = Field.LookupValue(subtitle.dialogueEntry.fields, uniqueFieldTitle);
            foreach (var table in tables)
            {
                var stringTableEntry = table[guid];
                if (stringTableEntry != null)
                {
                    var localizedValue = stringTableEntry.LocalizedValue;
                    subtitle.formattedText = FormattedText.Parse(localizedValue);
                    break;
                }
            }
        }

        public void OnConversationResponseMenu(Response[] responses)
        {
            foreach (Response response in responses)
            {
                var guid = Field.LookupValue(response.destinationEntry.fields, uniqueFieldTitle);
                foreach (var table in tables)
                {
                    var stringTableEntry = table[guid + "_MenuText"];
                    if (stringTableEntry != null)
                    {
                        var localizedValue = (stringTableEntry != null) ? stringTableEntry.LocalizedValue : table[guid].LocalizedValue;
                        response.formattedText = FormattedText.Parse(localizedValue);
                        break;
                    }
                }
            }
        }

        private string GetLocalizedTextFromStringTables(string s)
        {
            foreach (var table in tables)
            {
                var stringTableEntry = table[s];
                if (stringTableEntry != null)
                {
                    return stringTableEntry.LocalizedValue;
                }
            }
            return s;
        }

    }
}
