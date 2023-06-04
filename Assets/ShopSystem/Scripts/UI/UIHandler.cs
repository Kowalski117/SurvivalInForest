using UnityEngine;
using UnityEngine.InputSystem;

public class UIHandler : MonoBehaviour
{
    [SerializeField] private ExchangeHandler _shopKeeperDisplay;
    [SerializeField] private ExchangeKeeper _shopKeeper;
    [SerializeField] private CraftingHandler _craftingHandler;
    
    private void Awake()
    {
        _shopKeeperDisplay.gameObject.SetActive(false);
        _craftingHandler.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        CraftBench.OnCraftingDisplayRequested += DisplayCraftWindow;
        _shopKeeper.OnInteractionComplete += DisplayShopWindow;
    }

    private void OnDisable()
    {
        CraftBench.OnCraftingDisplayRequested -= DisplayCraftWindow;
        _shopKeeper.OnInteractionComplete -= DisplayShopWindow;
    }

    private void Update()
    {
        if(_shopKeeperDisplay.gameObject.activeInHierarchy && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            _shopKeeperDisplay.gameObject.SetActive(false);
        }

        if (_craftingHandler.gameObject.activeInHierarchy && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            _craftingHandler.gameObject.SetActive(false);
        }
    }

    private void DisplayShopWindow(IInteractable interactor)
    {
        _shopKeeperDisplay.gameObject.SetActive(true);
        _shopKeeperDisplay.CreateSlots();
    }

    private void DisplayCraftWindow(CraftBench craftBench)
    {
        _craftingHandler.gameObject.SetActive(true);
        _craftingHandler.DisplayCraftingWindow(craftBench);
    }
}
