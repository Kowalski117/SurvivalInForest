using UnityEngine;

public class ManualWorkbench : MonoBehaviour
{
    [SerializeField] private CraftingHandler _craftingHandler;
    [SerializeField] private CraftingÑategory _craftingÑategory;

    public CraftingHandler CraftingHandler => _craftingHandler;
    public CraftingÑategory CraftingÑategory => _craftingÑategory;
}
