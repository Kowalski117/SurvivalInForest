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
    [SerializeField] private bool _isOpen = true;
    [SerializeField] private bool _isActiveGroup = true;
    [SerializeField] private bool _isObjectActiveToggle = false;

    protected Tween Tween;
    protected Coroutine Coroutine;
    protected WaitForSeconds DelayWait;

    private WaitForSeconds _groupWait;
    private Coroutine _groupCoroutine;

    private Tween _tweenGroup;
    private bool _isActive = true;

    public bool IsOpen => _isOpen;

    private void Awake()
    {
        DelayWait = new WaitForSeconds(DurationAnim);
        _groupWait = new WaitForSeconds(_delayGroup);
    }

    public void Open()
    {
        if (!_isActive || _isOpen)
            return;

        ClearTween();
        ClearCoroutine(ref Coroutine);

        Coroutine = StartCoroutine(OpenWaitFor());

        _isOpen = true;
    }

    public void Close()
    {
        if (!_isOpen)
            return;

        ClearTween();
        ClearCoroutine(ref Coroutine);

        Coroutine = StartCoroutine(CloseWaitFor());

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

        ClearCoroutine(ref _groupCoroutine);
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

        if(_isObjectActiveToggle)
        {
            if (isActive)
            {
                CanvasGroup.gameObject.SetActive(true);
            }
            else
                StartTurnOffObject();
        }

        if (!isActive && _isObjectDisactiveEnd)
            SetActivePanel(false);
    }

    protected abstract IEnumerator OpenWaitFor();

    protected abstract IEnumerator CloseWaitFor();

    protected void SetChildren(bool isActive)
    {
        if (Children.Length > 0)
        {
            for (int i = 0; i < Children.Length; i++)
            {
                if(isActive)
                    Children[i].Open();
                else
                    Children[i].Close();
            }
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

    

    private void StartTurnOffObject()
    {
        ClearCoroutine(ref _groupCoroutine);

         _groupCoroutine = StartCoroutine(TurnOffObject());
    }

    private IEnumerator TurnOffObject()
    {
        yield return _groupWait;

        CanvasGroup.gameObject.SetActive(false);
    }


    private void ClearCoroutine(ref Coroutine coroutine)
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }
    }
}
