using UnityEngine;
using System;

[RequireComponent(typeof(PixelCrushers.QuestMachine.QuestControl))]
public class BuildTool : Raycast
{
    [SerializeField] private LayerMask _buildModeLayerMask;
    [SerializeField] private LayerMask _furnitureModeLayerMask;
    [SerializeField] private LayerMask _deleteModeLayerMask;
    [SerializeField] private float _rotateSnapAngle = 45f;

    [SerializeField] private PlayerHealth _playerHealth;
    [SerializeField] private SaveBuildingHandler _baseHandler;
    [SerializeField] private PlayerAnimatorHandler _playerAnimation;
    [SerializeField] private DelayHandler _loadingWindow;
    [SerializeField] private PlayerInventoryHolder _inventoryHolder;
    [SerializeField] private PlayerHandler _playerInputHandler;
    [SerializeField] private CraftingHandler _craftingHandler;
    [SerializeField] private Material _buildingMatPositive;
    [SerializeField] private Material _buildingMatNegative;
    [SerializeField] private Transform _containerBuildings;
    [SerializeField] private SphereCollider _selectionCollider;

    private bool _deleteModeEnabled;
    private bool _isMovedBuild = false;
    private BuildingRecipe _recipe;
    private Building _spawnBuilding;
    private Building _targetBuilding;
    private Quaternion _lastRotation;
    private FoundationConnection _currentFoundationConnection;
    private PixelCrushers.QuestMachine.QuestControl _questControl;

    private float _radiusSpawn = 2f;
    private float _spawnPointUp = 0.5f;

    public event Action OnBuildingCreated;
    public event Action OnBuildingCompleted;
    public event Action OnBuildingDestroyed;
    public event Action<bool> OnDeleteModeChanged;

    public bool IsMoveBuild => _isMovedBuild;

    protected override void Awake()
    {
        base.Awake();
        _questControl = GetComponent<PixelCrushers.QuestMachine.QuestControl>();
    }

    private void OnEnable()
    {
        _craftingHandler.OnBuildCreated += ChoosePart;

        _playerInputHandler.BuildPlayerInput.OnBuildingPutted += PutBuilding;
        _playerInputHandler.BuildPlayerInput.OnBuildingRotated += RotateBuilding;
        _playerInputHandler.BuildPlayerInput.OnDeleteModeBuildingToggled += DeleteMobeBuilding;
        _playerInputHandler.BuildPlayerInput.OnBuildingDeleted += DeleteBuildingMode;

        _playerHealth.OnRevived += DeleteBuilding;
    }

    private void OnDisable()
    {
        _craftingHandler.OnBuildCreated -= ChoosePart;

        _playerInputHandler.BuildPlayerInput.OnBuildingPutted -= PutBuilding;
        _playerInputHandler.BuildPlayerInput.OnBuildingRotated -= RotateBuilding;
        _playerInputHandler.BuildPlayerInput.OnDeleteModeBuildingToggled -= DeleteMobeBuilding;
        _playerInputHandler.BuildPlayerInput.OnBuildingDeleted -= DeleteBuildingMode;

        _playerHealth.OnRevived -= DeleteBuilding;
    }

    private void Update()
    {
        if (_deleteModeEnabled)
            DeleteModeLogic();
        else
            BuildModeLogic();
    }

    public void DeleteBuilding()
    {
        DeleteObjectPreview();
    }

    public void SetDeleteModeEnabled(bool deleteMode)
    {
        _deleteModeEnabled = deleteMode;
        OnDeleteModeChanged?.Invoke(_deleteModeEnabled);
    }

    public void DeleteObjectPreview()
    {
        if (_spawnBuilding != null)
        {
            _playerAnimation.TurnOffAnimations();
            Destroy(_spawnBuilding.gameObject);
            _spawnBuilding = null;
            _isMovedBuild = false;
            OnBuildingDestroyed?.Invoke();
        }
    }

    private void DeleteModeLogic()
    {
        if (!IsRayHittingSomething(_deleteModeLayerMask, out RaycastHit hitInfo))
            return;

        var detectedBuilding = hitInfo.collider.gameObject.GetComponentInParent<Building>();

        if (detectedBuilding == null || !detectedBuilding.IsCanDelete)
            return;

        if (_targetBuilding == null)
            _targetBuilding = detectedBuilding;

        if (detectedBuilding != _targetBuilding && _targetBuilding.FlaggedForDelete)
        {
            _targetBuilding.RemoveDeleteFlag();
            _targetBuilding = detectedBuilding; 
        }

        if (detectedBuilding == _targetBuilding && !_targetBuilding.FlaggedForDelete)
            _targetBuilding.FlagForDelete(_buildingMatNegative);
    }

