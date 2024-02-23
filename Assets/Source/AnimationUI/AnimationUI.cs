using DG.Tweening;
using System.Collections;
using UnityEngine;

public abstract class AnimationUI : MonoBehaviour
{
    [SerializeField] protected RectTransform Panel;
    [SerializeField] protected float DurationAnim = 0.3f;
    [SerializeField] protected CanvasGroup CanvasGroup;
    [SerializeField] protected AnimationUI[] Children;
    [SerializeField] protected bool IsStartingInOrder = false;
    [SerializeField] protected bool IsUseDelayGroup = false;
    [SerializeField] private float _delayGroup = 0.3f;
    [SerializeField] private bool _isObjectDisactiveEnd = false;

    protected Tween Tween;
    protected Coroutine Coroutine;
    protected WaitForSeconds Delay;

    private Tween _tweenGroup;
    private bool _isActiveGroup = true;
    private bool _isActive = true;
    private bool _isOpen = true;

    public bool IsOpen => _isOpen;

    public void OpenAnimation()
    {
        if (!_isActive || _isOpen)
            return;

        ClearTween();
        ClearCoroutine();

        Coroutine = StartCoroutine(Open());

        _isOpen = true;
    }

    public void CloseAnimation()
    {
        if (!_isOpen)
            return;

        ClearTween();
        ClearCoroutine();

        Coroutine = StartCoroutine(Close());

        _isOpen = false;
    }

    public void SetActivePanel(bool isActive)
    {
        CanvasGroup.gameObject.SetActive(isActive);
    }

    public void SetActive(bool isActive)
    {
        _isActive = isActive;
    }

    public void SetCanvasGroup(bool isActive)
    {
        if (!CanvasGroup || isActive == _isActiveGroup)
            return;

        _tweenGroup.Kill();

        _isActiveGroup = isActive;
        CanvasGroup.blocksRaycasts = _isActiveGroup;

        if (IsUseDelayGroup)
        {
            if (isActive)
                _tweenGroup = CanvasGroup.DOFade(1, _delayGroup);
            else
                _tweenGroup = CanvasGroup.DOFade(0, _delayGroup);
        }
        else
        {
            if (isActive)
                CanvasGroup.alpha = 1;
            else
                CanvasGroup.alpha = 0;
        }

        if (!isActive && _isObjectDisactiveEnd)
            SetActivePanel(false);
    }

    protected abstract IEnumerator Open();

    protected abstract IEnumerator Close();

    protected void SetChildren(bool isActive)
    {
        if (Children.Length > 0)
        {
            for (int i = 0; i < Children.Length; i++)
            {
                if(isActive)
                    Children[i].OpenAnimation();
                else
                    Children[i].CloseAnimation();
            }
        }
    }

    protected void ClearCoroutine()
    {
        if(Coroutine != null)
        {
            StopCoroutine(Coroutine);
            Coroutine = null;
        }
    }

    protected void ClearTween()
    {
        if(Tween != null)
        {
            Tween.Kill();
            Tween = null;
        }
    }
}
