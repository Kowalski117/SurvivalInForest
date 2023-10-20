using UnityEngine;

public class CraftObject : MonoBehaviour
{
    [SerializeField] private CraftingÑategory _craftingÑategory;
    [SerializeField] private bool _isEnabledInitially = false;

    private Building _building;
    private SphereCollider _sphereCollider;
    private BoxCollider _boxCollider;
    private ManualWorkbench _workbench;
    private DistanceHandler _distanceHandler;

    public bool IsEnabledInitially => _isEnabledInitially;

    private void Awake()
    {
        _building = GetComponentInParent<Building>();
        _sphereCollider = GetComponent<SphereCollider>();
        _boxCollider = GetComponent<BoxCollider>();
        _distanceHandler = GetComponentInChildren<DistanceHandler>();

        if (_building != null)
        {
            _boxCollider.enabled = false;
            _sphereCollider.enabled = false;
        }

        if (_isEnabledInitially)
        {
            _boxCollider.enabled = true;
            _sphereCollider.enabled = true;
        }
    }

    private void OnEnable()
    {
        if(_building != null )
        {
            _building.OnCompletedBuild += EnableCollider;
        }

        _distanceHandler.OnDistanceExceeded += SetDefoultCrafts;
    }

    private void OnDisable()
    {
        if (_building != null)
        {
            _building.OnCompletedBuild -= EnableCollider;
        }

        _distanceHandler.OnDistanceExceeded -= SetDefoultCrafts;
    }

    public void TurnOff()
    {
        _sphereCollider.enabled = false;

        if(_workbench != null)
        {
            _workbench.CraftingHandler.DisplayCraftWindow(_workbench.CraftingÑategory);
            _workbench = null;
            _distanceHandler.SetActive(false);
        }

        enabled = false;
    }

    private void SetDefoultCrafts()
    {
        _workbench.CraftingHandler.DisplayCraftWindow(_workbench.CraftingÑategory);
        _workbench = null;
        _distanceHandler.SetActive(false);
    }

    private void EnableCollider()
    {
        _sphereCollider.enabled = true;
    }

    private void OnDestroy()
    {
        TurnOff();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out ManualWorkbench manualWorkbench))
        {
            _workbench = manualWorkbench;
            manualWorkbench.CraftingHandler.DisplayCraftWindow(_craftingÑategory);
            _distanceHandler.SetActive(true);
        }
    }

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.TryGetComponent(out ManualWorkbench manualWorkbench))
    //    {
    //        manualWorkbench.CraftingHandler.DisplayCraftWindow(manualWorkbench.CraftingÑategory);
    //        _workbench = null;
    //    }
    //}
}
