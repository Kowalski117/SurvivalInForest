using TMPro;
using UnityEngine;

public class ChestView : MonoBehaviour
{
    [SerializeField] private ChestHandler _handler;

    [SerializeField] private TMP_Text _labelText;

    private const string _bonusChestLabel = "Бонусный сундук";
    private const string _survivalChestLabel = "Cундук";

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
            _labelText.text = _bonusChestLabel;
        else if(type == ChestType.SurvivalChest)
            _labelText.text = _survivalChestLabel;
    }
}
