using UnityEngine;
using UnityEngine.UI;

public class DeathScreen : MonoBehaviour
{
    [SerializeField] private PlayerHealth _playerHealth;
    [SerializeField] private PlayerInventoryHolder _inventoryHolder;
    [SerializeField] private YandexAds _andexAds;

    [SerializeField] private AnimationUI _animationUI;
    [SerializeField] private Transform _screen;

    [SerializeField] private Button _rebirthButton;
    [SerializeField] private Button _rebirthButtonForAdvertising;

    private bool _isDeathScreenOpen;
    private int _divisible = 3;
    private int _multiplier = 2;

    private void Awake()
    {
        _animationUI.Close();
    }

    private void OnEnable()
    {
        _playerHealth.OnDied += Show;
        _playerHealth.OnRevived += Show;

        _rebirthButton.onClick.AddListener(RebirthButtonClick);
        _rebirthButtonForAdvertising.onClick.AddListener(RebirthButtonForAdvertisingClick);
    }

    private void OnDisable()
    {
        _playerHealth.OnDied -= Show;
        _playerHealth.OnRevived -= Show;

        _rebirthButton.onClick.RemoveListener(RebirthButtonClick);
        _rebirthButtonForAdvertising.onClick.RemoveListener(RebirthButtonForAdvertisingClick);
    }

    private void Show()
    {
        _isDeathScreenOpen = !_isDeathScreenOpen;

        if (_isDeathScreenOpen)
            _animationUI.Open();
        else
            _animationUI.Close();
    }

    private void RebirthButtonClick()
    {
        int countSlot = _inventoryHolder.InventorySystem.GetAllFilledSlots().Count / _divisible * _multiplier;

        _inventoryHolder.DeletePart(countSlot);

        _playerHealth.Reborn();
    }

    private void RebirthButtonForAdvertisingClick()
    {
        _andexAds.ShowRewardAd(() => _playerHealth.Reborn());
    }
}
