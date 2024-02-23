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

    private const string _day = "Day";
    private Color _defoultColor;

    public void UpdateSlot(int day, Color color)
    {
        _dayText.text = _textTable.GetFieldTextForLanguage(_day, Localization.language) +" "+ day.ToString();
        _frameImage.color = color;
        _defoultColor = _frameImage.color;
    }

    public void ResetSlot()
    {
        _frameImage.color = _defoultColor;
    }

    public void UseSlot()
    {
        _frameImage.color = _usedSlotColor;
    }

    public void ToggleFrameImage(bool isActive)
    {
        if (isActive)
            _frameImage.color = _usedSlotColor;
        //else
        //    _frameImage.color = _usedSlotColor;
    }
}