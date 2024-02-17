using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Interactor : Raycast
{
    [SerializeField] private LayerMask _interactionInventoryLayer;
    [SerializeField] private LayerMask _interactionItemLayer;
    [SerializeField] private int _interactionItemLayerIndex;
    [SerializeField] private LayerMask _interactionConstructionLayer;

    [SerializeField] private PlayerInventoryHolder _playerInventoryHolder;
    [SerializeField] private PlayerHandler _playerInputHandler;
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
    private GardenBed _currentGardenBed;
    private Note _note;
    private WaitForSeconds _waitForSeconds;
    private Coroutine _coroutine;
    private List<ItemPickUp> _itemsPickUp = new List<ItemPickUp>();

    private int _addAmount = 1;

    public event UnityAction<float, string> OnTimeUpdate;
    public event UnityAction OnInteractionStarted;
    public event UnityAction OnInteractionFinished;
    public event UnityAction OnSeedPlanted;

    public float LookTimerPracent => _lookTimer / _liftingDelay;
    public PlayerInventoryHolder PlayerInventoryHolder => _playerInventoryHolder;
    public SleepPointSaveData SleepPointSaveData => _sleepPointSaveData;
    public Fire CurrentFire => _currentFire;
    public GardenBed CurrentGardenBed => _currentGardenBed;
    public Note Note => _note;

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
        _playerInputHandler.InteractionPlayerInput.OnInteractedConstruction += InteractableConstruction;
        _playerInputHandler.InteractionPlayerInput.OnAddedFire += AddFire;
        _playerInputHandler.InteractionPlayerInput.OnOpenNote += OpenNote;

        _hotbarDisplay.OnItemClicked += PlantSeed;

        SaveGame.OnSaveGame += Save;
        SaveGame.OnLoadData += Load;
    }

    private void OnDisable()
    {
        _playerInputHandler.InteractionPlayerInput.OnPickUp -= PickUpItem;
        _playerInputHandler.InteractionPlayerInput.OnInteractedConstruction -= InteractableConstruction;
        _playerInputHandler.InteractionPlayerInput.OnAddedFire -= AddFire;
        _playerInputHandler.InteractionPlayerInput.OnOpenNote -= OpenNote;

        _hotbarDisplay.OnItemClicked -= PlantSeed;

        SaveGame.OnSaveGame -= Save;
        SaveGame.OnLoadData -= Load;
    }

    private void Update()
    {
        HandleLookTimer();
        HandleInventoryFull();

        //if (_itemsPickUp.Count > 0)
        //{
        //    if (_isStartingPick && !_isInventoryFull)
        //    {
        //        _lookTimer += Time.deltaTime;

        //        OnTimeUpdate?.Invoke(LookTimerPracent, "");
        //        Debug.Log(_lookTimer);
        //        if (_lookTimer >= _liftingDelay && !_isIconFilled)
        //        {
        //            Debug.Log("4");
        //            _isIconFilled = true;

        //            _currentItemPickUp = _itemsPickUp[0];
        //            _itemsPickUp.Remove(_currentItemPickUp);

        //            if (!_isKeyPickUp)
        //            {
        //                _playerAnimation.PickUp();
        //                PickUpAninationEvent();
        //            }
        //        }
        //        else
        //            _isIconFilled = false;
        //    }
        //    else
        //    {
        //        ResetLookTimer();
        //        Debug.Log("6");
        //    }
        //}
    }

    public void UpdateIsKeyPickUp(bool isKeyPickUp)
    {
        _isKeyPickUp = !isKeyPickUp;
    }

    private void HandleLookTimer()
    {
        if (IsRayHittingSomething(_interactionItemLayer, out RaycastHit hitInfo) || _itemsPickUp.Count > 0)
        {
            if (_isStartingPick && !_isInventoryFull)
            {
                _lookTimer += Time.deltaTime;

                OnTimeUpdate?.Invoke(LookTimerPracent, "");

                if (_lookTimer >= _liftingDelay && !_isIconFilled)
                {
                    _isIconFilled = true;

                    if (_itemsPickUp.Count > 0)
                    {
                        _currentItemPickUp = _itemsPickUp[0];
                        _itemsPickUp.Remove(_currentItemPickUp);
                    }
                    else if (hitInfo.collider != null)
                    {
                        if (hitInfo.collider.TryGetComponent(out ItemPickUp itemPickUp))
                            _currentItemPickUp = itemPickUp;
                        else if (hitInfo.collider.TryGetComponent(out ObjectPickUp objectPickUp))
                            _currentObjectPickUp = objectPickUp;
                    }
                    

                    if (!_isKeyPickUp)
                    {
                        _playerAnimation.PickUp();
                        PickUpAninationEvent();
                    }
                }
                else
                    _isIconFilled = false;
            }
            else
                ResetLookTimer();
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
            ResetLookTimer();

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

    private void HandleInventoryFull()
    {
        if (_isInventoryFull)
        {
            ResetLookTimer();
        }
    }

    private void ResetLookTimer()
    {
        if(_lookTimer >= _liftingDelay || _itemsPickUp.Count == 0)
        {
            _lookTimer = 0;
            OnTimeUpdate?.Invoke(0f, "");
            _isIconFilled = false;
            _isInventoryFull = false;
        }
    }

    public void PickUpAninationEvent()
    {
        if(_coroutine == null)
            _coroutine = StartCoroutine(AddItemInventory());
    }

    private void InteractableConstruction()
    {
        if (_currentSleepingPlace)
        {
            _sleepPanel.OpenWindow();
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

    private IEnumerator AddItemInventory()
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
            foreach (var inventoryData in _currentObjectPickUp.ObjectItemsData.LootRandomItems.Items)
            {
                for (var i = 0; i < inventoryData.Amount; i++)
                {
                    if (!_playerInventoryHolder.AddToInventory(inventoryData.ItemData, _addAmount, inventoryData.ItemData.Durability))
                    {
                        _inventoryOperator.StartCreateItems(inventoryData.ItemData, inventoryData.ItemData.Durability, _addAmount);
                    }
                }
            }
            _currentObjectPickUp.PicUp();
            _currentObjectPickUp = null;
        }
        _audioSource.PlayOneShot(_playerAudioHandler.PickUpClip);

        yield return _waitForSeconds = new WaitForSeconds(0.5f);
        _coroutine = null;
    }

    private void PlantSeed(InventorySlot slot)
    {
        if (_currentGardenBed && _currentGardenBed.StartGrowingSeed(slot.ItemData))
        {
            _playerInventoryHolder.RemoveInventory(slot, _addAmount);
            OnSeedPlanted?.Invoke();
        }
    }

    private void OpenNote()
    {
        if(_note)
            _note.Init();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out ItemPickUp itemPickUp) && itemPickUp.enabled)
        {
            if (!_itemsPickUp.Contains(itemPickUp))
                _itemsPickUp.Add(itemPickUp);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out ItemPickUp itemPickUp))
        {
            if (_itemsPickUp.Contains(itemPickUp))
                _itemsPickUp.Remove(itemPickUp);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == _interactionItemLayerIndex)
        {
            
           
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