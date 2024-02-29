using System;

public class BuildPlayerInput : PlayerInputAction
{
    public event Action OnBuildingPutted;
    public event Action OnBuildingRotated;
    public event Action OnDeleteModeBuildingToggled;
    public event Action OnBuildingDeleted;

    protected override void OnEnable()
    {
        base.OnEnable();

        PlayerInput.BuildSystem.PutBuilding.performed += ctx => Put();
        PlayerInput.BuildSystem.RotateBuilding.performed += ctx => Rotate();
        PlayerInput.BuildSystem.DeleteModeBuilding.performed += ctx => ToggleDeleteMode();
        PlayerInput.BuildSystem.DeleteBuilding.performed += ctx => Delete();
    }

    protected override void OnDisable()
    {
        PlayerInput.BuildSystem.PutBuilding.performed -= ctx => Put();
        PlayerInput.BuildSystem.RotateBuilding.performed -= ctx => Rotate();
        PlayerInput.BuildSystem.DeleteModeBuilding.performed -= ctx => ToggleDeleteMode();
        PlayerInput.BuildSystem.DeleteBuilding.performed -= ctx => Delete();

        base.OnDisable();
    }

    public void Put()
    {
        OnBuildingPutted?.Invoke();
    }

    public void Rotate()
    {
        OnBuildingRotated?.Invoke();
    }

    public void ToggleDeleteMode()
    {
        OnDeleteModeBuildingToggled?.Invoke();
    }

    public void Delete()
    {
        OnBuildingDeleted?.Invoke();
    }
}
