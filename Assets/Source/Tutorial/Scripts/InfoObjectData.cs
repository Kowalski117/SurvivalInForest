using PixelCrushers.DialogueSystem;
using PixelCrushers.Wrappers;
using UnityEngine;

[CreateAssetMenu(menuName = "Tutorial/Info Object Data", order = 51)]
public class InfoObjectData : ScriptableObject
{
    [SerializeField] private string _nameFieldTextTable;
    [SerializeField] private TextTable _textTableName;
    [SerializeField] private TextTable _textTableDescription;
    [SerializeField] private Sprite _sprite;
    [SerializeField] private bool _isDone;

    public string Name => _textTableName.GetFieldTextForLanguage(_nameFieldTextTable,Localization.language);
    public string Description => _textTableDescription.GetFieldTextForLanguage(_nameFieldTextTable,Localization.language);
    public Sprite Sprite => _sprite;
    public bool IsDone => _isDone;

    public void Init()
    {
        _isDone = PlayerPrefs.GetInt(SaveLoadConstants.IsDone + _nameFieldTextTable, 0) == 1;
        PlayerPrefs.SetInt(SaveLoadConstants.IsDone + _nameFieldTextTable, _isDone ? 1 : 0);
    }

    public void SetIsDone(bool isDone)
    {
        _isDone = isDone;
        PlayerPrefs.SetInt(SaveLoadConstants.IsDone + _nameFieldTextTable, isDone ? 1 : 0);
    }
}