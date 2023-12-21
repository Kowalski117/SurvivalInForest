using PixelCrushers.QuestMachine;
using UnityEngine;

public class FishingRod : MonoBehaviour
{
    [SerializeField] private Float _float;
    [SerializeField] private FishingRodRenderer _renderer;
    [SerializeField] private PlayerInteraction _playerInteraction;
    [SerializeField] private PlayerInputHandler _inputHandler;
    [SerializeField] private PlayerInventoryHolder _inventoryHolder;
    [SerializeField] private HotbarDisplay _hotbarDisplay;
    [SerializeField] private float _distance = 10;
    [SerializeField] private float _velocityForse = 50;

    private QuestControl _questControl;
    private bool _isFishing = false;
    private bool _isAllowedFishing = true;
    private bool _isEnable = false;
    private FishingRodItemData _currentFishingRod;
    private float _fishingDelay = 1.5f;
    private float _timer = 0;
    private int _maxRandomNumder = 100;
    private int _addAmount = 1;

    private void Awake()
    {
        _questControl = GetComponent<QuestControl>();
    }

    private void OnEnable()
    {
        _inputHandler.InteractionPlayerInput.OnUse += ThrowFishingRod;
        _playerInteraction.OnUpdateItemData += Init;
        _float.FishCaught += CatchFish;
        _float.FishMissed += MissFish;
    }

    private void OnDisable()
    {
        _inputHandler.InteractionPlayerInput.OnUse += ThrowFishingRod;
        _playerInteraction.OnUpdateItemData -= Init;
        _float.FishCaught -= CatchFish;
        _float.FishMissed -= MissFish;
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, _float.transform.position) > _distance && _isEnable)
        {
            RebornToRod();
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
            RebornToRod();
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
                _float.StartFishing(_velocityForse, _currentFishingRod.RandomTime, GetRandomItem());
            }
            else
            {
                RebornToRod();
            }
        }
    }

    private void RebornToRod()
    {
        _float.ReturnToRod(transform);
        _isFishing = false;
        _isAllowedFishing = false;
    }

    private InventoryItemData GetRandomItem() 
    {
        int randomValue = Random.Range(0, _maxRandomNumder);
        int currentProbability = 0;

        foreach (var extraction in _currentFishingRod.Extractions)
        {
            currentProbability += (int)(extraction.Chance * _maxRandomNumder);

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
        {
            _inventoryHolder.AddToInventory(inventoryItemData, _addAmount);
            _playerInteraction.UpdateDurabilityItem(_playerInteraction.CurrentInventorySlot);
            _questControl.SendToMessageSystem(MessageConstants.Сaught + inventoryItemData.Name);
        }
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
