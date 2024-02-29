using PixelCrushers.DialogueSystem;
using PixelCrushers.Wrappers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardSlotView : MonoBehaviour
{
    [SerializeField] private TMP_Text _dayText;
    [SerializeField] private Color _usedSlotColor;
    [SerializeField] private Image _frameImage;
    [SerializeField] private TextTable _textTable;

    private Color _defoultColor;

    public void UpdateInfo(int day, Color color)
    {
        _dayText.text = _textTable.GetFieldTextForLanguage(GameConstants.Day, Localization.language) + " " + day.ToString();
        _frameImage.color = color;
        _defoultColor = _frameImage.color;
    }

    public void Clear()
    {
        _frameImage.color = _defoultColor;
    }

    public void Use()
    {
        _frameImage.color = _usedSlotColor;
    }

    public void ToggleFrameImage(bool isActive)
    {
        if (isActive)
            _frameImage.color = _usedSlotColor;
        //else
        //    _frameImage.color = _defoultColor;
    }
}