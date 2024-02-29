using UnityEngine;

namespace StarterAssets
{
    public class UICanvasControllerInput : MonoBehaviour
    {
        [SerializeField] private PlayerHandler _playerInputHandler;
        [SerializeField] private InventoryPlayerInput _inventoryPlayerInput;
        [SerializeField] private StarterAssetsInputs _starterAssetsInputs;
        [SerializeField] private BuildTool _buildTool;

        public void VirtualMoveInput(Vector2 virtualMoveDirection)
        {
            _starterAssetsInputs.MoveInput(virtualMoveDirection);
        }

        public void VirtualLookInput(Vector2 virtualLookDirection)
        {
            _starterAssetsInputs.LookInput(virtualLookDirection);
        }

        public void VirtualSprintInput(bool virtualSprintState)
        {
            _starterAssetsInputs.SprintInput(virtualSprintState);
        }

        public void VirtualStealthInput(bool virtualSprintState)
        {
            _starterAssetsInputs.StealthInput(virtualSprintState);
        }

        public void Attack(bool isPressed)
        {
            _playerInputHandler.InteractionPlayerInput.Attack(isPressed);
        }

        public void ToggleInventory()
        {
            _playerInputHandler.InventoryPlayerInput.Toggle();
        }

        public void InteractedConstruction()
        {
            _playerInputHandler.InteractionPlayerInput.InteractConstruction();
        }

        public void AddFire()
        {
            _playerInputHandler.InteractionPlayerInput.AddFire();
        }

        public void RotateBuild()
        {
            _playerInputHandler.BuildPlayerInput.Rotate();
        }

        public void DestroyBuild()
        {
            _buildTool.DeleteBuilding();
        }

        public void RemoveBuildingMode()
        {
            _playerInputHandler.BuildPlayerInput.ToggleDeleteMode();
        }
        
        public void DeleteBuildingMode()
        {
            _playerInputHandler.BuildPlayerInput.Delete();
        }


        public void PutBuild()
        {
            _playerInputHandler.BuildPlayerInput.Put();
        }

        public void UseItem()
        {
            _playerInputHandler.HotbarDisplay.UseItem();
        }

        public void AimBow()
        {
            _playerInputHandler.InteractionPlayerInput.Aim();
        }

        public void ShootBow()
        {
            _playerInputHandler.InteractionPlayerInput.Use();
        }

        public void ToggleQuestJournal()
        {
            _playerInputHandler.ScreenPlayerInput.ToggleQuestJournal();
        }
    }
}
