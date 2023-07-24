using UnityEngine;

namespace StarterAssets
{
    public class UICanvasControllerInput : MonoBehaviour
    {
        [SerializeField] private PlayerInputHandler _playerInputHandler;
        [SerializeField] private InventoryPlayerInput _inventoryPlayerInput;

        [Header("Output")]
        public StarterAssetsInputs starterAssetsInputs;

        public void VirtualMoveInput(Vector2 virtualMoveDirection)
        {
            starterAssetsInputs.MoveInput(virtualMoveDirection);
        }

        public void VirtualLookInput(Vector2 virtualLookDirection)
        {
            starterAssetsInputs.LookInput(virtualLookDirection);
        }

        public void VirtualSprintInput(bool virtualSprintState)
        {
            starterAssetsInputs.SprintInput(virtualSprintState);
        }

        public void VirtualStealthInput(bool virtualSprintState)
        {
            starterAssetsInputs.StealthInput(virtualSprintState);
        }

        public void Attack(bool virtualSprintState)
        {
            _playerInputHandler.InteractionPlayerInput.Attack();
        }

        public void ToggleInventory()
        {
            _playerInputHandler.InventoryPlayerInput.ToggleInventory();
            _playerInputHandler.InventoryPlayerInput.ToggleIInteractable();
        }

        public void InteractedConstruction()
        {
            _playerInputHandler.InteractionPlayerInput.InteractedConstruction();
        }
    }
}
