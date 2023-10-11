using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider))]
public class Building : MonoBehaviour
{
    [SerializeField] private int _defoultLayerInt = 12;
    [SerializeField] private bool _isCanDelete = true;
    [SerializeField] private GameObject _colliderGround;
    [SerializeField] private Renderer[] _renderers;

    private BuildingRecipe _buildingRecipe;
    private BoxCollider _boxCollider;
    private Rigidbody _rigidbody;
    private Transform _colliders;
    private bool _isOverlapping;

    private Material[] _defoultMaterial;

    private bool _flaggedForDelete;
    private BuildingSaveData _saveData;
    private UniqueID _uniqueID;

    public event UnityAction OnCompletedBuild;

    public string UniqueID =>  _uniqueID.Id;
    public BuildingRecipe BuildingRecipe => _buildingRecipe;
    public bool FlaggedForDelete => _flaggedForDelete;
    public bool IsOverlapping => _isOverlapping;
    public bool IsCanDelete => _isCanDelete;

    private void Awake()
    {
        _uniqueID = GetComponent<UniqueID>();
        _boxCollider = GetComponent<BoxCollider>();
        _rigidbody = GetComponent<Rigidbody>();
        _boxCollider.isTrigger = true;
        _rigidbody.isKinematic = true;
        //_renderers = transform.GetComponentsInChildren<Renderer>();
        _defoultMaterial = new Material[_renderers.Length];

        if(_colliderGround)
            _colliderGround.SetActive(false);

        for (int i = 0; i < _renderers.Length; i++)
        {
            _defoultMaterial[i] = _renderers[i].material;
        }
    }

    public void Init(BuildingRecipe buildingRecipe, string id = null)
    {
        _buildingRecipe = buildingRecipe;

        if (id != null)
            _uniqueID.SetId(id);
        else
            _uniqueID.Generate();
    }

    public void PlaceBuilding()
    {
        _boxCollider.enabled = true;

        UpdateMaterials(_defoultMaterial);

        gameObject.layer = _defoultLayerInt;
        OnCompletedBuild?.Invoke();

        if (_colliderGround)
            _colliderGround.SetActive(true);

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

    public void Save()
    {
        BuildingSaveData itemSaveData = new BuildingSaveData(_buildingRecipe.BuildingData.Id, transform.position, transform.rotation);
        ES3.Save(_uniqueID.Id, itemSaveData);
    }

    private void OnTriggerStay(Collider other)
    {
        _isOverlapping = true;
    }

    private void OnTriggerExit(Collider other)
    {
        _isOverlapping = false;
    }
}

[System.Serializable]
public class BuildingSaveData
{
    [SerializeField] private string _buildingName;
    [SerializeField] private int _id;
    [SerializeField] private Vector3 _position;
    [SerializeField] private Quaternion _rotation;

    public string BuildingName => _buildingName;
    public int Id => _id;
    public Vector3 Position => _position;
    public Quaternion Rotation => _rotation;

    public BuildingSaveData(int id, Vector3 position, Quaternion rotation)
    {
        _id = id;
        _position = position;
        _rotation = rotation;
    }
}
