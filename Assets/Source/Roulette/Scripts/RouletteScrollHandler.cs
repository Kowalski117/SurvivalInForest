using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ItemsSpawner))]
public class RouletteScrollHandler : MonoBehaviour
{
    private const float WaitingTime = 1f;
    private const float Delay = 9;
    private const float MinDelay = 0.5f;

    [SerializeField] private ScrollRect _scrollRect;
    [SerializeField] private PlayerInventoryHolder _playerInventoryHolder;
    [SerializeField] private PlayerHandler _playerHandler;
    [SerializeField] private RouletteScreen _screen;
    [SerializeField] private RouletteSlot _slot;
    [SerializeField] private Timer _timer;
    [SerializeField] private YandexAds _andexAds;
    [SerializeField] private ScreenAnimation _revardImage;
    [SerializeField] private Button _closeScreen;
    [SerializeField] private Button _buttonScroll;
    [SerializeField] private Transform _container;

    private ItemsSpawner _spawner;
    private Tween _tween;
    private Coroutine _twistCoroutine;
    private Coroutine _claimCoroutine;

    private float _minPosition = 0.501f;
    private float _maxPosition = 0.509f;
    private float _defoultPosition = 0.50505f;
    private bool _isFirstScroll = true;
    private bool _isScroll = false;

    private WaitForSeconds _waitingTimeYield = new WaitForSeconds(WaitingTime);
    private WaitForSeconds _delayYield = new WaitForSeconds(Delay);
    private WaitForSeconds _minDelayYield = new WaitForSeconds(MinDelay);

    public event Action OnScrolling;
    public event Action<Dictionary<InventoryItemData, int>> OnBonusShown;

    private void Awake()
    {
        _spawner = GetComponent<ItemsSpawner>();

        Load();

        if (_isFirstScroll)
            _revardImage.Close();
        else
            _revardImage.Open();
    }

    private void Start()
    {
        _timer.IsCheckState();

        if (!_timer.IsClaimReward)
            _buttonScroll.enabled = false;
    }

    private void OnEnable()
    {
        _screen.OnScreenOpened += StartCoroutine;
        _screen.OnScreenClosed += StopCoroutine;

        _timer.OnTimerExpired += ExpireTimer;

        SavingGame.OnGameSaved += Save;
        SavingGame.OnGameSaved += Load;
        SavingGame.OnSaveDeleted += Delete;
    }

    private void OnDisable()
    {
        _screen.OnScreenOpened -= StartCoroutine;
        _screen.OnScreenClosed -= StopCoroutine;

        _timer.OnTimerExpired -= ExpireTimer;

        SavingGame.OnGameSaved -= Save;
        SavingGame.OnGameSaved -= Load;
        SavingGame.OnSaveDeleted -= Delete;
    }

    public void TwistButtonClick()
    {
        if (!_timer.IsClaimReward)
            return;

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

        _closeScreen.enabled = false;
        _buttonScroll.enabled = false;

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

        _claimCoroutine = StartCoroutine(UpdateState());

        if(!_isScroll)
            _container.localPosition = Vector2.zero;
    }

    private IEnumerator UpdateState()
    {
        VerifyState();
        yield return _waitingTimeYield;

        StartCoroutine();
    }

    private void ExpireTimer()
    {
        _revardImage.Open();
        _buttonScroll.enabled = true;
    }

    private void VerifyState()
    {
        _timer.IsCheckState();
        _timer.UpdateRewardsUI();
    }

    private IEnumerator StartScroll()
    {
        _playerHandler.ToggleScreenPlayerInput(false);
        _isScroll = true;
        _spawner.Spawn();
        yield return _scrollRect.normalizedPosition = Vector2.zero;

        float position = UnityEngine.Random.Range(_minPosition, _maxPosition);
       _tween = _scrollRect.DOHorizontalNormalizedPos(position, Delay).SetEase(Ease.OutQuint);
        yield return _delayYield;

        _tween.Kill();

        if (position % 1 != 0) 
        {
            yield return _minDelayYield;
            _tween = _scrollRect.DOHorizontalNormalizedPos(_defoultPosition, MinDelay);
        }

        yield return _minDelayYield;

        AddItem();
        _timer.SetLastClaimTime();

        _closeScreen.enabled = true;

        _playerHandler.ToggleScreenPlayerInput(true);
        OnScrolling?.Invoke();
        _isScroll = false;
        _revardImage.Close();
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
            _isFirstScroll = ES3.Load<bool>(SaveLoadConstants.IsFirstScroll);
    }

    private void Delete()
    {
        if (ES3.KeyExists(SaveLoadConstants.IsFirstScroll))
            ES3.DeleteKey(SaveLoadConstants.IsFirstScroll);
    }
}
