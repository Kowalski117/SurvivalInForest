using System;
using UnityEngine;

public class InteractorConstruction : Raycast
{
    [SerializeField] private LayerMask _interactionConstructionLayer;

    [SerializeField] private PlayerInventoryHolder _playerInventoryHolder;
    [SerializeField] private PlayerHandler _playerInputHandler;
    [SerializeField] private HotbarDisplay _hotbarDisplay;
    [SerializeField] private SleepHandler _sleepHandler;

    [SerializeField] private Transform _playerTransform;

    private SleepingPlace _currentSleepingPlace;
    private Fire _currentFire;
    private GardenBed _currentGardenBed;
    private Note _note;

    private int _addAmount = 1;

    public event Action OnInteractionStarted;
    public event Action OnInteractionFinished;
    public event Action OnSeedPlanted;

    public Fire CurrentFire => _currentFire;
    public GardenBed CurrentGardenBed => _currentGardenBed;

    private void OnEnable()
    {
        _playerInputHandler.InteractionPlayerInput.OnConstructionInteracted += InteractableSleepingPlace;
        _playerInputHandler.InteractionPlayerInput.OnFireAdded += AddFire;
        _playerInputHandler.InteractionPlayerInput.OnNoteOpened += OpenNote;
        _hotbarDisplay.OnItemClicked += PlantSeed;
    }

    private void OnDisable()
    {
        _playerInputHandler.InteractionPlayerInput.OnConstructionInteracted -= InteractableSleepingPlace;
        _playerInputHandler.InteractionPlayerInput.OnFireAdded -= AddFire;
        _playerInputHandler.InteractionPlayerInput.OnNoteOpened -= OpenNote;
        _hotbarDisplay.OnItemClicked -= PlantSeed;
    }

    private void Update()
    {
        HandleInteraction();
    }

    private void HandleInteraction()
    {
        if (IsRayHittingSomething(_interactionConstructionLayer, out RaycastHit hit))
        {
            if (hit.collider.TryGetComponent(out SleepingPlace interactable))
            {
                if (!_currentSleepingPlace)
                {
                    _currentSleepingPlace = interactable;
                    OnInteractionStarted?.Invoke();
                }
            }
            else if (hit.collider.TryGetComponent(out Fire fire))
            {
                if (!_currentFire)
                {
                    _currentFire = fire;

                    if (_currentFire.IsFire)
                        OnInteractionStarted?.Invoke();
                }
            }
            else if (hit.collider.TryGetComponent(out GardenBed gardenBed))
            {
                if (!_currentGardenBed)
                {
                    _currentGardenBed = gardenBed;
                    OnInteractionStarted?.Invoke();
                }
            }
            else if (hit.collider.TryGetComponent(out Note note))
            {
                if (!_note)
                {
                    _note = note;
                    OnInteractionStarted?.Invoke();
                }
            }
        }
        else
        {
            if (_currentSleepingPlace || _currentFire || _currentGardenBed || _note)
            {
                _currentSleepingPlace = null;
                _currentFire = null;
                _currentGardenBed = null;
                _note = null;
                OnInteractionFinished?.Invoke();
            }
        }
    }

    private void InteractableSleepingPlace()
    {
        if (_currentSleepingPlace)
            _sleepHandler.SetPoint(_playerTransform);
    }

    private void AddFire()
    {
        if (_currentFire)
        {
            InventorySlot slot = _hotbarDisplay.GetInventorySlotUI().AssignedInventorySlot;

            if (_currentFire.AddSlot(slot))
                _playerInventoryHolder.RemoveSlot(slot, _addAmount);
        }
    }

    private void PlantSeed(InventorySlot slot)
    {
        if (_currentGardenBed && _currentGardenBed.StartGrowingSeed(slot.ItemData))
        {
            _playerInventoryHolder.RemoveSlot(slot, _addAmount);
            OnSeedPlanted?.Invoke();
        }
    }

    private void OpenNote()
    {
        if(_note)
            _note.Init();
    }
}