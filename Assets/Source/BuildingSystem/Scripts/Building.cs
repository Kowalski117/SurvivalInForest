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

    public string UniqueID => _buildingId + _uniqueID.Id;
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

    private void OnEnable()
    {
        SaveGame.OnLoadData += Load;
    }

    private void OnDisable()
    {
        SaveGame.OnLoadData -= Load;
    }

    public void Init(BuildingData data, BuildingSaveData saveData = null)
    {
        _assignedData = data;
        _colliders = transform.Find("Colliders");

        if(_colliders != null )
            _colliders.gameObject.SetActive(false);

        if(saveData != null)
            _saveData = saveData;

        _uniqueID.Generate();
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
        //if (ES3.KeyExists(UniqueID))
        //    ES3.DeleteKey(UniqueID);
    }

    public void Save()
    {
        BuildingSaveData itemSaveData = new BuildingSaveData(_assignedData.Prefab, transform.position, transform.rotation);
        Debug.Log(itemSaveData);
        Debug.Log(itemSaveData.BuildingPrefab);
        Debug.Log(itemSaveData.Position);
        Debug.Log(itemSaveData.Rotation);
        ES3.Save(UniqueID, itemSaveData);
        Debug.Log(ES3.Load<BuildingSaveData>(UniqueID).BuildingPrefab);
        Debug.Log(UniqueID);
    }

    private void Load()
    {
        if (!ES3.KeyExists(UniqueID))
        {
            Destroy(gameObject);
        }
    }
}

[System.Serializable]
public class BuildingSaveData
{
    [SerializeField] private string _buildingName;
    [SerializeField] private Building _buildingPrefab;
    [SerializeField] private Vector3 _position;
    [SerializeField] private Quaternion _rotation;

    public string BuildingName => _buildingName;
    public Building BuildingPrefab => _buildingPrefab;
    public Vector3 Position => _position;
    public Quaternion Rotation => _rotation;

    public BuildingSaveData(Building buildingPrefab, Vector3 position, Quaternion rotation)
    {
        _buildingPrefab = buildingPrefab;
        _position = position;
        _rotation = rotation;
    }
}
