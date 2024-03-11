using System;
using UnityEngine;

public class ManualWorkbench : Raycast
{
    [SerializeField] private CraftingHandler _craftingHandler;
    [SerializeField] private Crafting�ategory _crafting�ategory;
    [SerializeField] private LayerMask _layerMask;

    private Crafting�ategory _currentCrafting�ategory;

    public event Action OnInteractionStarted;
    public event Action OnInteractionFinished;

    public Crafting�ategory Crafting�ategory => _crafting�ategory;

    private void Update()
    {
        if (IsRayHittingSomething(_layerMask, out RaycastHit hitInfo))
        {
            if (hitInfo.collider.TryGetComponent(out CraftObject craftObject) && _currentCrafting�ategory == null && craftObject.IsEnabled)
            {
                _currentCrafting�ategory = craftObject.Crafting�ategory;
                _craftingHandler.SetCategory(craftObject.Crafting�ategory);
                OnInteractionStarted?.Invoke();
            }
            else if(craftObject && !craftObject.IsEnabled)
                ResetCraft();
        }
        else
            ResetCraft();
    }

    private void ResetCraft()
    {
        if (_currentCrafting�ategory != null)
        {
            _currentCrafting�ategory = null;
            _craftingHandler.SetCategory(_crafting�ategory);
            OnInteractionFinished?.Invoke();
        }
    }
}
