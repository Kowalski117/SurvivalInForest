using UnityEngine;

public class PlayerInputAction : MonoBehaviour
{
    protected PlayerInput PlayerInput;

    private void Awake()
    {
        PlayerInput = new PlayerInput();
    }

    protected virtual void OnEnable()
    {
        PlayerInput.Enable();
    }

    protected virtual void OnDisable()
    {
        PlayerInput.Disable();
    }
}
