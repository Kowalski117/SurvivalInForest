using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class CutScene : MonoBehaviour
{
    [SerializeField] private LoadPanel _loadPanel;
    [SerializeField] private PlayableDirector _playableDirector;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private float _fadeSpeed;

    private int _indexNextScene = 2;
    private int _waitForFadeTime = 3;

    private void OnEnable()
    {
        _loadPanel.OnDeactivated += PlayCutScene;
    }

    private void OnDisable()
    {
        _loadPanel.OnDeactivated -= PlayCutScene;
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
        _loadPanel.LoadScene(_indexNextScene);
    }

    private void PlayCutScene()
    {
        _playableDirector.Play();
    }
}
