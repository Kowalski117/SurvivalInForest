using UnityEngine;

public class BuildHandler : MonoBehaviour
{
    [SerializeField] private PlayerHandler _playerInputHandler;
    [SerializeField] private UIInventoryHandler _inventoryHandler;
    [SerializeField] private BuildTool _buildTool;

    private bool _isTurnOffWindows = false;

    private void OnEnable()
    {
        _playerInputHandler.BuildPlayerInput.OnBuildingDeleted += EnableWindows;
        _buildTool.OnBuildingCreated += TurnOffWindows;
        _buildTool.OnBuildingCompleted += EnableWindows;
        _buildTool.OnBuildingDestroyed += UnplugWindow;
    }

    private void OnDisable()
    {
        _playerInputHandler.BuildPlayerInput.OnBuildingDeleted -= EnableWindows;
        _buildTool.OnBuildingCreated -= TurnOffWindows;
        _buildTool.OnBuildingCompleted -= EnableWindows;
        _buildTool.OnBuildingDestroyed -= UnplugWindow;
    }

    private void TurnOffWindows()
    {
        _isTurnOffWindows = !_isTurnOffWindows;

        if (_isTurnOffWindows)
        {
            _inventoryHandler.TurnOffDisplayInventory();
            _playerInputHandler.ToggleInteractionInput(false);
        }
    }

    private void UnplugWindow()
    {
        _isTurnOffWindows = false;
    }

    private void EnableWindows()
    {
        if (_isTurnOffWindows)
        {
            _playerInputHandler.ToggleInteractionInput(true);
            _isTurnOffWindows = false;
        }
    }
}
