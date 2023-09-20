using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardSlotView : MonoBehaviour
{
    [SerializeField] private TMP_Text _dayText;
    [SerializeField] private Image _imageBackground;
    [SerializeField] private Color _usedSlotColor;
    [SerializeField] private Image _frameImage;

    private Color _defoultColor;

    public void UpdateSlot(int day, Color color)
    {
        _dayText.text = day.ToString();
        _imageBackground.color = color;
        _defoultColor = color;
    }

    public void ResetSlot()
    {
        _imageBackground.color = _defoultColor;
    }

    public void UseSlot()
    {
        _imageBackground.color = _usedSlotColor;
    }

    public void ToggleFrameImage(bool isActive)
    {
        _frameImage.gameObject.SetActive(isActive);           
    }
}
