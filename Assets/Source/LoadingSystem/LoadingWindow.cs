using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LoadingWindow : MonoBehaviour
{
    [SerializeField] private Transform _loadingPanel;
    [SerializeField] private Image _loadingBar;
    [SerializeField] private TimeHandler _timeHandler;
    [SerializeField] private PlayerInputHandler _playerInputHandler;
    [SerializeField] private TMP_Text _text;
    [SerializeField] private TMP_Text _timeText;

    private DateTime time;
    private Coroutine _coroutine;
    public event UnityAction OnLoadingComplete;

    public void ShowLoadingWindow(float delay, float skipTime, string name)
    {
        if(_coroutine != null)
        {
            StopCoroutine(_coroutine);
            _coroutine = null;
        }

        _coroutine = StartCoroutine(LoadingRoutine(delay, skipTime, name));
    }

    private IEnumerator LoadingRoutine(float delay, float skipTime, string name)
    {
        DateTime times;
        _playerInputHandler.ToggleAllInput(false);
        _loadingPanel.gameObject.SetActive(true);

        times = time + TimeSpan.FromHours(skipTime);

        _text.text = $"Крафтится {name}, затраченное время - {time.ToString("HH:mm")}";
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
        
        _timeHandler.AddTimeInHours(skipTime);
        _loadingPanel.gameObject.SetActive(false);
        _playerInputHandler.ToggleAllInput(true);
        OnLoadingComplete?.Invoke();
    }
}
