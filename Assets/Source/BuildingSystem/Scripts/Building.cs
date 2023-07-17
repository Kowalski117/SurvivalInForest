using System.Xml;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider))]
public class Building : MonoBehaviour
{
    [SerializeField] private int _defoultLayerInt = 12;

    private string _buildingId = "Building_";

    private BuildingData _assignedData;
    private BoxCollider _boxCollider;
    private Rigidbody _rigidbody;
    private Transform _colliders;
    private bool _isOverlapping;

    private Renderer[] _renderers;
    private Material[] _defoultMaterial;

    private bool _flaggedForDelete;
    private BuildingSaveData _saveData;
    private UniqueID _uniqueID;

    public event UnityAction OnCompletedBuild;

    public BuildingData AssignedData => _assignedData;
    public bool FlaggedForDelete => _flaggedForDelete;
    public bool IsOverlapping => _isOverlapping;

    private void Awake()
    {
        _uniqueID = GetComponent<UniqueID>();
        _boxCollider = GetComponent<BoxCollider>();
        _rigidbody = GetComponent<Rigidbody>();
        _boxCollider.isTrigger = true;
        _rigidbody.isKinematic = true;
        _renderers = transform.GetComponentsInChildren<Renderer>();
        _defoultMaterial = new Material[_renderers.Length];

        for (int i = 0; i < _renderers.Length; i++)
        {
            _defoultMaterial[i] = _renderers[i].material;
        }
    }

    public void Init(BuildingData data, BuildingSaveData saveData = null)
    {
        _assignedData = data;
        _colliders = transform.Find("Colliders");

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

        UpdateMaterials(_defoultMaterial);

        gameObject.layer = _defoultLayerInt;
        _boxCollider.enabled = false;
        OnCompletedBuild?.Invoke();

        Save();
    }

    public void UpdateMaterial(Material newMaterial)
    {
        foreach (var renderer in _renderers)
        {
            if (renderer != null && renderer.material != newMaterial)
            {
                renderer.material = newMaterial;
            }
        }
    }

    public void UpdateMaterials(Material[] newMaterials)
    {
        for (int i = 0; i < _renderers.Length; i++)
        {
            _renderers[i].material = newMaterials[i];
        }
    }

    public void FlagForDelete(Material deleteMaterial)
    {
        UpdateMaterial(deleteMaterial);
        _flaggedForDelete = true;
    }

    public void RemoveDeleteFlag()
    {
        UpdateMaterials(_defoultMaterial);
        _flaggedForDelete = false;
    }

    private void OnTriggerStay(Collider other)
    {
        _isOverlapping = true;
    }

    private void OnTriggerExit(Collider other)
    {
        _isOverlapping = false;
    }

    private void OnDestroy()
    {
        if (ES3.KeyExists(_buildingId + _uniqueID.Id))
            ES3.DeleteKey(_buildingId + _uniqueID.Id);
        Destroy(this.gameObject);
    }

    private void Save()
    {
        BuildingSaveData itemSaveData = new BuildingSaveData(_assignedData, transform.position, transform.rotation);
        ES3.Save(_buildingId + _uniqueID.Id, itemSaveData);
    }

    private void Load()
    {
        if (ES3.KeyExists(_buildingId + _uniqueID.Id))
        {
            BuildingSaveData itemSaveData = ES3.Load(_buildingId + _uniqueID.Id, new BuildingSaveData(_assignedData, transform.position, transform.rotation));
            _assignedData = itemSaveData.AssignedData;
            transform.position = itemSaveData.Position;
            transform.rotation = itemSaveData.Rotation;
        }
        else
        {
            Destroy(gameObject);
        }
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

    public BuildingSaveData(BuildingData assignedData, Vector3 position, Quaternion rotation)
    {
        _assignedData = assignedData;
        _position = position;
        _rotation = rotation;
    }
}
