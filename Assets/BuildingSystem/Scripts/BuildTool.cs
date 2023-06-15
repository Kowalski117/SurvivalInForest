using UnityEngine;
using UnityEngine.InputSystem;

public class BuildTool : MonoBehaviour
{
    [SerializeField] private float _rotateSnapAngle = 45f;
    [SerializeField] private float _rayDistance;
    [SerializeField] private LayerMask _buildModeLayerMask;
    [SerializeField] private LayerMask _deleteModeLayerMask;
    [SerializeField] private int _defoultLayerInt = 9;
    [SerializeField] private Transform _rayOrigin;
    [SerializeField] private Material _buildingMatPositive;
    [SerializeField] private Material _buildingMatNegative;

    private bool _deleteModeEnabled;
    private Camera _camera;
    private Building _spawnBuilding;
    private Building _targetBuilding;
    private Quaternion _lastRotation;

    private void Awake()
    {
        _camera = Camera.main;
    }

    private void OnEnable()
    {
        BuildingPanelUI.OnPartChosen += ChoosePart;
    }

    private void OnDisable()
    {
        BuildingPanelUI.OnPartChosen -= ChoosePart;
    }

    private void ChoosePart(BuildingData data) 
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

        GameObject gameObject = new GameObject
        {
            layer = _defoultLayerInt, 
            name = "Build Preview"
        };

        _spawnBuilding = gameObject.AddComponent<Building>();
        _spawnBuilding.Init(data);
        _spawnBuilding.transform.rotation = _lastRotation;
    }

    private void Update()
    {
        if(_spawnBuilding && Keyboard.current.escapeKey.wasPressedThisFrame)
            DeleteObjectPreview();

        if (Keyboard.current.qKey.wasPressedThisFrame)
            _deleteModeEnabled = !_deleteModeEnabled;

        if (_deleteModeEnabled)
            DeleteModeLogic();
        else
            BuildModeLogic();
    }

    private void DeleteObjectPreview()
    {
        if (_spawnBuilding != null)
        {
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
        _spawnBuilding.UpdateMaterial(_spawnBuilding.IsOverlappig ? _buildingMatNegative : _buildingMatPositive);

        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            _spawnBuilding.transform.Rotate(0, _rotateSnapAngle, 0);
            _lastRotation = _spawnBuilding.transform.rotation;
        }

        if (IsRayHittingSomething(_buildModeLayerMask, out RaycastHit hitInfo))
        {
            Vector3 gridPosition = WorldGrid.GridPositionFromWorldPoint3D(hitInfo.point, 1f);
            _spawnBuilding.transform.position = gridPosition;

            if (Mouse.current.leftButton.wasPressedThisFrame && !_spawnBuilding.IsOverlappig)
            {
                _spawnBuilding.PlaceBuilding();
                BuildingData dataCopy = _spawnBuilding.AssignedData;
                _spawnBuilding = null;
                ChoosePart(dataCopy);
            }
        }
    }
}
