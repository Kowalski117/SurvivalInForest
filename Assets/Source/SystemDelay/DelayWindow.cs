using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DelayWindow : MonoBehaviour
{
    [SerializeField] private Transform _loadingPanel;
    [SerializeField] private Image _loadingBar;
    [SerializeField] private TimeHandler _timeHandler;
    [SerializeField] private SurvivalHandler _survivalHandler;
    [SerializeField] private PlayerInputHandler _playerInputHandler;
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private TMP_Text _timerText;

    private AudioSource _audioSource;
    private DateTime _time;
    private Coroutine _coroutine;
    private bool _isLoading = false;
    private float _delay = 0.5f;
    private float _maximumDivisor = 3;

    public event UnityAction OnLoadingComplete;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
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
        _playerInputHandler.ToggleAllInput(false);
        _playerInputHandler.SetCursorVisible(false);
        _playerInputHandler.ToggleScreenPlayerInput(false);
        _loadingPanel.gameObject.SetActive(true);

        _time = _time + TimeSpan.FromHours(skipTime);

        _nameText.text = name;
        _timerText.text = _time.ToString(GameConstants.HHmm);

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
        _loadingPanel.gameObject.SetActive(false);
        _playerInputHandler.ToggleAllInput(true);
        _playerInputHandler.ToggleScreenPlayerInput(true);

        _time = DateTime.MinValue;

        OnLoadingComplete?.Invoke();
        _audioSource.Stop();
    }
}

public enum ActionType
{
    CraftItem,
    CraftBuild,
    Sleep
}
