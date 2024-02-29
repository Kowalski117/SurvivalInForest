using PixelCrushers.QuestMachine;
using UnityEngine;
using UnityEngine.UI;

public class QuestJournalScreen : MonoBehaviour
{
    [SerializeField] private UIInventoryHandler _inventoryHandler;
    [SerializeField] private PlayerHandler _playerInputHandler;
    [SerializeField] private QuestJournal _questJournal;
    [SerializeField] private Button _exitButton;

    private bool _isOpen = false;   

    private void OnEnable()
    {
        _playerInputHandler.ScreenPlayerInput.OnQuestJournalToggled += ToggleActiveWindow;
        _exitButton.onClick.AddListener(ToggleActiveWindow);
    }

    private void OnDisable()
    {
        _playerInputHandler.ScreenPlayerInput.OnQuestJournalToggled -= ToggleActiveWindow;
        _exitButton.onClick.RemoveListener(ToggleActiveWindow);
    }

    private void ToggleActiveWindow()
    {
        _isOpen = !_isOpen;

        if (_isOpen)
        {
            if (_inventoryHandler.IsInventoryOpen)
                _playerInputHandler.InventoryPlayerInput.Toggle();

            _questJournal.ShowJournalUI();
            _playerInputHandler.SetCursorVisible(true);
            _playerInputHandler.ToggleHotbarDisplay(false);
            _playerInputHandler.ToggleInteractionInput(false);
            _playerInputHandler.ToggleInventoryInput(false);
            _playerInputHandler.ToggleBuildPlayerInput(false);
        }
        else
        {
            _playerInputHandler.SetCursorVisible(false);
            _playerInputHandler.ToggleInteractionInput(true);
            _playerInputHandler.ToggleHotbarDisplay(true);
            _playerInputHandler.ToggleInventoryInput(true);
            _playerInputHandler.ToggleBuildPlayerInput(true);
            _questJournal.HideJournalUI();
        }
    }
}
