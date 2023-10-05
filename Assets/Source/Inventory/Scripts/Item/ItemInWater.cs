using DG.Tweening;
using System.Collections;
using UnityEngine;

public class ItemInWater : MonoBehaviour
{
    private ItemPickUp _itemPickUp;
    private Tween _popsDownTween;
    private Tween _popsUpTween;

    private int _groundLayer = 7;
    private Vector3 _calmWaterOffset = new Vector3(0, 0.15f, 0);
    private Vector3 _shiftingPositionUp = new Vector3(0, 0.3f, 0);
    private float _animationDuration = 2f;
    private int _numberOfRepetitions = -1;
    private float _drag = 10;
    private float _delay = 2f;
    private bool _isGrounded = false;

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
                if (_isGrounded)
                {
                    StartCoroutine(WaitForSoundToFinish(_delay, 1));
                }
                else
                {
                    StartCoroutine(WaitForSoundToFinish(_delay, 1));
                }
            }
            else if(_itemPickUp.ItemData.TypeBehaviorInWater == TypeBehaviorInWater.Sinking)
            {
                _itemPickUp.Rigidbody.drag = _drag;
            }
        }
    }

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.TryGetComponent(out Water water))
    //    {
    //        if (_itemPickUp.ItemData.TypeBehaviorInWater == TypeBehaviorInWater.PopsUp)
    //        {
    //            ClearTween();
    //        }
    //    }
    //}

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.gameObject.layer == _groundLayer)
        {
            _isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.gameObject.layer == _groundLayer)
        {
            _isGrounded = false;
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

        if(liftingDelay >= 0)
        {
            PopsUp(liftingDelay);
            yield return new WaitForSeconds(liftingDelay);
            ClearTween(_popsUpTween);
        }

        ClearTween(_popsDownTween);
        PopsDown();
    }
}
