using UnityEngine;

public class BuildHandler : MonoBehaviour
{
    [SerializeField] private PlayerHandler _playerInputHandler;
    [SerializeField] private UIInventoryHandler _inventoryHandler;
    [SerializeField] private BuildTool _buildTool;

    private bool _isTurnOffWindows = false;

    private void OnEnable()
    {
        _playerInputHandler.BuildPlayerInput.OnDeleteBuilding += EnableWindows;
        _buildTool.OnCreateBuilding += TurnOffWindows;
        _buildTool.OnCompletedBuilding += EnableWindows;
        _buildTool.OnDestroyBuilding += UnplugWindow;
    }

    private void OnDisable()
    {
        _playerInputHandler.BuildPlayerInput.OnDeleteBuilding -= EnableWindows;
        _buildTool.OnCreateBuilding -= TurnOffWindows;
        _buildTool.OnCompletedBuilding -= EnableWindows;
        _buildTool.OnDestroyBuilding -= UnplugWindow;
    }

    private void TurnOffWindows()
    {
        _isTurnOffWindows = !_isTurnOffWindows;

        if (_isTurnOffWindows)
        {
            _inventoryHandler.TurnOffDisplayInventory();
            //_playerInputHandler.ToggleBuildPlayerInput(true);
            _playerInputHandler.ToggleInteractionInput(false);
            //_playerInputHandler.SetCursorVisible(false); //вот ту тпоходу дич
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
            //_playerInputHandler.ToggleBuildPlayerInput(false);
            _playerInputHandler.ToggleInteractionInput(true);
            _isTurnOffWindows = false;
        }
    }
}
