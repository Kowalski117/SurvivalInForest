using DG.Tweening;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(ItemPickUp))]
public class ItemInWater : MonoBehaviour
{
    private const float DiveDelay = 2f;
    private const float LiftingDelay = 1f;

    private ItemPickUp _itemPickUp;
    private Tween _popsDownTween;
    private Tween _popsUpTween;
    private Coroutine _coroutine;
    private WaitForSeconds _diveWait = new WaitForSeconds(DiveDelay);
    private WaitForSeconds _liftingWait = new WaitForSeconds(LiftingDelay);

    private Vector3 _calmWaterOffset = new Vector3(0, 0.15f, 0);
    private Vector3 _shiftingPositionUp = new Vector3(0, 0.3f, 0);
    private float _animationDuration = 2f;
    private int _numberOfRepetitions = -1;
    private float _drag = 10;

    private void Awake()
    {
        _itemPickUp = GetComponent<ItemPickUp>();
    }

    private void PopsDown()
    {
        _popsDownTween = transform.DOMove(transform.position - _calmWaterOffset, _animationDuration).SetLoops(_numberOfRepetitions, LoopType.Yoyo);
        _itemPickUp.Rigidbody.isKinematic = true;
    }

    private void PopsUp()
    {
        _popsUpTween = transform.DOMove(transform.position + _shiftingPositionUp, LiftingDelay);
        _itemPickUp.Rigidbody.isKinematic = true;
    }


    private void ClearTween(ref Tween tween)
    {
        if (tween != null && tween.IsActive())
            tween.Kill();
    }

    private IEnumerator WaitForSoundToFinish()
    {
        yield return _diveWait;

        Collider[] colliders = Physics.OverlapSphere(transform.position, 0.5f);
        bool isInWater = false;

        foreach (var collider in colliders)
        {
            if (collider.GetComponent<Water>() != null)
            {
                isInWater = true;
                break;
            }
        }

        if (!isInWater)
        {
            ClearTween(ref _popsDownTween);
            ClearTween(ref _popsUpTween);
            _itemPickUp.Rigidbody.isKinematic = false;
            yield break;
        }

        PopsUp();
        yield return _liftingWait;
        ClearTween(ref _popsUpTween);

        ClearTween(ref _popsDownTween);
        PopsDown();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Water>())
        {
            if(_itemPickUp.ItemData.TypeBehaviorInWater == TypeBehaviorInWater.PopsUp)
            {
                if(_coroutine != null)
                {
                    StopCoroutine(_coroutine);
                    _coroutine = null;
                }

                _coroutine = StartCoroutine(WaitForSoundToFinish());
            }
            else if(_itemPickUp.ItemData.TypeBehaviorInWater == TypeBehaviorInWater.Sinking)
                _itemPickUp.Rigidbody.drag = _drag;
        }
    }
}
