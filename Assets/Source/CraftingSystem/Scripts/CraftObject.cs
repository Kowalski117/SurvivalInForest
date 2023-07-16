using UnityEngine;

public class CraftObject : MonoBehaviour
{
    [SerializeField] private CraftingÑategory _craftingÑategory;

    private Building _building;
    private SphereCollider _collider;
    private ManualWorkbench _workbench;

    private void Awake()
    {
        _building = GetComponentInParent<Building>();
        _collider = GetComponent<SphereCollider>();
        if(_building != null )
            _collider.enabled = false;
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
        _collider.enabled = false;

        if(_workbench != null)
        {
            _workbench.CraftingHandler.DisplayCraftWindow(_workbench.CraftingÑategory);
            _workbench = null;
        }

        enabled = false;
    }

    private void EnableCollider()
    {
        _collider.enabled = true;
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
