using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
    public class StarterAssetsInputs : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] private bool _isAnalogMovement;

        public Vector2 Move { get; private set; }
        public Vector2 Look { get; private set; }
        public bool Jump { get; private set; }
        public bool Sprint { get; private set; }
        public bool Stealth { get; private set; }
        public bool Interact { get; private set; }
        public bool Attack { get; private set; }

        public bool IsAnalogMovement => _isAnalogMovement;

#if ENABLE_INPUT_SYSTEM
        public void OnMove(InputValue value)
        {
            MoveInput(value.Get<Vector2>());
        }

        public void OnLook(InputValue value)
        {
            LookInput(value.Get<Vector2>());
        }

        public void OnJump(InputValue value)
        {
            JumpInput(value.isPressed);
        }

        public void OnSprint(InputValue value)
        {
            SprintInput(value.isPressed);
        }

        public void OnStealth(InputValue value)
        {
            StealthInput(value.isPressed);
        }

        public void OnInteract(InputValue value)
        {
            InteractInput(value.isPressed);
        }

        public void OnAttack(InputValue value)
        {
            AttackInput(value.isPressed);
        }
#endif

        public void MoveInput(Vector2 newMoveDirection)
        {
            Move = newMoveDirection;
        }

        public void LookInput(Vector2 newLookDirection)
        {
            Look = newLookDirection;
        }

        public void JumpInput(bool newJumpState)
        {
            Jump = newJumpState;
        }

        public void SprintInput(bool newSprintState)
        {
            Sprint = newSprintState;
        }

        public void StealthInput(bool newStealthState)
        {
            Stealth = !Stealth;
        }

        public void InteractInput(bool newInteractState)
        {
            Interact = newInteractState;
        }

        public void AttackInput(bool newAttackState)
        {
            Attack = newAttackState;
        }
    }
}