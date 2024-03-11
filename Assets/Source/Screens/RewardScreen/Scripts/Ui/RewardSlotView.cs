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
    private int _day;

    public void Init(int day, Color color)
    {
        _day = day;
        _dayText.text = _textTable.GetFieldTextForLanguage(GameConstants.Day, Localization.language) + " " + _day.ToString();
        _frameImage.color = color;
        _defoultColor = _frameImage.color;
    }

    public void UpdateInfo()
    {
        _dayText.text = _textTable.GetFieldTextForLanguage(GameConstants.Day, Localization.language) + " " + _day.ToString();
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