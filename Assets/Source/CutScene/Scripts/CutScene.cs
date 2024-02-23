using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class CutScene : MonoBehaviour
{
    [SerializeField] private LoadPanel _loadPanel;
    [SerializeField] private PlayableDirector _playableDirector;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private float _fadeSpeed;

    private int _waitForFadeTime = 3;

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

    public void StartCoroutine()
    {
        StartCoroutine(FillAlpha());
    }

    private IEnumerator FillAlpha()
    {
        while (_canvasGroup.alpha != 1)
        {
            _canvasGroup.alpha = Mathf.MoveTowards(_canvasGroup.alpha, 1, Time.deltaTime * _fadeSpeed);
            yield return null;
        }
        _canvasGroup.alpha = 1;

        yield return new WaitForSeconds(_waitForFadeTime);
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
