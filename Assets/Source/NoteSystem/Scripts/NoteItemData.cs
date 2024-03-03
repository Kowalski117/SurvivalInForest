using PixelCrushers.DialogueSystem;
using PixelCrushers.Wrappers;
using UnityEngine;

[CreateAssetMenu(menuName = "Note", order = 51)]
public class NoteItemData : ScriptableObject
{
    [SerializeField] private string _textTableField;
    [SerializeField] private TextTable _textTable;

    public string Letter => _textTable.GetFieldTextForLanguage(_textTableField,Localization.language);
}
