using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Interactor : Raycast
{
    [SerializeField] private LayerMask _interactionInventoryLayer;
    [SerializeField] private LayerMask _interactionItemLayer;
    [SerializeField] private LayerMask _interactionConstructionLayer;

    [SerializeField] private PlayerInventoryHolder _playerInventoryHolder;
    [SerializeField] private PlayerInputHandler _playerInputHandler;
    [SerializeField] private SaveItemHandler _saveItemHandler;
    [SerializeField] private BuildTool _buildTool;
    [SerializeField] private HotbarDisplay _hotbarDisplay;
    [SerializeField] private ClothesSlotsHandler _clothesSlotsHandler;
    [SerializeField] private PlayerAnimatorHandler _playerAnimation;
    [SerializeField] private InventoryOperator _inventoryOperator;
    [SerializeField] private SleepPanel _sleepPanel;

    [SerializeField] private float _liftingDelay = 2f;
    [SerializeField] private Transform _removeItemPoint;
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private PlayerPosition _playerPositionLastScene;
    [SerializeField] private PlayerAudioHandler _playerAudioHandler;
    [SerializeField] private bool _isKeyPickUp = true;

    private AudioSource _audioSource; 
    private bool _isStartingPick = true;
    private float _lookTimer = 0;
    private ItemPickUp _currentItemPickUp;
    private ObjectPickUp _currentObjectPickUp;
    private bool _isIconFilled = false;
    private bool _isInventoryFull = false;
    private SleepPointSaveData _sleepPointSaveData;
    private SleepingPlace _currentSleepingPlace;
    private Fire _currentFire;

    private int _addAmount = 1;

    public event Action<float, string> OnTimeUpdate;
    public event Action OnInteractionStarted;
    public event Action OnInteractionFinished;

    public float LookTimerPracent => _lookTimer / _liftingDelay;
    public PlayerInventoryHolder PlayerInventoryHolder => _playerInventoryHolder;
    public SleepPointSaveData SleepPointSaveData => _sleepPointSaveData;
    public Fire CurrentFire => _currentFire;

    protected override void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        base.Awake();

        if (_sleepPointSaveData.Position == Vector3.zero)
        {
            foreach (var sleepPosition in _playerPositionLastScene.SleepPositions)
            {
                if (SceneManager.GetActiveScene().buildIndex == sleepPosition.SceneIndex)
                {
                    _sleepPointSaveData = new SleepPointSaveData(sleepPosition.Position, sleepPosition.Rotation);
                }
            }
        }
    }

    private void OnEnable()
    {
        _playerInputHandler.InteractionPlayerInput.OnPickUp += PickUpItem;
        //_playerInputHandler.InventoryPlayerInput.OnToggleIInteractable += InteractableInventory;
        _playerInputHandler.InteractionPlayerInput.OnInteractedConstruction += InteractableConstruction;
        _playerInputHandler.InteractionPlayerInput.OnAddedFire += AddFire;

        _hotbarDisplay.OnItemClicked += PlantSeed;

        SaveGame.OnSaveGame += Save;
        SaveGame.OnLoadData += Load;
    }

    private void OnDisable()
    {
        _playerInputHandler.InteractionPlayerInput.OnPickUp -= PickUpItem;
        //_playerInputHandler.InventoryPlayerInput.OnToggleIInteractable -= InteractableInventory;
        _playerInputHandler.InteractionPlayerInput.OnInteractedConstruction -= InteractableConstruction;
        _playerInputHandler.InteractionPlayerInput.OnAddedFire -= AddFire;

        _hotbarDisplay.OnItemClicked -= PlantSeed;

        SaveGame.OnSaveGame -= Save;
        SaveGame.OnLoadData -= Load;
    }

    private void Update()
    {
        HandleLookTimer();
        HandleInventoryFull();
    }

    private void HandleLookTimer()
    {
        if (IsRayHittingSomething(_interactionItemLayer, out RaycastHit hitInfo))
        {
            if (_isStartingPick && !_isInventoryFull)
            {
                _lookTimer += Time.deltaTime;

                OnTimeUpdate?.Invoke(LookTimerPracent, "");

                if (_lookTimer >= _liftingDelay && !_isIconFilled)
                {
                    _isIconFilled = true;

                    if (hitInfo.collider.TryGetComponent(out ItemPickUp itemPickUp) && itemPickUp.enabled)
                    {
                        _currentItemPickUp = itemPickUp;
                    }
                    else if (hitInfo.collider.TryGetComponent(out ObjectPickUp objectPickUp))
                    {
                        _currentObjectPickUp = objectPickUp;
                    }

                    if (!_isKeyPickUp)
                    {
                        _playerAnimation.PickUp();
                        PickUpAninationEvent();
                    }
                }
                else
                {
                    _isIconFilled = false;
                }
            }
            else
            {
                ResetLookTimer();
            }
        }
        else if (IsRayHittingSomething(_interactionConstructionLayer, out RaycastHit hit))
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

                    if(_currentFire.IsFire)
                        OnInteractionStarted?.Invoke();
                }
            }
        }
        else
        {
            ResetLookTimer();

            if (_currentSleepingPlace)
            {
                _currentSleepingPlace = null;
                OnInteractionFinished?.Invoke();
            }
            else if (_currentFire)
            {
                _currentFire = null;
                OnInteractionFinished?.Invoke();
            }
        }
    }

    private void HandleInventoryFull()
    {
        if (_isInventoryFull)
        {
            ResetLookTimer();
        }
    }

    private void ResetLookTimer()
    {
        _lookTimer = 0;
        OnTimeUpdate?.Invoke(0f, "");
        _isIconFilled = false;
        _isInventoryFull = false;
    }

    public void PickUpAninationEvent()
    {
        _isIconFilled = false;
        _isStartingPick = true;

        if (_currentItemPickUp != null)
        {
            if (!_playerInventoryHolder.AddToInventory(_currentItemPickUp.ItemData, _addAmount, _currentItemPickUp.Durability))
            {
                _inventoryOperator.StartCreateItems(_currentItemPickUp.ItemData, _currentItemPickUp.ItemData.Durability, _addAmount);
                _isInventoryFull = true;
            }
            _currentItemPickUp.PickUp();
            _currentItemPickUp = null;
        }
        else if (_currentObjectPickUp != null)
        {
            foreach (var inventoryData in _currentObjectPickUp.ObjectItemsData.Items)
            {
                for (int i = 0; i < inventoryData.Items.Length; i++)
                {
                    for (int j = 0; j < inventoryData.Items[i].Amount; j++)
                    {
                        if (!_playerInventoryHolder.AddToInventory(inventoryData.Items[i].ItemData, _addAmount, inventoryData.Items[i].ItemData.Durability))
                        {
                            _inventoryOperator.StartCreateItems(inventoryData.Items[i].ItemData, inventoryData.Items[i].ItemData.Durability, _addAmount);
                        }
                    }
                }
            }
            _currentObjectPickUp.PicUp();
            _currentObjectPickUp = null;
        }
        _audioSource.PlayOneShot(_playerAudioHandler.PickUpClip);
    }

    public void StartPickUpAninationEvent()
    {
        _isStartingPick = false;
    }
    
    //private void InteractableInventory()
    //{
    //    if (IsRayHittingSomething(_interactionConstructionLayer, out RaycastHit hitInfo))
    //    {
    //        if (hitInfo.collider.TryGetComponent(out IInteractable interactable))
    //        {
    //            interactable.Interact();
    //        }
    //    }
    //}

    private void InteractableConstruction()
    {
        if (_currentSleepingPlace)
        {
            _sleepPanel.DisplaySleepWindow();
            _sleepPointSaveData = new SleepPointSaveData(_playerTransform.position, _playerTransform.rotation);
        }
    }

    private void AddFire()
    {
        if (_currentFire)
        {
            InventorySlot slot = _hotbarDisplay.GetInventorySlotUI().AssignedInventorySlot;

            if (_currentFire.AddFire(slot))
            {
                _playerInventoryHolder.RemoveInventory(slot, _addAmount);
            }
        }
    }

    private void PickUpItem()
    {
        if((_currentItemPickUp || _currentObjectPickUp) && _isKeyPickUp)
        {
            _playerAnimation.PickUp();
            PickUpAninationEvent();
        }
    }

    private void PlantSeed(InventorySlot slot)
    {
        if (IsRayHittingSomething(_interactionConstructionLayer, out RaycastHit hitInfo))
        {
            if (hitInfo.collider.TryGetComponent(out GardenBed gardenBed))
            {
                if (gardenBed.StartGrowingSeed(slot.ItemData))
                {
                    _playerInventoryHolder.RemoveInventory(slot, _addAmount);
                }
            }
        }
    }

    private void Save()
    {
        ES3.Save(SaveLoadConstants.SpawnPosition + SceneManager.GetActiveScene().buildIndex, _sleepPointSaveData);
    }

    private void Load()
    {
        if (ES3.KeyExists(SaveLoadConstants.SpawnPosition + SceneManager.GetActiveScene().buildIndex))
        {
            _sleepPointSaveData = ES3.Load<SleepPointSaveData>(SaveLoadConstants.SpawnPosition + SceneManager.GetActiveScene().buildIndex);
            return;
        }
    }
}

[System.Serializable]
public struct SleepPointSaveData
{
    [SerializeField] private Vector3 _position;
    [SerializeField] private Quaternion _rotation;

    public Vector3 Position => _position;
    public Quaternion Rotation => _rotation;

    public SleepPointSaveData(Vector3 position, Quaternion rotation)
    {
        _position = position;
        _rotation = rotation;
    }
}