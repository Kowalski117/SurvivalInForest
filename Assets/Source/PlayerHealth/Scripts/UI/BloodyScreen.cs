using System.Collections;
using UnityEngine;
using DG.Tweening;

public class BloodyScreen : MonoBehaviour
{
    [SerializeField] private PlayerHealth _playerHealth;
    [SerializeField] private CanvasGroup _screen;
    [SerializeField] private float _speenAlpha;
    [SerializeField] private float _repeatDelay;

    private Tween _bloodyTween;
    private Coroutine _showBloodCoroutine;
    private WaitForSeconds _fadeWait;
    private WaitForSeconds _repeatWait;

    private float _maxAlpha = 1;
    private float _minAlpha = 0;

    private void Awake()
    {
        _fadeWait = new WaitForSeconds(_speenAlpha);
        _repeatWait = new WaitForSeconds(_repeatDelay);
    }

    private void OnEnable()
    {
        _playerHealth.OnDamageDone += ShowBlood;
        _playerHealth.OnRestoringHealth += TurnOffBlow;
        _playerHealth.OnRevived += ClearBlow;
    }

    private void OnDisable()
    {
        _playerHealth.OnDamageDone -= ShowBlood;
        _playerHealth.OnRestoringHealth -= TurnOffBlow;
        _playerHealth.OnRevived -= ClearBlow;
    }

    private void TurnOffBlow()
    {
        _screen.alpha = 0;
    }

    private void ClearBlow()
    {
        TurnOffBlow();

        if (_showBloodCoroutine != null)
        {
            StopCoroutine(_showBloodCoroutine);
            _showBloodCoroutine = null;
        }

        ClearTween();
    }

    private void ShowBlood()
    {
        if (_showBloodCoroutine == null)
            _showBloodCoroutine = StartCoroutine(ChangeAlpha());
    }

    private IEnumerator ChangeAlpha()
    {
        ClearTween();

        _bloodyTween = _screen.DOFade(_maxAlpha, _speenAlpha);
        yield return _fadeWait;

        _bloodyTween = _screen.DOFade(_minAlpha, _repeatDelay);
        yield return _repeatWait;

        StopCoroutine(_showBloodCoroutine);
    }

    private void ClearTween()
    {
        if (_bloodyTween != null && _bloodyTween.IsActive())
        {
            _bloodyTween.Kill();
            _bloodyTween = null;
        }
    }
}
