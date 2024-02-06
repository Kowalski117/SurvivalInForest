using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class RouletteScrollHandler : MonoBehaviour
{
    [SerializeField] private ScrollRect _scrollRect;
    [SerializeField] private PlayerInventoryHolder _playerInventoryHolder;
    [SerializeField] private PlayerHandler _playerHandler;
    [SerializeField] private RouletteScreen _screen;
    [SerializeField] private RouletteSlot _slot;
    [SerializeField] private Timer _timer;
    [SerializeField] private YandexAds _andexAds;
    [SerializeField] private GameObject _revardImage;
    [SerializeField] private GameObject _closeScreen;

    private ItemsSpawner _spawner;
    private Tween _tween;
    private Coroutine _twistCoroutine;
    private Coroutine _claimCoroutine;
    private float _minPosition = 0.501f;
    private float _maxPosition = 0.509f;
    private float _defoultPosition = 0.50505f;
    private float _delay = 10f;
    private float _minDelay = 0.5f;
    private bool _isFirstScroll = true;

    public event UnityAction OnScroll;
    public event UnityAction<Dictionary<InventoryItemData, int>> OnBonusShown;

    void Start()
    {
        _spawner = GetComponent<ItemsSpawner>();

        if (_isFirstScroll)
        {
            _revardImage.SetActive(false);
        }
    }

    private void OnEnable()
    {
        _screen.OnOpenScreen += StartCoroutine;
        _screen.OnCloseScreen += StopCoroutine;
        SaveGame.OnSaveGame += Save;
        SaveGame.OnSaveGame += Load;
    }

    private void OnDisable()
    {
        _screen.OnOpenScreen -= StartCoroutine;
        _screen.OnCloseScreen -= StopCoroutine;
        SaveGame.OnSaveGame -= Save;
        SaveGame.OnSaveGame -= Load;
    }

    public void TwistButtonClick()
    {
        if (!_timer.IsClaimReward)
            return;

        _playerHandler.ToggleScreenPlayerInput(false);

        if (_isFirstScroll)
        {
            StartTwist();
            _isFirstScroll = false;
        }
        else
            _andexAds.ShowRewardAd(() => StartTwist());
    }

    private void StartTwist()
    {
        if (!_timer.IsClaimReward)
            return;

        if (_twistCoroutine != null)
        {
            StopCoroutine(_twistCoroutine);
            _tween.Kill();
        }

        _closeScreen.SetActive(false);
        _twistCoroutine = StartCoroutine(StartScroll());
    }

    public void StopCoroutine()
    {
        if (_claimCoroutine != null)
        {
            StopCoroutine(_claimCoroutine);
            _claimCoroutine = null;
        }

        if (_twistCoroutine != null)
        {
            StopCoroutine(_twistCoroutine);
            _tween.Kill();
        }
    }

    private void StartCoroutine()
    {
        if (_claimCoroutine != null)
        {
            StopCoroutine(_claimCoroutine);
            _claimCoroutine = null;
        }

        _claimCoroutine = StartCoroutine(RewardsStateUpdate());
    }

    private IEnumerator RewardsStateUpdate()
    {
        UpdateRewardsState();
        yield return new WaitForSeconds(1f);

        StartCoroutine();
    }

    private void UpdateRewardsState()
    {
        _timer.IsCheckState();
        _timer.UpdateRewardsUI();
    }

    private IEnumerator StartScroll()
    {
        _spawner.SpawnItems();
        yield return _scrollRect.normalizedPosition = Vector2.zero;

        float position = Random.Range(_minPosition, _maxPosition);
       _tween = _scrollRect.DOHorizontalNormalizedPos(position, _delay).SetEase(Ease.OutQuint);
        yield return new WaitForSeconds(_delay - _minDelay - _minDelay);
        _tween.Kill();
        if (position % 1 != 0) 
        {
            yield return new WaitForSeconds(_minDelay);
            _tween = _scrollRect.DOHorizontalNormalizedPos(_defoultPosition, _minDelay);
        }

        yield return new WaitForSeconds(_minDelay);

        AddItem();
        _timer.SetLastClaimTime();

        if (!_revardImage.activeInHierarchy)
            _revardImage.SetActive(true);

        _closeScreen.SetActive(true);
        _playerHandler.ToggleScreenPlayerInput(true);
        OnScroll?.Invoke();
    }

    private void AddItem()
    {
        Dictionary<InventoryItemData, int> items = new Dictionary<InventoryItemData, int>();
        items.Add(_slot.InventorySlotUI.AssignedInventorySlot.ItemData, _slot.InventorySlotUI.AssignedInventorySlot.Size);

        OnBonusShown?.Invoke(items);
    }

    private void Save()
    {
        ES3.Save(SaveLoadConstants.IsFirstScroll, _isFirstScroll);
    }

    private void Load()
    {
        if (ES3.KeyExists(SaveLoadConstants.IsFirstScroll))
        {
            _isFirstScroll = ES3.Load<bool>(SaveLoadConstants.IsFirstScroll);
        }
    }
}
