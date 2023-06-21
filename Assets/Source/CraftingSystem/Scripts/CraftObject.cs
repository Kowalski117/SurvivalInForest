using UnityEngine;

public class CraftObject : MonoBehaviour
{
    [SerializeField] private Crafting�ategory _crafting�ategory;
    [SerializeField] private CraftingHandler _craftingHandler;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out ManualWorkbench manualWorkbench))
        {
            _craftingHandler.DisplayCraftWindow(_crafting�ategory);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out ManualWorkbench manualWorkbench))
        {
            _craftingHandler.DisplayCraftWindow(manualWorkbench.Crafting�ategory);
        }
    }
}
