using UnityEngine;

public class ManualWorkbench : MonoBehaviour
{
    [SerializeField] private CraftingHandler _craftingHandler;
    [SerializeField] private Crafting�ategory _crafting�ategory;

    public CraftingHandler CraftingHandler => _craftingHandler;
    public Crafting�ategory Crafting�ategory => _crafting�ategory;
}
