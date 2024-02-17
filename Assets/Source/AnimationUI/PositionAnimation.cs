using DG.Tweening;
using System.Collections;
using UnityEngine;

public class PositionAnimation : AnimationUI
{
    [SerializeField] private Vector3 _positonStart;
    [SerializeField] private Vector3 _positonFinish;

    [SerializeField] private bool _defoultY;

    protected override IEnumerator Open()
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

        if (_defoultY)
        {
            Panel.transform.localPosition = new Vector3(_positonStart.x, Panel.transform.localPosition.y, _positonStart.z);
            Tween = Panel.transform.DOLocalMove(new Vector3(_positonFinish.x, Panel.transform.localPosition.y, _positonFinish.z), DurationAnim);
        }
        else
        {
            Panel.transform.localPosition = _positonStart;
            Tween = Panel.transform.DOLocalMove(_positonFinish, DurationAnim);
        }

        if (IsStartingInOrder)
            yield return Delay = new WaitForSeconds(DurationAnim);

        SetChildren(true);
    }

    protected override IEnumerator Close()
    {
        SetChildren(false);

        if (IsStartingInOrder)
            yield return Delay = new WaitForSeconds(DurationAnim);

        ClearTween();

        if (_defoultY)
        {
            Panel.transform.localPosition = new Vector3(_positonFinish.x, Panel.transform.localPosition.y, _positonFinish.z);
            Tween = Panel.transform.DOLocalMove(new Vector3(_positonStart.x, Panel.transform.localPosition.y, _positonStart.z), DurationAnim);
        }
        else
        {
            Panel.transform.localPosition = _positonFinish;
            Tween = Panel.transform.DOLocalMove(_positonStart, DurationAnim);
        }

        yield return Delay = new WaitForSeconds(DurationAnim);

        SetCanvasGroup(false);
    }
}
