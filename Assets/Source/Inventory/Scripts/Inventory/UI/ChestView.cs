using PixelCrushers.DialogueSystem;
using PixelCrushers.Wrappers;
using TMPro;
using UnityEngine;

public class ChestView : MonoBehaviour
{
    [SerializeField] private ChestHandler _handler;

    [SerializeField] private TMP_Text _labelText;
    [SerializeField] private TextTable _textTable;
    
    private const string _bonusChestNameTextTable = "BonusСhest";
    private const string _survivalChesNameTextTable = "Chest";
   
    private void OnEnable()
    {
        _handler.OnChestTypeChanged += ChangeChestType;
    }

    private void OnDisable()
    {
        _handler.OnChestTypeChanged -= ChangeChestType;
    }

    private void ChangeChestType(ChestType type)
    {
        if (type == ChestType.BonusChest)
            _labelText.text = _textTable.GetFieldTextForLanguage(_bonusChestNameTextTable,Localization.language);
        else if(type == ChestType.SurvivalChest)
            _labelText.text =  _textTable.GetFieldTextForLanguage(_survivalChesNameTextTable,Localization.language);
    }
}
