using UnityEngine;

public class BuildHandler : MonoBehaviour
{
    [SerializeField] private PlayerInputHandler _playerInputHandler;
    [SerializeField] private BuildTool _buildTool;

    private bool _isTurnOffWindows = false;

    private void OnEnable()
    {
        _playerInputHandler.BuildPlayerInput.OnDeleteBuilding += EnableWindows;
        _buildTool.OnCreateBuild += TurnOffWindows;
        _buildTool.OnCompletedBuild += EnableWindows;
        _buildTool.OnDestroyBuild += UnplugWindow;
    }

    private void OnDisable()
    {
        _playerInputHandler.BuildPlayerInput.OnDeleteBuilding -= EnableWindows;
        _buildTool.OnCreateBuild -= TurnOffWindows;
        _buildTool.OnCompletedBuild -= EnableWindows;
        _buildTool.OnDestroyBuild -= UnplugWindow;
    }

    private void TurnOffWindows()
    {
        _isTurnOffWindows = !_isTurnOffWindows;

        if (_isTurnOffWindows)
        {
            _playerInputHandler.InventoryPlayerInput.ToggleInventory();
            _playerInputHandler.SetCursorVisible(false);
            _playerInputHandler.ToggleBuildPlayerInput(true);
            //_playerInputHandler.ToggleInventoryInput(false);
            _playerInputHandler.ToggleInteractionInput(false);
            //_buildTool.SetDeleteModeEnabled(false);
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
            _playerInputHandler.ToggleBuildPlayerInput(false);
            //_playerInputHandler.ToggleInventoryInput(true);
            _playerInputHandler.ToggleInteractionInput(true);
            _isTurnOffWindows = false;
        }
    }
}
