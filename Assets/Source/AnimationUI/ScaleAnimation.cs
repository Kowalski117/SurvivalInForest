using DG.Tweening;
using System.Collections;
using UnityEngine;

public class ScaleAnimation : AnimationUI
{
    [SerializeField] private Vector3 _scaleMin;
    [SerializeField] private Vector3 _scaleMax = Vector3.one;

    protected override IEnumerator OpenWaitFor()
    {
        if (Children.Length > 0)
        {
            foreach (var child in Children)
            {
                child.SetCanvasGroup(false);
            }
        }

        ClearTween();
        SetCanvasGroup(true);

        Panel.localScale = _scaleMin;
        Tween = Panel.DOScale(_scaleMax, DurationAnim);

        if (IsStartingInOrder)
            yield return DelayWait;

        SetChildren(true);
    }

    protected override IEnumerator CloseWaitFor()
    {
        SetChildren(false);

        if(IsStartingInOrder)
            yield return DelayWait;

        ClearTween();

        Panel.localScale = _scaleMax;
        Tween = Panel.DOScale(_scaleMin, DurationAnim);

        if(!IsUseDelayGroup)
            yield return DelayWait;

        SetCanvasGroup(false);
    }
}