    private void DeleteBuildingMode()
    {
        if (_deleteModeEnabled && _targetBuilding)
        {
            foreach (var item in _targetBuilding.BuildingRecipe.CraftingIngridients)
            {
                SpawnItem(item.ItemRequired.ItemPrefab, _radiusSpawn, _spawnPointUp, item.AmountRequured);
            }

            Destroy(_targetBuilding.gameObject);
            _targetBuilding = null;
        }
    }

    private void BuildModeLogic()
    {
        if (_targetBuilding != null && _targetBuilding.FlaggedForDelete)
        {
            _targetBuilding.RemoveDeleteFlag();
            _targetBuilding = null;
        }

        if (_spawnBuilding == null)
            return;

        PositionBuildingPreview();
    }

    private void PositionBuildingPreview()
    {
        _spawnBuilding.UpdateMaterial(_spawnBuilding.IsOverlapping ? _buildingMatNegative : _buildingMatPositive);

        if (_recipe.BuildingData.Type == ItemType.Build)
        {
            if (IsRayHittingSomething(_buildModeLayerMask, out RaycastHit hitInfo))
            {
                if (hitInfo.collider.TryGetComponent(out FoundationConnection foundationConnection))
                {
                    _currentFoundationConnection = foundationConnection;

                    if (_currentFoundationConnection.BuildType == _recipe.BuildingData.BuildType)
                        _spawnBuilding.transform.position = foundationConnection.PointPlaceFloor.position;
                    else
                        _spawnBuilding.transform.position = hitInfo.collider.transform.position;
                }
                else
                    _spawnBuilding.transform.position = hitInfo.point;
            }
        }
        else
        {
            if (IsRayHittingSomething(_furnitureModeLayerMask, out RaycastHit hit))
                _spawnBuilding.transform.position = hit.point;
        }
    }

    private void PutBuilding()
    {
        if (_spawnBuilding != null && !_spawnBuilding.IsOverlapping)
            _loadingWindow.ShowLoadingWindow(_recipe.DelayCraft, _recipe.CraftingTime, _recipe.BuildingData.DisplayName, ActionType.CraftBuild, () => FinishComplete());
    }

    private void FinishComplete()
    {
        OnBuildingCompleted?.Invoke();
        Crafting(_recipe);
        _spawnBuilding.Place();
        _baseHandler.AddId(_spawnBuilding.UniqueID);
        _spawnBuilding = null;
        _playerAnimation.TurnOffAnimations();
        _selectionCollider.enabled = true;
        _isMovedBuild = false;
        _questControl.SendToMessageSystem(MessageConstants.Build + _recipe.BuildingData.Name);
    }

    private void RotateBuilding()
    {
        if (_spawnBuilding != null)
        {
            _spawnBuilding.transform.Rotate(0,_rotateSnapAngle, 0);
            _lastRotation = _spawnBuilding.transform.rotation;
        }
    }

    private void DeleteMobeBuilding()
    {
        DeleteObjectPreview(); 
        SetDeleteModeEnabled(!_deleteModeEnabled);
        _isMovedBuild = false;
    }

    private void Crafting(BuildingRecipe craftRecipe)
    {
        if (_inventoryHolder.CheckIfCanCraft(craftRecipe))
        {
            foreach (var ingredient in craftRecipe.CraftingIngridients)
            {
                _inventoryHolder.RemoveItem(ingredient.ItemRequired, ingredient.AmountRequured);
            }
        }
    }

    private void ChoosePart(BuildingRecipe buildingRecipe)
    {
        _recipe = buildingRecipe;
        if (_deleteModeEnabled)
        {
            if (_targetBuilding != null && _targetBuilding.FlaggedForDelete)
            {
                _targetBuilding.RemoveDeleteFlag();
                _targetBuilding = null;
                _deleteModeEnabled = false;
            }
        }

        DeleteObjectPreview();
        SetDeleteModeEnabled(false);

        _spawnBuilding = Instantiate(_recipe.BuildingData.Prefab, _containerBuildings);
        _spawnBuilding.Init(_recipe);
        _spawnBuilding.transform.rotation = _lastRotation;
        OnBuildingCreated?.Invoke();
        _playerAnimation.Build();
        _selectionCollider.enabled = false;
        _isMovedBuild = true;
    }

    private void SpawnItem(ItemPickUp itemPickUp, float radius, float spawnPointUp, int count)
    {
        if (_deleteModeEnabled && _targetBuilding != null)
        {
            for (int i = 0; i < count; i++)
            {
                Vector3 position = (_targetBuilding.transform.position + UnityEngine.Random.insideUnitSphere * radius);
                SpawnLoots.Spawn(itemPickUp, position, _targetBuilding.transform, false, spawnPointUp, false);
            }
        }
    }
}
