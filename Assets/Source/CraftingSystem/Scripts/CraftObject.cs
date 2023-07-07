using UnityEngine;

public class CraftObject : MonoBehaviour
{
    [SerializeField] private Crafting�ategory _crafting�ategory;

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
