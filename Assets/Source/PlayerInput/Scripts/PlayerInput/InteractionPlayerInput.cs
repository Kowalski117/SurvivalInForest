using System;

public class InteractionPlayerInput : PlayerInputAction
{
    private bool _isEnable = true;

    public event Action OnConstructionInteracted;
    public event Action OnFireAdded;
    public event Action OnPickedUp;
    public event Action<bool> OnAttacked;
    public event Action OnUsed;
    public event Action OnAimed;
    public event Action OnNoteOpened;

    protected override void OnEnable()
    {
        base.OnEnable();

        PlayerInput.Player.InteractionConstruction.performed += ctx => InteractConstruction();
        PlayerInput.Player.AddFire.performed += ctx => AddFire();
        PlayerInput.Player.PickUp.performed += ctx => PickUp();
        PlayerInput.Player.OpenNote.performed += ctx => OpenNote();
        PlayerInput.WeaponSystem.Attack.performed += ctx => Attack(PlayerInput.WeaponSystem.Attack.IsPressed());
        PlayerInput.WeaponSystem.Use.performed += ctx => Use();
        PlayerInput.WeaponSystem.Aim.performed += ctx => Aim();
    }

    protected override void OnDisable()
    {
        PlayerInput.Player.InteractionConstruction.performed -= ctx => InteractConstruction();
        PlayerInput.Player.AddFire.performed -= ctx => AddFire();
        PlayerInput.Player.PickUp.performed -= ctx => PickUp();
        PlayerInput.Player.OpenNote.performed -= ctx => OpenNote();
        PlayerInput.WeaponSystem.Attack.performed -= ctx => Attack(PlayerInput.WeaponSystem.Attack.IsPressed());
        PlayerInput.WeaponSystem.Use.performed -= ctx => Use();
        PlayerInput.WeaponSystem.Aim.performed -= ctx => Aim();
        
        base.OnDisable();
    }

    public void InteractConstruction()
    {
        if(_isEnable)
            OnConstructionInteracted?.Invoke();
    }

    public void AddFire()
    {
        if (_isEnable)
            OnFireAdded?.Invoke();
    }

    public void OpenNote()
    {
        if (_isEnable)
            OnNoteOpened?.Invoke();
    }

    private void PickUp()
    {
        if (_isEnable)
            OnPickedUp?.Invoke();
    }

    public void Attack(bool isPressed)
    {
        if (_isEnable)
            OnAttacked?.Invoke(isPressed);
        else
            OnAttacked?.Invoke(false);
    }

    public void Use()
    {
        if (_isEnable)
            OnUsed?.Invoke();
    }

    public void Aim()
    {
        if (_isEnable)
            OnAimed?.Invoke();
    }

    public void SetEnable(bool enable)
    {
        _isEnable = enable;
    }
}
