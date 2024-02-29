using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class CutScene : MonoBehaviour
{
    private const int Delay = 3;

    [SerializeField] private LoadPanel _loadPanel;
    [SerializeField] private PlayableDirector _playableDirector;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private float _fadeDelay = 2;

    private Tween _tween;
    private Coroutine _coroutine;
    private WaitForSeconds _delayWait = new WaitForSeconds(Delay);

    public event Action OnStart;
    public event Action OnScip;
    public event Action OnFinish;

    private void OnEnable()
    {
        _loadPanel.OnDeactivated += PlayCutScene;
    }

    private void OnDisable()
    {
        _loadPanel.OnDeactivated -= PlayCutScene;
    }

    public void Skip()
    {
        StartCoroutine();
        _playableDirector.Stop();
        OnScip?.Invoke();
    }

    public void Finish()
    {
        StartCoroutine();
        OnFinish?.Invoke();
    }

    private void StartCoroutine()
    {
        if(_coroutine != null)
        {
            StopCoroutine(_coroutine);
            _coroutine = null;
        }

        _coroutine = StartCoroutine(FillAlpha());
    }

    private IEnumerator FillAlpha()
    {
        _tween.Kill();
        _tween = _canvasGroup.DOFade(1, _fadeDelay);

        yield return _delayWait;

        _loadPanel.StartLoad(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void PlayCutScene()
    {
        _playableDirector.Play();
        OnStart?.Invoke();
    }

    private void OnApplicationFocus(bool focus)
    {
        if (focus)
            _playableDirector.Resume();
        else
            _playableDirector.Pause();  
    }
}
