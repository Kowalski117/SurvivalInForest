using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(PixelCrushers.QuestMachine.QuestControl))]
public class FishingRod : MonoBehaviour
{
    private const float ThrowDelay = 0.75f;
    private const float TurnOffDelay = 1.5f;

    [SerializeField] private Float _float;
    [SerializeField] private FishingRodRenderer _renderer;
    [SerializeField] private PlayerEquipmentHandler _playerEquipmentHandler;
    [SerializeField] private PlayerHandler _inputHandler;
    [SerializeField] private PlayerInventoryHolder _inventoryHolder;
    [SerializeField] private HotbarDisplay _hotbarDisplay;
    [SerializeField] private PlayerAnimatorHandler _playerAnimatorHandler;
    [SerializeField] private float _distance = 10;
    [SerializeField] private float _velocityForse = 50;

    private bool _isFishing = false;
    private bool _isAllowedFishing = true;
    private bool _isEnable = false;
    private float _fishingDelay = 1.5f;
    private float _timer = 0;
    private int _maxRandomNumder = 100;
    private int _addAmount = 1;

    private FishingRodItemData _currentFishingRod;

    private PixelCrushers.QuestMachine.QuestControl _questControl;
    private Coroutine _throwCoroutine;
    private WaitForSeconds _throwWait = new WaitForSeconds(ThrowDelay);
    private Coroutine _turnOffRendererCoroutine;
    private WaitForSeconds _turnOffWait = new WaitForSeconds(TurnOffDelay);


    public event Action OnFishCaughted;

    private void Awake()
    {
        _questControl = GetComponent<PixelCrushers.QuestMachine.QuestControl>();
    }

    private void OnEnable()
    {
        _inputHandler.InteractionPlayerInput.OnUsed += StartThrow;
        _playerEquipmentHandler.OnUpdateItemData += InitItemData;
        _float.FishCaught += CatchFish;
        _float.FishMissed += MissFish;
    }

    private void OnDisable()
    {
        _inputHandler.InteractionPlayerInput.OnUsed += StartThrow;
        _playerEquipmentHandler.OnUpdateItemData -= InitItemData;
        _float.FishCaught -= CatchFish;
        _float.FishMissed -= MissFish;
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, _float.transform.position) > _distance && _isEnable)
            RebornToRod();

        if (!_isAllowedFishing)
        {
            if (_timer >= _fishingDelay)
            {
                _timer = 0;
                _isAllowedFishing = true;
            }
            else
                _timer += Time.deltaTime;
        }

        if (_currentFishingRod == null && _float.transform.position != transform.position)
            RebornToRod();
    }

    private void StartThrow()
    {
        if(_throwCoroutine != null)
        {
            StopCoroutine(_throwCoroutine);
            _throwCoroutine = null;
        }

        _throwCoroutine = StartCoroutine(Throw());
    }

    private IEnumerator Throw()
    {
        if (_isAllowedFishing && _isEnable)
        {
            if (!_isFishing && _float && _currentFishingRod)
            {
                if (_currentFishingRod)
                    _renderer.ToggleActive(true);

                _playerAnimatorHandler.SwingFishingRod();
                yield return _throwWait;
                _isFishing = true;

                _renderer.DrawRope();

                if(_currentFishingRod)
                    _float.StartFishing(_velocityForse, _currentFishingRod.RandomTime, GetRandomItem());
            }
            else
                RebornToRod();
        }
    }

    private void RebornToRod()
    {
        if(_isFishing)
            _playerAnimatorHandler.ThrowFishingRod();

        _float.ReturnToRod(transform);
        _isFishing = false;
        _isAllowedFishing = false;
    }

    private IEnumerator TurnOffRenderer()
    {
        yield return _turnOffWait;
        _renderer.ToggleActive(false);
    }

    private InventoryItemData GetRandomItem() 
    {
        int randomValue = UnityEngine.Random.Range(0, _maxRandomNumder);
        float currentProbability = 0;

        foreach (var extraction in _currentFishingRod.Extractions)
        {
            currentProbability += extraction.Chance * _maxRandomNumder;

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
            _inventoryHolder.AddItem(inventoryItemData, _addAmount);
            _playerEquipmentHandler.UpdateDurabilityItem(_playerEquipmentHandler.CurrentInventorySlot);
            _questControl.SendToMessageSystem(MessageConstants.Сaught + inventoryItemData.Name);
            OnFishCaughted?.Invoke();
        }
    }

    private void MissFish()
    {
        _isFishing = false;
    }

    private void InitItemData(InventoryItemData itemData)
    {
        if(itemData != null && itemData is FishingRodItemData fishingRodItem)
        {
            _currentFishingRod = fishingRodItem;
            _isEnable= true;
            _renderer.SetDefoultPoint(true);
            StopCoroutine(_turnOffRendererCoroutine);
        }
        else
        {
            _currentFishingRod = null;
            _isEnable = false;
            _renderer.SetDefoultPoint(false);
            _turnOffRendererCoroutine = StartCoroutine(TurnOffRenderer());
        }
    }
}
