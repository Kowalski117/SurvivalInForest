using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadPanel : MonoBehaviour
{
    [SerializeField] private AudioHandler _audioHandler;
    [SerializeField] private PlayerInputHandler _playerInputHandler;
    [SerializeField] private LoadingScreenSettings _screenSettings;
    [SerializeField] private Image _imageHint;
    [SerializeField] private Image _loadBarImage;
    [SerializeField] private Transform _loadBarTransform;
    [SerializeField] private TMP_Text _sceneNameIndex;
    [SerializeField] private TMP_Text _textHint;
    [SerializeField] private TMP_Text _loadBarText;
    [SerializeField] private Button _resumeButton;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private GameObject _text;
    [SerializeField] private float _fadeSpeed;
    [SerializeField] private bool _isStart = false;

    private const float _waitForFadeTime = 4f;
    private const float _zeroVolume = -80f;

    private Coroutine _coroutine;
    private Coroutine _coroutine1;
    private Coroutine _coroutineDeactivate;
    private int _randomIndex = 0;

    public event UnityAction OnDeactivated;

    private void Start()
    {
        if (!_isStart)
            Load(0, null, SceneManager.GetActiveScene().buildIndex);
    }

    private void OnEnable()
    {
        _resumeButton.onClick.AddListener(Deactivate);
    }

    private void OnDisable()
    {
        _resumeButton.onClick.RemoveListener(Deactivate);
    }

    public void StartLoad(int index)
    {
        ES3.Save(SaveLoadConstants.StartLastSaveScene, false);
        ES3.Save(SaveLoadConstants.IsNewGame, true);
        LoadScene(index);
    }

    public void StartLoadLastSave()
    {
        if (ES3.KeyExists(SaveLoadConstants.SceneIndex))
        {
            int indexScene = ES3.Load<int>(SaveLoadConstants.SceneIndex);
            ES3.Save(SaveLoadConstants.StartLastSaveScene, true);
            ES3.Save(SaveLoadConstants.IsNewGame, false);
            LoadScene(indexScene);
        }
    }

    public void LoadScene(int index)
    {
        gameObject.SetActive(true);
        Load(1, () => SceneManager.LoadScene(index), index);
    }

    public void Load(float alpha, UnityAction OnFadingDone, int indexScene)
    {
        if (alpha == 1)
            _audioHandler.FadeIn();

        if (alpha == 0)
            _audioHandler.FadeOut();

        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
            _coroutine = null;
        }

         _coroutine = StartCoroutine(Fade(alpha, OnFadingDone, indexScene));
    }

    private IEnumerator Fade(float alpha, UnityAction OnFadingDone, int indexScene)
    {
        SetSettingsScreen(indexScene);
        _canvasGroup.blocksRaycasts = true;

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

            _canvasGroup.blocksRaycasts = true;
            _loadBarText.enabled = false;
            _loadBarTransform.gameObject.SetActive(false);
            _resumeButton.gameObject.SetActive(true);
        }
        else if (alpha == 1)
        {
            _resumeButton.gameObject.SetActive(false);

            if (_playerInputHandler != null)
                _playerInputHandler.PlayerHealth.SetActiveCollider(false);

            ES3.Save(SaveLoadConstants.LastSceneIndex, SceneManager.GetActiveScene().buildIndex);
            ES3.Save(SaveLoadConstants.NextSceneIndex, indexScene);
        }
        _loadBarTransform.gameObject.SetActive(true);
        _loadBarImage.fillAmount = 0;

        while (_canvasGroup.alpha != 1)
        {
            _canvasGroup.alpha = Mathf.MoveTowards(_canvasGroup.alpha, 1, Time.unscaledDeltaTime * _fadeSpeed);
            yield return null;
        }

        if (alpha == 1)
            yield return new WaitForSeconds(_waitForFadeTime);

        if (OnFadingDone != null)
            OnFadingDone();

        yield break;
    }

    public void Deactivate()
    {
        if (_playerInputHandler != null)
        {
            _playerInputHandler.ToggleAllParametrs(true);
            _playerInputHandler.PlayerHealth.SetActiveCollider(true);
        }

        if (_coroutineDeactivate != null)
        {
            StopCoroutine(_coroutineDeactivate);
            _coroutineDeactivate = null;
        }
        OnDeactivated?.Invoke();
        _coroutineDeactivate = StartCoroutine(DeactivateCoroutine());
    }

    public string GetNameSettingsScreen(int indexScene)
    {
        foreach (var sceneParameter in _screenSettings.SceneParameters)
        {
            if (indexScene == sceneParameter.SceneIndex)
                return sceneParameter.SceneName;
        }

        return null;
    }

    private IEnumerator DeactivateCoroutine()
    {
        while (_canvasGroup.alpha != 0)
        {
            _canvasGroup.alpha = Mathf.MoveTowards(_canvasGroup.alpha, 0, Time.deltaTime * _fadeSpeed);
            yield return null;
        }
        _canvasGroup.alpha = 0;
        gameObject.SetActive(false);
    }

    private void SetSettingsScreen(int indexScene)
    {
        _sceneNameIndex.text = GetNameSettingsScreen(indexScene);

        _randomIndex = Random.Range(0, _screenSettings.HintSprites.Length);
        _imageHint.sprite = _screenSettings.HintSprites[_randomIndex];

        _randomIndex = Random.Range(0, _screenSettings.HintTexts.Length);
        _textHint.text = _screenSettings.HintTexts[_randomIndex];
    }
}