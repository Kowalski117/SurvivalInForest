using Mono.Cecil;
using UnityEngine;

public class MiningHandler : MonoBehaviour
{
    [SerializeField] private HotbarDisplay _hotbarDisplay;
    [SerializeField] private InteractionPlayerInput _interactionPlayerInput;
    [SerializeField] private AudioSource _audioSource;

    private ItemType _typeSlot = ItemType.Tool;
    private InventoryItemData _currentItemData;
    private Resource _currentResoure;
    private float _nextFire;

    private void OnEnable()
    {
        _interactionPlayerInput.OnHit += InteractResource;
    }

    private void OnDisable()
    {
        _interactionPlayerInput.OnHit -= InteractResource;
    }

    private void InteractResource()
    {
        //_audioSource.PlayOneShot(_currentWeapon.MuzzleSound);
        ////_currentWeapon.MuzzleFlash.Play();

        if (_currentItemData != null && _currentResoure != null)
        {
            var itemData = _hotbarDisplay.GetInventorySlotUI().AssignedInventorySlot.ItemData;
            if (itemData != null && itemData.Type == _typeSlot)
            {
                if (itemData is ToolItemData toolItemData && _currentResoure.ExtractionType == toolItemData.ToolType)
                {
                    if (Time.time > _nextFire)
                    {
                        _nextFire = Time.time + 1 / toolItemData.Speed;
                        _currentResoure.TakeDamage(toolItemData.DamageResources, 0);

                        if (_currentResoure.Health <= 0)
                        {
                            _currentItemData = null;
                            _currentResoure = null;
                        }
                    }
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out Resource resource))
        {
            var itemData = _hotbarDisplay.GetInventorySlotUI().AssignedInventorySlot.ItemData;
            if (itemData != null && itemData.Type == _typeSlot)
            {
                _currentItemData = itemData;
                _currentResoure = resource;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Resource resource))
        {
            _currentItemData = null;
            _currentResoure = null;
        }
    }
}
