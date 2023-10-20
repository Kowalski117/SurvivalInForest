using UnityEngine;

public class Torch : MonoBehaviour
{
    [SerializeField] private PlayerInteraction _playerInteraction;
    [SerializeField] private PlayerAnimatorHandler _playerAnimatorHandler;

    private ToolItemData _currentTool;
    private bool _isEnable = false;
    private float _timer = 1f;
    private float _timerTime = 0f;

    private void OnEnable()
    {
        _playerInteraction.OnUpdateToolItemData += Init;
    }

    private void OnDisable()
    {
        _playerInteraction.OnUpdateToolItemData -= Init;
    }

    private void Update()
    {
        if (_isEnable && _currentTool != null)
        {
            _playerInteraction.UpdateDurabilityWithGameTime(_playerInteraction.CurrentInventorySlot);
        }
    }

    private void Init(ToolItemData itemData)
    {
        if (itemData != null && itemData.ToolType == ToolType.Torch)
        {
            _currentTool = itemData;
            _isEnable = true;
        }
        else
        {
            _currentTool = null;
            _isEnable = false;
        }
    }
}
