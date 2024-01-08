using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RouletteScrollHandler : MonoBehaviour
{
    [SerializeField] private ScrollRect _scrollRect;
    [SerializeField] private PlayerInventoryHolder _playerInventoryHolder;
    [SerializeField] private RouletteScreen _screen;
    [SerializeField] private RouletteSlot _slot;
    [SerializeField] private Timer _timer;

    private ItemsSpawner _spawner;
    private Tween _tween;
    private Coroutine _twistCoroutine;
    private Coroutine _claimCoroutine;
    private float _minPosition = 0.501f;
    private float _maxPosition = 0.509f;
    private float _defoultPosition = 0.50505f;
    private float _delay = 10f;
    private float _minDelay = 0.5f;

    void Start()
    {
        _spawner = GetComponent<ItemsSpawner>();
    }

    private void OnEnable()
    {
        _screen.OnOpenScreen += StartCoroutine;
        _screen.OnCloseScreen += StopCoroutine;
    }

    private void OnDisable()
    {
        _screen.OnOpenScreen -= StartCoroutine;
        _screen.OnCloseScreen -= StopCoroutine;
    }

    public void StartTwist()
    {
        if (!_timer.IsClaimReward)
            return;

        if (_twistCoroutine != null)
        {
            StopCoroutine(_twistCoroutine);
            _tween.Kill();
        }

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

        if(_playerInventoryHolder.AddToInventory(_slot.InventorySlotUI.AssignedInventorySlot.ItemData, 1, _slot.InventorySlotUI.AssignedInventorySlot.ItemData.Durability))
        {
            _timer.SetLastClaimTime();
        }
    }
}
