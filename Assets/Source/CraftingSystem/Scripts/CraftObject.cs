using UnityEngine;

public class CraftObject : MonoBehaviour
{
    [SerializeField] private CraftingÑategory _craftingÑategory;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out ManualWorkbench manualWorkbench))
        {
            manualWorkbench.CraftingHandler.DisplayCraftWindow(_craftingÑategory);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out ManualWorkbench manualWorkbench))
        {
            manualWorkbench.CraftingHandler.DisplayCraftWindow(manualWorkbench.CraftingÑategory);
        }
    }
}
