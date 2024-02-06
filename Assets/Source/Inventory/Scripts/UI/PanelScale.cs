using UnityEngine;

public class PanelScale : MonoBehaviour
{
    [SerializeField] private DynamicInventoryDisplay _dynamicInventory;
    [SerializeField] private OptionScale[] _optionScales;
    [SerializeField] private RectTransform _panel;

    private void OnEnable()
    {
        _dynamicInventory.OnDisplayRefreshed += ChangeScale;
    }

    private void OnDisable()
    {
        _dynamicInventory.OnDisplayRefreshed -= ChangeScale;
    }

    private void ChangeScale(int slotCount)
    {
        foreach (var item in _optionScales)
        {
            if(item.SlotCount == slotCount)
            {
                Vector2 sizeDelta = _panel.sizeDelta;
                Vector2 position = _panel.localPosition;
                sizeDelta.y = item.Height;
                position.y = item.PositionY;
                _panel.sizeDelta = sizeDelta;
                _panel.localPosition = position;
                break;
            }
        }
    }
} 

[System.Serializable]
public struct OptionScale
{
    [SerializeField] private int _slotCount;
    [SerializeField] private int _height;
    [SerializeField] private int _positionY;

    public int SlotCount => _slotCount;
    public int Height => _height;
    public int PositionY => _positionY;

}
