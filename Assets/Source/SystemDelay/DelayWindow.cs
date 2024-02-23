using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DelayWindow : MonoBehaviour
{
    [SerializeField] private AnimationUI _animationUI;
    [SerializeField] private Image _loadingBar;
    [SerializeField] private MainClock _timeHandler;
    [SerializeField] private SurvivalHandler _survivalHandler;
    [SerializeField] private PlayerHandler _playerInputHandler;
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private TMP_Text _timerText;
    [SerializeField] private TMP_Text _actionText;

    private AudioSource _audioSource;
    private DateTime _time;
    private Coroutine _coroutine;
    private bool _isLoading = false;
    private float _delay = 0.5f;
    private float _maximumDivisor = 3;

    public event Action OnLoadingComplete;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _animationUI.CloseAnimation();
    }

    public void ShowLoadingWindow(float delay, float skipTime, string name, ActionType actionType)
    {
        if (!_isLoading)
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
                _coroutine = null;
            }

            _coroutine = StartCoroutine(LoadingRoutine(delay, skipTime, name, actionType));
        }
    }

    private IEnumerator LoadingRoutine(float delay, float skipTime, string name, ActionType actionType)
    {
        _audioSource.Play();
        _isLoading = true;
        _survivalHandler.TimeHandler.ToggleEnable(false);
        _playerInputHandler.ToggleScreenPlayerInput(false);
        _playerInputHandler.ToggleAllInput(false);
        _playerInputHandler.TogglePersonController(false);
        _playerInputHandler.ToggleCamera(false);

        _animationUI.OpenAnimation();

        _time += TimeSpan.FromHours(skipTime);

        _nameText.text = name;
        _timerText.text = _time.ToString(GameConstants.HHmm);
        SetActionText(actionType);
        float elapsedTime = 0f;
        float duration = delay;

        while (elapsedTime < duration)
        {
            float progress = elapsedTime / duration;
            _loadingBar.fillAmount = progress;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        _loadingBar.fillAmount = 1f;

        yield return new WaitForSeconds(_delay);

        _isLoading = false;
        _timeHandler.AddTimeInHours(skipTime);
        _survivalHandler.Sleep.LowerValueInFours(skipTime / _maximumDivisor);
        _survivalHandler.Hunger.LowerValueInFours(skipTime / _maximumDivisor);
        _survivalHandler.Thirst.LowerValueInFours(skipTime / _maximumDivisor);
        _survivalHandler.TimeHandler.ToggleEnable(true);
        _animationUI.CloseAnimation();

        _playerInputHandler.ToggleAllInput(true);
        _playerInputHandler.TogglePersonController(true);
        _playerInputHandler.ToggleScreenPlayerInput(true);

        if (actionType == ActionType.CraftItem)
        {
            _playerInputHandler.ToggleHotbarDisplay(false);
            _playerInputHandler.ToggleInteractionInput(false);
        }
        else
        {
            _playerInputHandler.SetCursorVisible(false);
            _playerInputHandler.ToggleCamera(true);
        }

        _time = DateTime.MinValue;

        OnLoadingComplete?.Invoke();
        _audioSource.Stop();
    }

    private void SetActionText(ActionType actionType)
    {
        if(actionType == ActionType.CraftItem)
        {
            _actionText.text = "Крафтится:";
        }
        else if(actionType == ActionType.CraftBuild)
        {
            _actionText.text = "Строится:";
        }
        else if(actionType == ActionType.Sleep) 
        {
            _actionText.text = "Спит:";
            _nameText.text = "...";
        }
        else if(actionType == ActionType.Preparing)
        {
            _actionText.text = "Готовится:";
        }
    }
}

public enum ActionType
{
    CraftItem,
    CraftBuild,
    Sleep,
    Preparing
}
