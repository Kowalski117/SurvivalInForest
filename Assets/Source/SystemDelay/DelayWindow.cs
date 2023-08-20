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
    [SerializeField] private PlayerInputHandler _playerInputHandler;
    [SerializeField] private TMP_Text _text;
    [SerializeField] private TMP_Text _timeText;

    private DateTime _time;
    private Coroutine _coroutine;
    private bool _isLoading = false;

    public event UnityAction OnLoadingComplete;

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
        _isLoading = true;
        _playerInputHandler.ToggleAllInput(false);
        _loadingPanel.gameObject.SetActive(true);

        _time = _time + TimeSpan.FromHours(skipTime);

        string actionText = GetActionText(actionType, name);
        _text.text = actionText;

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

        yield return new WaitForSeconds(0.5f);
        _isLoading = false;
        _timeHandler.AddTimeInHours(skipTime);
        _loadingPanel.gameObject.SetActive(false);
        _playerInputHandler.ToggleAllInput(true);

        if (actionType == ActionType.CraftItem)
        {
            _playerInputHandler.ToggleHotbarDisplay(false);
            _playerInputHandler.ToggleBuildPlayerInput(false);
            _playerInputHandler.ToggleInteractionConstructionInput(false);
            _playerInputHandler.TogglePersonController(false);
        }
        else if (actionType == ActionType.CraftBuild)
        {
            _playerInputHandler.ToggleInteractionConstructionInput(false);
        }

        _time = DateTime.MinValue;

        OnLoadingComplete?.Invoke();
    }

    private string GetActionText(ActionType actionType, string name)
    {
        switch (actionType)
        {
            case ActionType.CraftItem:
                return $"Крафтится {name}, затраченное время - {_time.ToString("HH:mm")}";
            case ActionType.CraftBuild:
                return $"Строится {name}, затраченное время - {_time.ToString("HH:mm")}";
            case ActionType.Sleep:
                return $"Вы спите, затраченное время - {_time.ToString("HH:mm")}";
            default:
                return string.Empty;
        }
    }
}

public enum ActionType
{
    CraftItem,
    CraftBuild,
    Sleep
}
