using UnityEngine;
using UnityEngine.Events;

public class BuildPlayerInput : MonoBehaviour
{
    private PlayerInput _playerInput;

    public event UnityAction OnPutBuilding;
    public event UnityAction OnRotateBuilding;
    public event UnityAction OnDeleteModeBuilding;
    public event UnityAction OnDeleteBuilding;

    private void Awake()
    {
        _playerInput = new PlayerInput();
    }

    private void OnEnable()
    {
        _playerInput.Enable();
        _playerInput.BuildSystem.PutBuilding.performed += ctx => PutBuilding();
        _playerInput.BuildSystem.RotateBuilding.performed += ctx => RotateBuilding();
        _playerInput.BuildSystem.DeleteModeBuilding.performed += ctx => DeleteModeBuilding();
        _playerInput.BuildSystem.DeleteBuilding.performed += ctx => DeleteBuilding();
    }

    private void OnDisable()
    {
        _playerInput.BuildSystem.PutBuilding.performed -= ctx => PutBuilding();
        _playerInput.BuildSystem.RotateBuilding.performed -= ctx => RotateBuilding();
        _playerInput.BuildSystem.DeleteModeBuilding.performed -= ctx => DeleteModeBuilding();
        _playerInput.BuildSystem.DeleteBuilding.performed -= ctx => DeleteBuilding();
        _playerInput.Disable();
    }

    private void PutBuilding()
    {
        OnPutBuilding?.Invoke();
    }

    private void RotateBuilding()
    {
        OnRotateBuilding?.Invoke();
    }

    private void DeleteModeBuilding()
    {
        OnDeleteModeBuilding?.Invoke();
    }

    private void DeleteBuilding()
    {
        OnDeleteBuilding?.Invoke();
    }
}
