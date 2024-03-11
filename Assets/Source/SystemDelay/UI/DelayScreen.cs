using DG.Tweening;
using PixelCrushers;
using PixelCrushers.DialogueSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DelayScreen : MonoBehaviour
{
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private TMP_Text _timerText;
    [SerializeField] private TMP_Text _actionText;
    [SerializeField] private Image _loadingBar;

    [SerializeField] private TextTable _textTable;

    private Tween _tweenGroup;

    public void UpdateText(string name, string timer, ActionType actionType)
    {
        _nameText.text = name;
        _timerText.text = timer;
        SetActionText(actionType);
    }

    public void FillAmount(float delay)
    {
        Crear();
        _tweenGroup = _loadingBar.DOFillAmount(1, delay);
    }

    private void SetActionText(ActionType actionType)
    {
        switch (actionType)
        {
            case ActionType.CraftItem:
                _actionText.text = _textTable.GetFieldTextForLanguage(GameConstants.Crafting, Localization.language);
                break;
            case ActionType.CraftBuild:
                _actionText.text = _textTable.GetFieldTextForLanguage(GameConstants.Build, Localization.language);
                break;
            case ActionType.Preparing:
                _actionText.text = _textTable.GetFieldTextForLanguage(GameConstants.Prepare, Localization.language);
                break;
            case ActionType.Sleep:
                _actionText.text = _textTable.GetFieldTextForLanguage(GameConstants.Sleeping, Localization.language);
                _nameText.text = GameConstants.EmptyLine;
                break;
        }
    }

    private void Crear()
    {
        _loadingBar.fillAmount = 0;
        _tweenGroup.Kill();
    }
}
