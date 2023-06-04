using UnityEngine;
using UnityEngine.InputSystem;

public class UIHandler : MonoBehaviour
{
    [SerializeField] private ExchangeHandler _shopKeeperDisplay;
    [SerializeField] private ExchangeKeeper _shopKeeper;
    
    private void Awake()
    {
        _shopKeeperDisplay.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        _shopKeeper.OnInteractionComplete += DisplayShopWindow;
    }

    private void OnDisable()
    {
        _shopKeeper.OnInteractionComplete -= DisplayShopWindow;
    }

    private void Update()
    {
        if(_shopKeeperDisplay.gameObject.activeInHierarchy && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            _shopKeeperDisplay.gameObject.SetActive(false);
        }
    }

    private void DisplayShopWindow(IInteractable interactor)
    {
        _shopKeeperDisplay.gameObject.SetActive(true);
        _shopKeeperDisplay.Init();
    }
}
