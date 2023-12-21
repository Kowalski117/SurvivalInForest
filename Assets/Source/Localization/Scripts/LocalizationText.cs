using PixelCrushers.Wrappers;
using TMPro;
using UnityEngine;

public class LocalizationText : MonoBehaviour
{
    [SerializeField] private TextTable _textTable;
    [SerializeField] private string _fieldTextTable;
    [SerializeField] private TMP_Text _text;

    private void UpdateLanguage(string language)
    {
        _text.text = _textTable.GetFieldTextForLanguage(_fieldTextTable, language);
    }
}
