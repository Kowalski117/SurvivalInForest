using UnityEngine;
using UnityEngine.Events;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
    public class StarterAssetsInputs : MonoBehaviour
    {
        [Header("Character Input Values")]
        public Vector2 move;
        public Vector2 look;
        public bool jump;
        public bool sprint;
        public bool stealth;
        public bool interact;
        public bool attack;

        [Header("Movement Settings")]
        public bool analogMovement;
        //private PlayerInput _playerInput;

        //[Header("Mouse Cursor Settings")]
        //public bool cursorLocked = true;
        //public bool cursorInputForLook = true;

        //public event UnityAction OnToggleStealth;

        //private void Awake()
        //{
        //    _playerInput = new PlayerInput();
        //}

        //private void OnEnable()
        //{
        //    _playerInput.Enable();
        //    _playerInput.Player.Stealth.performed += ctx => StealthInput();
        //}

        //private void OnDisable()
        //{
        //    _playerInput.Player.Stealth.performed -= ctx => StealthInput();
        //    _playerInput.Disable();
        //}

#if ENABLE_INPUT_SYSTEM
        public void OnMove(InputValue value)
        {
            MoveInput(value.Get<Vector2>());
        }

        public void OnLook(InputValue value)
        {
            //if (cursorInputForLook)
            {
                LookInput(value.Get<Vector2>());
            }
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

        //public void OnAttack(InputValue value)
        //{
        //    AttackInput(value.isPressed);
        //}
#endif

        public void MoveInput(Vector2 newMoveDirection)
        {
            move = newMoveDirection;
        }

        public void LookInput(Vector2 newLookDirection)
        {
            look = newLookDirection;
        }

        public void JumpInput(bool newJumpState)
        {
            jump = newJumpState;
        }

        public void SprintInput(bool newSprintState)
        {
            sprint = newSprintState;
        }

        public void StealthInput(bool newStealthState)
        {
            stealth = !stealth;
        }

        public void InteractInput(bool newInteractState)
        {
            interact = newInteractState;
        }

        public void AttackInput(bool newAttackState)
        {
            attack = newAttackState;
        }

        //private void OnApplicationFocus(bool hasFocus)
        //{
        //    SetCursorState(cursorLocked);
        //}

        //public void SetCursorState(bool newState)
        //{
        //    Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
        //}
    }
}