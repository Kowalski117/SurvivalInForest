using UnityEngine;
using UnityEngine.UI;

public class DeathScreen : MonoBehaviour
{
    [SerializeField] private PlayerHealth _playerHealth;
    [SerializeField] private PlayerInventoryHolder _inventoryHolder;
    [SerializeField] private YandexAds _andexAds;

    [SerializeField] private Transform _screen;
    [SerializeField] private Button _rebirthButton;
    [SerializeField] private Button _rebirthButtonForAdvertising;

    private bool _isDeathScreenOpen;
    private int _divisible = 3;
    private int _multiplier = 2;

    private void OnEnable()
    {
        _playerHealth.OnDied += ShowDeathWindow;
        _playerHealth.OnRevived += ShowDeathWindow;

        _rebirthButton.onClick.AddListener(RebirthButtonClick);
        _rebirthButtonForAdvertising.onClick.AddListener(RebirthButtonForAdvertisingClick);
    }

    private void OnDisable()
    {
        _playerHealth.OnDied -= ShowDeathWindow;
        _playerHealth.OnRevived -= ShowDeathWindow;

        _rebirthButton.onClick.RemoveListener(RebirthButtonClick);
        _rebirthButtonForAdvertising.onClick.RemoveListener(RebirthButtonForAdvertisingClick);
    }

    private void ShowDeathWindow()
    {
        _isDeathScreenOpen = !_isDeathScreenOpen;

        if (_isDeathScreenOpen)
            _screen.gameObject.SetActive(true);
        else
            _screen.gameObject.SetActive(false);
    }

    private void RebirthButtonClick()
    {
        int countSlot = _inventoryHolder.InventorySystem.GetAllFilledSlots().Count / _divisible * _multiplier;

        _inventoryHolder.DeletePartOfInventory(countSlot);

        _playerHealth.Reborn();
    }

    private void RebirthButtonForAdvertisingClick()
    {
        _andexAds.ShowRewardAd(() => _playerHealth.Reborn());
    }
}
