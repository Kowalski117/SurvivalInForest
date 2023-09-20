using UnityEngine;

[RequireComponent(typeof(RewardSlotView))]
public class RewardSlot : MonoBehaviour
{
    [SerializeField] private InventorySlotUI _slot;

    private RewardSlotView _view;
    private bool _isEmpty = true;

    public bool IsEmpty => _isEmpty;
    public InventorySlotUI Slot => _slot;

    private void Awake()
    {
        _view = GetComponent<RewardSlotView>();
    }

    public void Init(DayReward dayReward, int day)
    {
        _isEmpty = false;
        _slot.AssignedInventorySlot.AssignItem(dayReward.Slot.ItemData, dayReward.Slot.Amount, dayReward.Slot.ItemData.Durability);
        _slot.UpdateUiSlot();
        _view.UpdateSlot(day, dayReward.BackgroundColor);
    }

    public void ToggleSlot(bool isActive)
    {
        _view.ToggleFrameImage(isActive);
    }

    public void TakeSlot()
    {
        _view.UseSlot();
    }

    public void ResetSlot()
    {
        _view.ResetSlot();
    }
}
