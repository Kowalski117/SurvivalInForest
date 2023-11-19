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
    private bool _isPlayCoroutine = false;

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
        _isPlayCoroutine = false;
    }

    private void ClearBlow()
    {
        _screen.alpha = 0;
        StopCoroutine(_showBloodCoroutine);
        ClearTween();
    }

    private void ShowBlood()
    {
        if(!_isPlayCoroutine)
        {
            if (_showBloodCoroutine != null)
            {
                StopCoroutine(_showBloodCoroutine);
            }

            _showBloodCoroutine = StartCoroutine(ChangeAlpha(_screen, _speenAlpha, _repeatDelay));
        }
    }

    private IEnumerator ChangeAlpha(CanvasGroup canvasGroup, float delay, float repeatDelay)
    {
        ClearTween();
        _isPlayCoroutine = true;
        _bloodyTween = canvasGroup.DOFade(1, delay);
        yield return new WaitForSeconds(delay);
        _bloodyTween = canvasGroup.DOFade(0, delay);
        yield return new WaitForSeconds(repeatDelay);

        if(_isPlayCoroutine)
            _showBloodCoroutine = StartCoroutine(ChangeAlpha(canvasGroup, delay, repeatDelay));
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
