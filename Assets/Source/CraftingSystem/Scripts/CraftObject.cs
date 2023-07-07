using UnityEngine;

public class CraftObject : MonoBehaviour
{
    [SerializeField] private Crafting�ategory _crafting�ategory;

    private Building _building;
    private SphereCollider _collider;

    private void Awake()
    {
        _building = GetComponentInParent<Building>();
        _collider = GetComponent<SphereCollider>();
        _collider.enabled = false;
    }

    private void OnEnable()
    {
        _building.OnCompletedBuild += EnableCollider;
    }

    private void OnDisable()
    {
        _building.OnCompletedBuild -= EnableCollider;
    }

    private void EnableCollider()
    {
        _collider.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out ManualWorkbench manualWorkbench))
        {
            manualWorkbench.CraftingHandler.DisplayCraftWindow(_crafting�ategory);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out ManualWorkbench manualWorkbench))
        {
            manualWorkbench.CraftingHandler.DisplayCraftWindow(manualWorkbench.Crafting�ategory);
        }
    }
}
