using DG.Tweening;
using System.Collections;
using UnityEngine;

public class ItemInWater : MonoBehaviour
{
    private ItemPickUp _itemPickUp;
    private Tween _popsUpTween;

    private int _groundLayer = 7;
    private Vector3 _calmWaterOffset = new Vector3(0, 0.15f, 0);
    private float _animationDuration = 2f;
    private int _numberOfRepetitions = -1;
    private float _drag = 10;
    private float _delay = 1f;
    private bool _isGrounded = false;

    private void Awake()
    {
        _itemPickUp = GetComponent<ItemPickUp>();
    }

    private void PopsUp()
    {
        _popsUpTween = transform.DOMove(transform.position + _calmWaterOffset, _animationDuration).SetLoops(_numberOfRepetitions, LoopType.Yoyo);
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
                    StartCoroutine(WaitForSoundToFinish(_delay));
                }
                else
                {
                    ClearTween();
                    PopsUp();
                }
            }
            else if(_itemPickUp.ItemData.TypeBehaviorInWater == TypeBehaviorInWater.Sinking)
            {
                _itemPickUp.Rigidbody.drag = _drag;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Water water))
        {
            if (_itemPickUp.ItemData.TypeBehaviorInWater == TypeBehaviorInWater.PopsUp)
            {
                ClearTween();
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.gameObject.layer == _groundLayer)
        {
            _isGrounded = true;
        }
    }

    private void ClearTween()
    {
        if (_popsUpTween != null && _popsUpTween.IsActive())
        {
            _popsUpTween.Kill();
            _popsUpTween = null;
        }
    }

    private IEnumerator WaitForSoundToFinish(float duration)
    {
        yield return new WaitForSeconds(duration);
        ClearTween();
        PopsUp();
    }
}
