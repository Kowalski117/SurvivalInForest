using DG.Tweening;
using IL3DN;
using System.Collections;
using UnityEngine;

public class ItemInWater : MonoBehaviour
{
    private ItemPickUp _itemPickUp;
    private Tween _popsDownTween;
    private Tween _popsUpTween;
    private Coroutine _coroutine;
    private Vector3 _calmWaterOffset = new Vector3(0, 0.15f, 0);
    private Vector3 _shiftingPositionUp = new Vector3(0, 0.3f, 0);
    private float _animationDuration = 2f;
    private int _numberOfRepetitions = -1;
    private float _drag = 10;
    private float _delay = 2f;

    private void Awake()
    {
        _itemPickUp = GetComponent<ItemPickUp>();
    }

    private void PopsDown()
    {
        _popsDownTween = transform.DOMove(transform.position - _calmWaterOffset, _animationDuration).SetLoops(_numberOfRepetitions, LoopType.Yoyo);
        _itemPickUp.Rigidbody.isKinematic = true;
    }

    private void PopsUp(float delay)
    {
        _popsUpTween = transform.DOMove(transform.position + _shiftingPositionUp, delay);
        _itemPickUp.Rigidbody.isKinematic = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out Water water))
        {
            if(_itemPickUp.ItemData.TypeBehaviorInWater == TypeBehaviorInWater.PopsUp)
            {
                if(_coroutine != null)
                {
                    StopCoroutine(_coroutine);
                    _coroutine = null;
                }
                _coroutine = StartCoroutine(WaitForSoundToFinish(_delay, 1));
            }
            else if(_itemPickUp.ItemData.TypeBehaviorInWater == TypeBehaviorInWater.Sinking)
            {
                _itemPickUp.Rigidbody.drag = _drag;
            }
        }
    }

    private void ClearTween(Tween tween)
    {
        if (tween != null && tween.IsActive())
        {
            tween.Kill();
            tween = null;
        }
    }

    private IEnumerator WaitForSoundToFinish(float diveDelay, float liftingDelay)
    {
        yield return new WaitForSeconds(diveDelay);

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
            ClearTween(_popsDownTween);
            ClearTween(_popsUpTween);
            _itemPickUp.Rigidbody.isKinematic = false;
            yield break;
        }

        if (liftingDelay >= 0)
        {
            PopsUp(liftingDelay);
            yield return new WaitForSeconds(liftingDelay);
            ClearTween(_popsUpTween);
        }

        ClearTween(_popsDownTween);
        PopsDown();
    }
}
