using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class BuildTool : MonoBehaviour
{
    [SerializeField] private PlayerAnimation _playerAnimation;
    [SerializeField] private PlayerInventoryHolder _inventoryHolder;
    [SerializeField] private BuildPlayerInput _buildPlayerInput;
    [SerializeField] private float _rotateSnapAngle = 45f;
    [SerializeField] private float _rayDistance;
    [SerializeField] private LayerMask _buildModeLayerMask;
    [SerializeField] private LayerMask _deleteModeLayerMask;
    [SerializeField] private int _defoultLayerInt = 11;
    [SerializeField] private Transform _rayOrigin;
    [SerializeField] private Material _buildingMatPositive;
    [SerializeField] private Material _buildingMatNegative;
    [SerializeField] private Transform _containerBuildings;

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

        _buildPlayerInput.OnPutBuilding += PutBuilding;
        _buildPlayerInput.OnRotateBuilding += RotateBuilding;
        _buildPlayerInput.OnDeleteModeBuilding += DeleteMobeBuilding;
        _buildPlayerInput.OnDeleteBuilding += DeleteBuilding;
    }

    private void OnDisable()
    {
        CrafBuildSlot.OnCreateRecipeButtonClick -= ChoosePart;

        _buildPlayerInput.OnPutBuilding -= PutBuilding;
        _buildPlayerInput.OnRotateBuilding -= RotateBuilding;
        _buildPlayerInput.OnDeleteModeBuilding -= DeleteMobeBuilding;
        _buildPlayerInput.OnDeleteBuilding -= DeleteBuilding;
    }

    public void SetDeleteModeEnabled(bool deleteMode)
    {
        _deleteModeEnabled = deleteMode;
    }

    private void ChoosePart(BuildingRecipe buildingRecipe) 
    { 
        if(_deleteModeEnabled)
        {
            if(_targetBuilding != null && _targetBuilding.FlaggedForDelete)
            {
                _targetBuilding.RemoveDeleteFlag();

                _targetBuilding = null;
                _deleteModeEnabled = false;
            }
        }

        DeleteObjectPreview();

        _recipe = buildingRecipe;
        _spawnBuilding = Instantiate(buildingRecipe.BuildingData.Prefab, _containerBuildings);
        _spawnBuilding.Init(buildingRecipe.BuildingData);
        _spawnBuilding.transform.rotation = _lastRotation;
        OnCreateBuild?.Invoke();
        _playerAnimation.Build();
    }

    private void Update()
    {
        if (_deleteModeEnabled)
            DeleteModeLogic();
        else
            BuildModeLogic();
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

        if(detectedBuilding == null)
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

            // Перемещение точки столкновения немного выше, чтобы постройка не находилась полностью внутри террейна
            hitPoint += terrainNormal * 0.1f;

            // Подстройка постройки под поверхность террейна
            Vector3 gridPosition = WorldGrid.GridPositionFromWorldPoint3D(hitPoint, 1f);

            // Получение высоты террейна в заданной точке
            float terrainHeight = Terrain.activeTerrain.SampleHeight(hitPoint);

            // Корректировка позиции постройки по вертикали, чтобы она соответствовала высоте террейна
            gridPosition.y = terrainHeight;

            _spawnBuilding.transform.position = gridPosition;
        }
    }

    private void PutBuilding()
    {
        if (_spawnBuilding != null && !_spawnBuilding.IsOverlapping)
        {
            CraftingItem(_recipe);
            _spawnBuilding.PlaceBuilding();
            _spawnBuilding = null;
            OnCompletedBuild?.Invoke();
            _playerAnimation.TurnOffAnimations();
        }
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
        DeleteObjectPreview();
        _deleteModeEnabled = !_deleteModeEnabled;
        OnCompletedBuild?.Invoke();

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
}
