using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LoadPanel : MonoBehaviour
{
    //[SerializeField] private AudioManager _audioManager;
    [SerializeField] private PlayerInputHandler _playerInputHandler;
    [SerializeField] private Hints _hints;
    [SerializeField] private Image _imageHint;
    [SerializeField] private Image _loadBarImage;
    [SerializeField] private TMP_Text _textHint;
    [SerializeField] private TMP_Text _loadBarText;
    [SerializeField] private Button _resumeButton;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private GameObject _text;
    [SerializeField] private Image _panel;
    [SerializeField] private Image _image;
    [SerializeField] private float _fadeSpeed;
    [SerializeField] private bool _isStart = false;

    private const float _waitForFadeTime = 5f;
    private const float _zeroVolume = -80f;

    private Coroutine _coroutine;
    private Coroutine _coroutineDeactivate;
    private int _randomIndex = 0;

    private void Start()
    {
        if(!_isStart)
            Load(0, null);
    }

    private void OnEnable()
    {
        _resumeButton.onClick.AddListener(Deactivate);
    }

    private void OnDisable()
    {
        _resumeButton.onClick.RemoveListener(Deactivate);
    }

    public void Load(float alpha, UnityAction OnFadingDone)
    {
        //if (alpha == 1)
        //    _audioManager.FadeIn();

        //if (alpha == 0)
        //    _audioManager.FadeOut();

        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
            _coroutine = null;
        }

        _coroutine = StartCoroutine(Fade(alpha, OnFadingDone));
    }

    private IEnumerator Fade(float alpha, UnityAction OnFadingDone)
    {
        SetHint();

        if (alpha == 0)
        {
            _canvasGroup.alpha = 1;
            float elapsedTime = 0;
            float targetTime = _waitForFadeTime;
            _loadBarText.enabled = true;

            while (elapsedTime < targetTime)
            {
                float fillAmount = Mathf.Clamp01(elapsedTime / targetTime);
                _loadBarImage.fillAmount = fillAmount;
                int percentage = Mathf.RoundToInt(fillAmount * 100);
                _loadBarText.text = $"{percentage}%";
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            _loadBarText.enabled = false;
            _resumeButton.gameObject.SetActive(true);
        }
        else if (alpha == 1)
        {
            _resumeButton.gameObject.SetActive(false);
        }

        _loadBarImage.fillAmount = 0;

        while (_canvasGroup.alpha != 1)
        {
            _canvasGroup.alpha = Mathf.MoveTowards(_canvasGroup.alpha, 1, Time.unscaledDeltaTime * _fadeSpeed);
            yield return null;
        }

        if (OnFadingDone != null)
            OnFadingDone();

        yield break;
    }

    public void Deactivate()
    {
        if (_coroutineDeactivate != null)
        {
            StopCoroutine(_coroutineDeactivate);
            _coroutineDeactivate = null;
        }

        _coroutineDeactivate = StartCoroutine(DeactivateCoroutine());
    }

    private IEnumerator DeactivateCoroutine()
    {
        while (_canvasGroup.alpha != 0)
        {
            _canvasGroup.alpha = Mathf.MoveTowards(_canvasGroup.alpha, 0, Time.deltaTime * _fadeSpeed);
            yield return null;
        }

        gameObject.SetActive(false);

        if (_playerInputHandler != null)
            _playerInputHandler.SetCursorVisible(false);
    }

    private void SetHint()
    {
        _randomIndex = Random.Range(0, _hints.SpriteHints.Length);
        _imageHint.sprite = _hints.SpriteHints[_randomIndex];

        _randomIndex = Random.Range(0, _hints.TextHints.Length);
        _textHint.text = _hints.TextHints[_randomIndex];
    }
}
