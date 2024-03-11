using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(DelayScreen))]
[RequireComponent(typeof(DelayAudio))]
public class DelayHandler : MonoBehaviour
{
    [SerializeField] private AnimationUI _animationUI;
    [SerializeField] private MainClock _timeHandler;
    [SerializeField] private SurvivalHandler _survivalHandler;
    [SerializeField] private PlayerHandler _playerInputHandler;

    private DelayScreen _screenView;
    private DelayAudio _audio;

    private DateTime _time;
    private bool _isLoading = false;
    private float _maximumDivisor = 3;

    private Coroutine _coroutine;

    private void Awake()
    {
        _screenView = GetComponent<DelayScreen>();
        _audio = GetComponent<DelayAudio>();
        _animationUI.Close();
    }

    public void ShowLoadingWindow(float delay, float skipTime, string name, ActionType actionType, Action OnCompleted)
    {
        if (!_isLoading)
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
                _coroutine = null;
            }

            _coroutine = StartCoroutine(LoadingRoutine(delay, skipTime, name, actionType, OnCompleted));
        }
    }

    private IEnumerator LoadingRoutine(float delay, float skipTime, string name, ActionType actionType, Action OnCompleted)
    {
        _time += TimeSpan.FromHours(skipTime);

        _screenView.UpdateText(name, _time.ToString(GameConstants.HHmm), actionType);
        Open();

        _screenView.FillAmount(delay);

        yield return new WaitForSeconds(delay);

        SkipTime(skipTime);
        Close(actionType);

        OnCompleted?.Invoke();

        _time = DateTime.MinValue;
    }

    private void SkipTime(float skipTime)
    {
        _timeHandler.AddTimeInHours(skipTime);
        _survivalHandler.Sleep.LowerValueInFours(skipTime / _maximumDivisor);
        _survivalHandler.Hunger.LowerValueInFours(skipTime / _maximumDivisor);
        _survivalHandler.Thirst.LowerValueInFours(skipTime / _maximumDivisor);
        _survivalHandler.TimeHandler.ToggleEnable(true);
    }

    private void Open()
    {
        _audio.Play();
        _isLoading = true;

        _survivalHandler.TimeHandler.ToggleEnable(false);
        _playerInputHandler.ToggleScreenPlayerInput(false);
        _playerInputHandler.ToggleAllInput(false);
        _playerInputHandler.TogglePersonController(false);
        _playerInputHandler.ToggleCamera(false);

        _animationUI.Open();
    }

    private void Close(ActionType actionType)
    {
        _isLoading = false;

        _playerInputHandler.ToggleAllInput(true);
        _playerInputHandler.TogglePersonController(true);
        _playerInputHandler.ToggleScreenPlayerInput(true);

        if (actionType == ActionType.CraftItem || actionType == ActionType.Sleep)
        {
            _playerInputHandler.ToggleHotbarDisplay(false);
            _playerInputHandler.ToggleInteractionInput(false);
        }
        else
        {
            _playerInputHandler.SetCursorVisible(false);
            _playerInputHandler.ToggleCamera(true);
        }

        _animationUI.Close();
        _audio.Stop();
    }
}

public enum ActionType
{
    CraftItem,
    CraftBuild,
    Sleep,
    Preparing
}
