using System;
using UnityEngine;

public class ManualWorkbench : Raycast
{
    [SerializeField] private CraftingHandler _craftingHandler;
    [SerializeField] private CraftingÑategory _craftingÑategory;
    [SerializeField] private LayerMask _layerMask;

    private CraftObject _craftObject;
    private CraftingÑategory _currentCraftingÑategory;

    public event Action OnInteractionStarted;
    public event Action OnInteractionFinished;

    public CraftingHandler CraftingHandler => _craftingHandler;
    public CraftingÑategory CraftingÑategory => _craftingÑategory;

    private void Update()
    {
        if (IsRayHittingSomething(_layerMask, out RaycastHit hitInfo))
        {
            if (hitInfo.collider.TryGetComponent(out CraftObject craftObject) && _currentCraftingÑategory == null && craftObject.IsEnabled)
            {
                _currentCraftingÑategory = craftObject.CraftingÑategory;
                _craftingHandler.DisplayCraftWindow(craftObject.CraftingÑategory);
                OnInteractionStarted?.Invoke();
            }
            else if(craftObject && !craftObject.IsEnabled)
            {
                ResetCraft();
            }


        }
        else
        {
            ResetCraft();
        }
    }

    private void ResetCraft()
    {
        if (_currentCraftingÑategory != null)
        {
            _currentCraftingÑategory = null;
            _craftingHandler.DisplayCraftWindow(_craftingÑategory);
            OnInteractionFinished?.Invoke();
        }
    }
}
