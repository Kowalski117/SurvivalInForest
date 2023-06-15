using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Building : MonoBehaviour
{
    [SerializeField] private int _defoultLayerInt = 8;

    private BuildingData _assignedData;
    private BoxCollider _boxCollider;
    private GameObject _graphic;
    private Transform _colliders;
    private bool _isOverlappig;

    private Renderer _renderer;
    private Material _defoultMaterial;

    private bool _flaggedForDelete;

    private BuildingSaveData _saveData;

    public BuildingData AssignedData => _assignedData;
    public bool FlaggedForDelete => _flaggedForDelete;
    public bool IsOverlappig => _isOverlappig;

    private void Awake()
    {
        _boxCollider = GetComponent<BoxCollider>();
    }

    public void Init(BuildingData data, BuildingSaveData saveData = null)
    {
        _assignedData = data;
        _boxCollider.size = _assignedData.BuildingSize;
        _boxCollider.center = new Vector3(0, (_assignedData.BuildingSize.y + 0.2f) * 0.5f, 0);
        _boxCollider.isTrigger = true;

        Rigidbody rigidbody = gameObject.AddComponent<Rigidbody>();
        rigidbody.isKinematic = true;

        _graphic = Instantiate(data.Prefab, transform);
        _renderer = _graphic.GetComponentInChildren<Renderer>();   
        _defoultMaterial = _renderer.material;

        _colliders = _graphic.transform.Find("Colliders");

        if(_colliders != null )
            _colliders.gameObject.SetActive(false);

        if(saveData != null)
            _saveData = saveData;
    }

    public void PlaceBuilding()
    {
        _boxCollider.enabled = true;

        if(_colliders != null)
            _colliders.gameObject.SetActive(true);

        UpdateMaterial(_defoultMaterial);
        gameObject.layer = _defoultLayerInt;
        gameObject.name = _assignedData.DisplayName + " - " + transform.position;


        if (_saveData == null)
            _saveData = new BuildingSaveData(gameObject.name, _assignedData, transform.position, transform.rotation);

        if (!SaveGameHandler.Data.BuildingSaveData.Contains(_saveData))
            SaveGameHandler.Data.AddBuildingSaveData(_saveData);
    }

    public void UpdateMaterial(Material newMaterial)
    {
        if (_renderer == null)
            return;
        if(_renderer.material != newMaterial)
            _renderer.material = newMaterial;
    }

    public void FlagForDelete(Material deleteMaterial)
    {
        UpdateMaterial(deleteMaterial);
        _flaggedForDelete = true;
    }

    public void RemoveDeleteFlag()
    {
        UpdateMaterial(_defoultMaterial);
        _flaggedForDelete = false;
    }

    private void OnTriggerStay(Collider other)
    {
        _isOverlappig = true;
    }

    private void OnTriggerExit(Collider other)
    {
        _isOverlappig = false;
    }

    private void OnDestroy()
    {
        if (SaveGameHandler.Data.BuildingSaveData.Contains(_saveData))
            SaveGameHandler.Data.RemoveBuildingSaveData(_saveData);
    }
}

[System.Serializable]
public class BuildingSaveData
{
    [SerializeField] private string _buildingName;
    [SerializeField] private BuildingData _assignedData;
    [SerializeField] private Vector3 _position;
    [SerializeField] private Quaternion _rotation;

    public string BuildingName => _buildingName;
    public BuildingData AssignedData => _assignedData;
    public Vector3 Position => _position;
    public Quaternion Rotation => _rotation;

    public BuildingSaveData(string buildingName, BuildingData assignedData, Vector3 position, Quaternion rotation)
    {
        _buildingName = buildingName;
        _assignedData = assignedData;
        _position = position;
        _rotation = rotation;
    }
}
