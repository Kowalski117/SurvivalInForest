using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonAnimation : MonoBehaviour, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Vector3 _minScale = new Vector3(0.7f, 0.7f, 0.7f);
    [SerializeField] private float _delay = 0.15f;

    private Button _button;
    private Tween _tween;
    private Vector3 _defaultScale = Vector3.one;
    private WaitForSeconds _waitForSeconds;
    private Coroutine _coroutine;
    private bool _isPointerOverButton = false;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (_isPointerOverButton && _button.enabled)
        {
            StartCoroutine();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _isPointerOverButton = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _isPointerOverButton = false;
    }

    private void StartCoroutine()
    {
        if(_coroutine != null)
        {
            StopCoroutine(_coroutine);
            _coroutine = null;
        }

        _coroutine = StartCoroutine(ÑhangeScale());
    }

    private IEnumerator ÑhangeScale()
    {
        ResetTween();
        _tween = _button.transform.DOScale(_minScale, _delay);
        yield return _waitForSeconds = new WaitForSeconds(_delay);
        ResetTween();
        _tween = _button.transform.DOScale(_defaultScale, _delay);
    }

    private void ResetTween()
    {
        if(_tween != null)
        {
            _tween.Kill();
            _tween = null;
        }
    }
}
