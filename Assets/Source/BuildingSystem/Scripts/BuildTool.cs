using BehaviorDesigner.Runtime.Tasks.Unity.UnityRenderer;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class BuildTool : MonoBehaviour
{
    [SerializeField] private PlayerHealth _playerHealth;
    [SerializeField] private SaveBuildingHandler _baseHandler;
    [SerializeField] private PlayerAnimatorHandler _playerAnimation;
    [SerializeField] private DelayWindow _loadingWindow;
    [SerializeField] private PlayerInventoryHolder _inventoryHolder;
    [SerializeField] private PlayerInputHandler _playerInputHandler;
    [SerializeField] private float _rotateSnapAngle = 45f;
    [SerializeField] private float _rayDistance;
    [SerializeField] private LayerMask _buildModeLayerMask;
    [SerializeField] private LayerMask _furnitureModeLayerMask;
    [SerializeField] private LayerMask _deleteModeLayerMask;
    [SerializeField] private int _foundationConnectionLayer;
    [SerializeField] private Transform _rayOrigin;
    [SerializeField] private Material _buildingMatPositive;
    [SerializeField] private Material _buildingMatNegative;
    [SerializeField] private Transform _containerBuildings;
    [SerializeField] private SphereCollider _selectionCollider;

    private bool _deleteModeEnabled;
    private bool _isMovedBuild = false;
    private Camera _camera;
    private BuildingRecipe _recipe;
    private Building _spawnBuilding;
    private Building _targetBuilding;
    private Quaternion _lastRotation;
    private FoundationConnection _currentFoundationConnection;

    private float _radiusSpawn = 2f;
    private float _spawnPointUp = 0.5f;
    private int _coutIndex = 2;

    public event UnityAction OnCreateBuild;
    public event UnityAction OnCompletedBuild;
    public event UnityAction OnDestroyBuild;

    public bool IsMoveBuild => _isMovedBuild;

    private void Awake()
    {
        _camera = Camera.main;
    }

    private void OnEnable()
    {
        CrafBuildSlot.OnCreateRecipeButtonClick += ChoosePart;

        _playerInputHandler.BuildPlayerInput.OnPutBuilding += PutBuilding;
        _playerInputHandler.BuildPlayerInput.OnRotateBuilding += RotateBuilding;
        _playerInputHandler.BuildPlayerInput.OnDeleteModeBuilding += DeleteMobeBuilding;
        _playerInputHandler.BuildPlayerInput.OnDeleteBuilding += DeleteBuilding;

        _playerHealth.OnRevived += DeleteBuilding;
    }

    private void OnDisable()
    {
        CrafBuildSlot.OnCreateRecipeButtonClick -= ChoosePart;

        _playerInputHandler.BuildPlayerInput.OnPutBuilding -= PutBuilding;
        _playerInputHandler.BuildPlayerInput.OnRotateBuilding -= RotateBuilding;
        _playerInputHandler.BuildPlayerInput.OnDeleteModeBuilding -= DeleteMobeBuilding;
        _playerInputHandler.BuildPlayerInput.OnDeleteBuilding -= DeleteBuilding;

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
    }

    public void DeleteObjectPreview()
    {
        if (_spawnBuilding != null)
        {
            _playerAnimation.TurnOffAnimations();
            Destroy(_spawnBuilding.gameObject);
            _spawnBuilding = null;
            _isMovedBuild = false;
            OnDestroyBuild?.Invoke();
        }
    }

    private bool IsRayHittingSomething(LayerMask layerMask, out RaycastHit hitInfo)
    {
        var ray = new Ray(_rayOrigin.position, _camera.transform.forward * _rayDistance);
        return Physics.Raycast(ray, out hitInfo, _rayDistance, layerMask);
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
        {
            _targetBuilding.FlagForDelete(_buildingMatNegative);
        }

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            foreach (var item in _targetBuilding.BuildingRecipe.CraftingIngridients)
            {
                SpawnItem(item.ItemRequired.ItemPrefab, _radiusSpawn, _spawnPointUp, item.AmountRequured / _coutIndex);
            }
            _targetBuilding = null;
            Destroy(hitInfo.collider.gameObject);
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
                {
                    _spawnBuilding.transform.position = hitInfo.point;
                }
            }
        }
        else
        {
            if (IsRayHittingSomething(_furnitureModeLayerMask, out RaycastHit hit))
            {
                _spawnBuilding.transform.position = hit.point;
            }
        }
    }

    private void PutBuilding()
    {
        if (_spawnBuilding != null && !_spawnBuilding.IsOverlapping)
        {
            //_playerInputHandler.ToggleAllInput(false);
            _loadingWindow.ShowLoadingWindow(_recipe.DelayCraft, _recipe.CraftingTime, _recipe.BuildingData.DisplayName, ActionType.CraftBuild);
            _loadingWindow.OnLoadingComplete += OnLoadingComplete;
        }
    }

    private void OnLoadingComplete()
    {
        OnCompletedBuild?.Invoke();
        CraftingItem(_recipe);
        _spawnBuilding.PlaceBuilding();
        _baseHandler.AddId(_spawnBuilding.UniqueID);
        _spawnBuilding = null;
        _playerAnimation.TurnOffAnimations();
        _selectionCollider.enabled = true;
        _isMovedBuild = false;
        //_playerInputHandler.ToggleBuildPlayerInput(true);

        _loadingWindow.OnLoadingComplete -= OnLoadingComplete;
    }

    private void RotateBuilding()
    {
        if (_spawnBuilding != null)
        {
            _spawnBuilding.transform.Rotate(0, _rotateSnapAngle, 0);
            _lastRotation = _spawnBuilding.transform.rotation;
        }
    }

    private void DeleteMobeBuilding()
    {
        OnCompletedBuild?.Invoke();
        DeleteObjectPreview();
        _deleteModeEnabled = !_deleteModeEnabled;
        _isMovedBuild = false;
    }

    private void CraftingItem(BuildingRecipe craftRecipe)
    {
        if (_inventoryHolder.CheckIfCanCraft(craftRecipe))
        {
            foreach (var ingredient in craftRecipe.CraftingIngridients)
            {
                _inventoryHolder.RemoveInventory(ingredient.ItemRequired, ingredient.AmountRequured);
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
        OnCreateBuild?.Invoke();
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
                Vector3 position = (_targetBuilding.transform.position + Random.insideUnitSphere * radius);
                SpawnLoots.Spawn(itemPickUp, position, _targetBuilding.transform, false, spawnPointUp, false);
            }
        }
    }
}
