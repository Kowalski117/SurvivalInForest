using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadPanel : MonoBehaviour
{
    [SerializeField] private AudioHandler _audioHandler;
    [SerializeField] private PlayerHandler _playerInputHandler;
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

    private const float WaitForFadeTime = 4f;
    private const float ZeroVolume = -80f;

    private Coroutine _coroutine;
    private Coroutine _coroutineDeactivate;
    private int _randomIndex = 0;
    private bool _isPlaying = false;

    public event UnityAction OnDeactivated;

    public bool IsPlaying => _isPlaying;

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
        //if (ES3.KeyExists(SaveLoadConstants.SceneIndex))
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
        _audioHandler.FadeIn();

        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
            _coroutine = null;
        }

         _coroutine = StartCoroutine(Fade(alpha, OnFadingDone, indexScene));
    }

    private IEnumerator Fade(float alpha, UnityAction OnFadingDone, int indexScene)
    {
        _isPlaying = true;
        SetSettingsScreen(indexScene);
        _canvasGroup.blocksRaycasts = true;

        if (alpha == 0)
        {
            _canvasGroup.alpha = 1;
            float elapsedTime = 0;
            float targetTime = WaitForFadeTime;
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
                _playerInputHandler.SetActiveCollider(false);

            ES3.Save(SaveLoadConstants.LastSceneIndex, SceneManager.GetActiveScene().buildIndex);
            ES3.Save(SaveLoadConstants.NextSceneIndex, indexScene);
        }
        //_loadBarTransform.gameObject.SetActive(true);
        _loadBarImage.fillAmount = 0;

        while (_canvasGroup.alpha != 1)
        {
            _canvasGroup.alpha = Mathf.MoveTowards(_canvasGroup.alpha, 1, Time.unscaledDeltaTime * _fadeSpeed);
            yield return null;
        }

        if (alpha == 1)
            yield return new WaitForSeconds(WaitForFadeTime);

        if (OnFadingDone != null)
        {
            OnFadingDone();
        }

        yield break;
    }

    public void Deactivate()
    {
        if (_playerInputHandler != null)
        {
            _playerInputHandler.ToggleAllParametrs(true);
            _playerInputHandler.ToggleAllInput(true);
            _playerInputHandler.TogglePersonController(true);
            _playerInputHandler.SetCursorVisible(false);
            _playerInputHandler.SetActiveCollider(true);
        }

        if (_coroutineDeactivate != null)
        {
            StopCoroutine(_coroutineDeactivate);
            _coroutineDeactivate = null;
        }
        OnDeactivated?.Invoke();
        _audioHandler.FadeOut();
        _isPlaying = false;
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
        
        _textHint.text = _screenSettings.HintText;
    }
}