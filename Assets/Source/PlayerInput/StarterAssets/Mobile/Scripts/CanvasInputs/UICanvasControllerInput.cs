using UnityEngine;

namespace StarterAssets
{
    public class UICanvasControllerInput : MonoBehaviour
    {
        [SerializeField] private WeaponPlayerInput _weaponPlayerInput;

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

        public void Hit()
        {
            _weaponPlayerInput.Shoot();
        }
    }
}
