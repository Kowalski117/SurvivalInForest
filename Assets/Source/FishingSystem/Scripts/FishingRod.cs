using UnityEngine;

public class FishingRod : MonoBehaviour
{
    [SerializeField] private Float _float;
    [SerializeField] private FishingRodRenderer _renderer;
    [SerializeField] private PlayerInputHandler _inputHandler;
    [SerializeField] private PlayerInventoryHolder _inventoryHolder;
    [SerializeField] private HotbarDisplay _hotbarDisplay;
    [SerializeField] private float _distance = 10;

    private bool _isFishing = false;
    private bool _isAllowedFishing = true;
    private bool _isEnable = false;
    private FishingRodItemData _currentFishingRod;
    private float _fishingDelay = 1.5f;
    private float _timer = 0;

    private void OnEnable()
    {
        _inputHandler.InteractionPlayerInput.OnThrowFishingRod += ThrowFishingRod;

        _float.FishCaught += CatchFish;
        _float.FishMissed += MissFish;
    }

    private void OnDisable()
    {
        _inputHandler.InteractionPlayerInput.OnThrowFishingRod += ThrowFishingRod;

        _float.FishCaught -= CatchFish;
        _float.FishMissed -= MissFish;
    }

    private void Update()
    {
        Init(_hotbarDisplay.GetInventorySlotUI().AssignedInventorySlot.ItemData);

        if (Vector3.Distance(transform.position, _float.transform.position) > _distance && _isEnable)
        {
            _float.ReturnToRod(transform);
            _isFishing = false;
        }

        if (!_isAllowedFishing)
        {
            if (_timer >= _fishingDelay)
            {
                _timer = 0;
                _isAllowedFishing = true;
            }
            else
            {
                _timer += Time.deltaTime;
            }
        }


        if (_currentFishingRod == null && _float.transform.position != transform.position)
        {
            _float.ReturnToRod(transform);
            _isFishing = false;
            _isAllowedFishing = false;
        }
    }

    private void ThrowFishingRod()
    {
        if (_isAllowedFishing && _isEnable)
        {
            if (!_isFishing)
            {
                _isFishing = true;
                _renderer.DrawRope();
                _float.StartFishing(50, _currentFishingRod.RandomTime, GetRandomItem());
            }
            else
            {
                _isFishing = false;
                _isAllowedFishing = false;
                _float.ReturnToRod(transform);
            }
        }
    }

    private InventoryItemData GetRandomItem() 
    {
        int randomValue = Random.Range(0, 100);
        int currentProbability = 0;

        foreach (var extraction in _currentFishingRod.Extractions)
        {
            currentProbability += (int)(extraction.Chance * 100);

            if (randomValue < currentProbability)
            {
                return extraction.InventoryItemData;
            }
        }
        return null;
    }

    private void CatchFish(InventoryItemData inventoryItemData)
    {
        if (inventoryItemData != null)
            _inventoryHolder.AddToInventory(inventoryItemData, 1);
    }

    private void MissFish()
    {
        _isFishing = false;
    }

    private void Init(InventoryItemData itemData)
    {
        if(itemData != null && itemData is FishingRodItemData fishingRodItem)
        {
            _currentFishingRod = fishingRodItem;
            _isEnable= true;
        }
        else
        {
            _currentFishingRod = null;
            _isEnable = false;
        }
    }
}
