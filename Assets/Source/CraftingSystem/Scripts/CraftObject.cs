using UnityEngine;

public class CraftObject : MonoBehaviour
{
    [SerializeField] private CraftingÑategory _craftingÑategory;
    [SerializeField] private bool _isEnabledInitially = false;

    private Building _building;
    private SphereCollider _sphereCollider;
    private BoxCollider _boxCollider;
    private ManualWorkbench _workbench;

    public bool IsEnabledInitially => _isEnabledInitially;

    private void Awake()
    {
        _building = GetComponentInParent<Building>();
        _sphereCollider = GetComponent<SphereCollider>();
        _boxCollider = GetComponent<BoxCollider>();
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
    }

    private void OnDisable()
    {
        if (_building != null)
        {
            _building.OnCompletedBuild -= EnableCollider;
        }
    }

    public void TurnOff()
    {
        _sphereCollider.enabled = false;

        if(_workbench != null)
        {
            _workbench.CraftingHandler.DisplayCraftWindow(_workbench.CraftingÑategory);
            _workbench = null;
        }

        enabled = false;
    }

    private void EnableCollider()
    {
        _sphereCollider.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out ManualWorkbench manualWorkbench))
        {
            _workbench = manualWorkbench;
            manualWorkbench.CraftingHandler.DisplayCraftWindow(_craftingÑategory);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out ManualWorkbench manualWorkbench))
        {
            manualWorkbench.CraftingHandler.DisplayCraftWindow(manualWorkbench.CraftingÑategory);
            _workbench = null;
        }
    }
}
