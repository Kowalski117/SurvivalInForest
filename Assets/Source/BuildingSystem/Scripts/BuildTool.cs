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
    [SerializeField] private LayerMask _deleteModeLayerMask;
    [SerializeField] private Transform _rayOrigin;
    [SerializeField] private Material _buildingMatPositive;
    [SerializeField] private Material _buildingMatNegative;
    [SerializeField] private Transform _containerBuildings;
    [SerializeField] private SphereCollider _selectionCollider;

    private bool _deleteModeEnabled;
    private Camera _camera;
    private BuildingRecipe _recipe;
    private Building _spawnBuilding;
    private Building _targetBuilding;
    private Quaternion _lastRotation;

    public event UnityAction OnCreateBuild;
    public event UnityAction OnCompletedBuild;

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

        _playerHealth.OnDied += DeleteBuilding;
    }

    private void OnDisable()
    {
        CrafBuildSlot.OnCreateRecipeButtonClick -= ChoosePart;

        _playerInputHandler.BuildPlayerInput.OnPutBuilding -= PutBuilding;
        _playerInputHandler.BuildPlayerInput.OnRotateBuilding -= RotateBuilding;
        _playerInputHandler.BuildPlayerInput.OnDeleteModeBuilding -= DeleteMobeBuilding;
        _playerInputHandler.BuildPlayerInput.OnDeleteBuilding -= DeleteBuilding;

        _playerHealth.OnDied -= DeleteBuilding;
    }

    private void Update()
    {
        if (_deleteModeEnabled)
            DeleteModeLogic();
        else
            BuildModeLogic();
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

        if(detectedBuilding == null || !detectedBuilding.IsCanDelete)
            return;

        if (_targetBuilding == null)
            _targetBuilding = detectedBuilding;

        if(detectedBuilding != _targetBuilding && _targetBuilding.FlaggedForDelete)
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
            Destroy(hitInfo.collider.gameObject);
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

        if (IsRayHittingSomething(_buildModeLayerMask, out RaycastHit hitInfo))
        {
            Vector3 hitPoint = hitInfo.point;
            Vector3 terrainNormal = hitInfo.normal;

            hitPoint += terrainNormal * 0.1f;
            Vector3 gridPosition = WorldGrid.GridPositionFromWorldPoint3D(hitPoint, 1f);
            float terrainHeight = Terrain.activeTerrain.SampleHeight(hitPoint);

            gridPosition.y = terrainHeight;
            _spawnBuilding.transform.position = hitPoint;
        }
    }

    private void PutBuilding()
    {
        if (_spawnBuilding != null && !_spawnBuilding.IsOverlapping)
        {
            _playerInputHandler.TogglePersonController(false);
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
        _playerInputHandler.TogglePersonController(true);
        _playerInputHandler.ToggleBuildPlayerInput(true);
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
    }

    private void DeleteBuilding()
    {
        DeleteObjectPreview();
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
        _spawnBuilding.Init(_recipe.BuildingData);
        _spawnBuilding.transform.rotation = _lastRotation;
        OnCreateBuild?.Invoke();
        _playerAnimation.Build();
        _selectionCollider.enabled = false;
    }
}
