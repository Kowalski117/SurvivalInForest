using System.Collections;
using UnityEngine;
using DG.Tweening;

public class BloodyScreen : MonoBehaviour
{
    [SerializeField] private PlayerHealth _playerHealth;
    [SerializeField] private CanvasGroup _screen;
    [SerializeField] private float _alphaPlus;
    [SerializeField] private float _speenAlpha;

    private Tween _bloodyTween;
    private Coroutine _showBloodCoroutine;
    private Coroutine _removeBloodCoroutine;

    private void OnEnable()
    {
        _playerHealth.OnDamageDone += ShowBlood;
        _playerHealth.OnRestoringHealth += RemoveBlood;
        _playerHealth.OnRevived += TurnOffBlow;
    }

    private void OnDisable()
    {
        _playerHealth.OnDamageDone -= ShowBlood;
        _playerHealth.OnRestoringHealth -= RemoveBlood;
        _playerHealth.OnRevived -= TurnOffBlow;
    }

    private void TurnOffBlow()
    {
        _screen.alpha = 0;
        ClearTween();
    }

    private void ShowBlood()
    {
        if (_removeBloodCoroutine != null)
        {
            StopCoroutine(_removeBloodCoroutine);
        }

        if (_showBloodCoroutine != null)
        {
            StopCoroutine(_showBloodCoroutine);
        }

        _showBloodCoroutine = StartCoroutine(ChangeAlpha(_screen, _screen.alpha + _alphaPlus, _speenAlpha));
    }

    private void RemoveBlood()
    {
        if (_showBloodCoroutine != null)
        {
            StopCoroutine(_showBloodCoroutine);
        }

        if (_removeBloodCoroutine != null)
        {
            StopCoroutine(_removeBloodCoroutine);
        }

        _removeBloodCoroutine = StartCoroutine(ChangeAlpha(_screen, -_screen.alpha, _speenAlpha));
    }

    private IEnumerator ChangeAlpha(CanvasGroup canvasGroup, float targetAlpha, float delay)
    {
        ClearTween();
        _bloodyTween = canvasGroup.DOFade(canvasGroup.alpha + targetAlpha, delay);
        yield return new WaitForSeconds(delay);
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
