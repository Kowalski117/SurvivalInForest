using PixelCrushers.DialogueSystem;
using PixelCrushers.Wrappers;
using TMPro;
using UnityEngine;

public class ChestView : MonoBehaviour
{
    [SerializeField] private ChestHandler _handler;

    [SerializeField] private TMP_Text _labelText;
    [SerializeField] private TextTable _textTable;
   
    private void OnEnable()
    {
        _handler.OnChestTypeChanged += ChangeType;
    }

    private void OnDisable()
    {
        _handler.OnChestTypeChanged -= ChangeType;
    }

    private void ChangeType(ChestType type)
    {
        if (type == ChestType.BonusChest)
            _labelText.text = _textTable.GetFieldTextForLanguage(GameConstants.BonusChest,Localization.language);
        else if(type == ChestType.SurvivalChest)
            _labelText.text =  _textTable.GetFieldTextForLanguage(GameConstants.Chest, Localization.language);
    }
}
