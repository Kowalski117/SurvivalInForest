using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Float : MonoBehaviour
{
    [SerializeField] private FishingRod _fishingRod;
    [SerializeField] private ParticleSystem _startInWaterParticle;
    [SerializeField] private ParticleSystem _waterParticle;

    private Rigidbody _rigidbody;
    private Vector3 _initialPosition;
    private Vector3 _positionInWater;
    private Quaternion _initialRotation;
    private Tween _fishingTween;
    private Coroutine _fishingCoroutine;
    private bool _isFishOnHook = false;
    private Vector2 _randomTime;
    private InventoryItemData _currentExtraction;

    private Vector3 _calmWaterOffset = new Vector3(0, -0.15f, 0);
    private Vector2 _fishOnHookOffset = new Vector2(0.4f, 1f);
    private float _timeBeforeFishEscapes = 3f;
    private float _animationDuration = 2f;
    private float _returnDelay = 1f;
    private int _numberOfRepetitions = -1;

    public event UnityAction<InventoryItemData> FishCaught;
    public event UnityAction FishMissed;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _initialPosition = transform.localPosition;
        _initialRotation = transform.localRotation;
    }

    public void StartFishing(float velocity, Vector2 randomTime, InventoryItemData extraction)
    {
        transform.parent = null;
        _rigidbody.isKinematic = false;
        _rigidbody.velocity = transform.forward * velocity;
        _randomTime = randomTime;
        _currentExtraction = extraction;
    }

    public void ReturnToRod(Transform parent)
    {
        _rigidbody.isKinematic = true;

        if (_fishingCoroutine != null)
        {
            StopCoroutine(_fishingCoroutine);
        }

        transform.parent = parent;

        _waterParticle.Stop();
        _waterParticle.gameObject.SetActive(false);
        _startInWaterParticle.gameObject.SetActive(false);
        SetPositionParticle(_startInWaterParticle, transform, transform.position);
        SetPositionParticle(_waterParticle, transform, transform.position);

        ClearTween();
        if (transform.position != _initialPosition)
            _fishingTween = transform.DOLocalMove(_initialPosition, _returnDelay);
        transform.localRotation = _initialRotation;

        if (_isFishOnHook)
        {
            FishCaught?.Invoke(_currentExtraction);
            _isFishOnHook = false;
        }

        _randomTime = Vector2.zero;
        _currentExtraction = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Water water))
        {
            _rigidbody.isKinematic = true;
            StartFishingOverTime();
            _startInWaterParticle.gameObject.SetActive(true);
            _startInWaterParticle.Play();
            SetPositionParticle(_startInWaterParticle, null, transform.position);
        }
    }

    private void StartFishingOverTime()
    {
        if (_fishingCoroutine != null)
        {
            StopCoroutine(_fishingCoroutine);
        }

        _fishingCoroutine = StartCoroutine(FishingOverTime());
    }

    private IEnumerator FishingOverTime()
    {
        ClearTween();
        _positionInWater = transform.position;
        _fishingTween = transform.DOMove(_positionInWater + _calmWaterOffset, _animationDuration).SetLoops(_numberOfRepetitions, LoopType.Yoyo);

        _waterParticle.gameObject.SetActive(true);
        _waterParticle.Play();
        SetPositionParticle(_waterParticle, null, _positionInWater);
        _isFishOnHook = false;

        yield return new WaitForSeconds(GetRandomNumber(_randomTime.x, _randomTime.y));

        _waterParticle.Stop();
        ClearTween();
        _fishingTween = transform.DOMove(_positionInWater - GetRandomOffset(_fishOnHookOffset), _timeBeforeFishEscapes).SetLoops(_numberOfRepetitions, LoopType.Yoyo);
        _isFishOnHook = true;

        yield return new WaitForSeconds(_timeBeforeFishEscapes);

        ClearTween();
        _fishingTween = transform.DOMove(new Vector3(transform.position.x, _positionInWater.y, transform.position.z), _returnDelay);
        _isFishOnHook = false;

        yield return new WaitForSeconds(_returnDelay);

        ReturnToRod(_fishingRod.transform);
        FishMissed?.Invoke();
    }

    private void ClearTween()
    {
        if (_fishingTween != null && _fishingTween.IsActive())
        {
            _fishingTween.Kill();
            _fishingTween = null;
        }
    }

    private float GetRandomNumber(float min, float max)
    {
        return Random.Range(min, max);
    }

    private Vector3 GetRandomOffset(Vector2 range)
    {
        return new Vector3(GetRandomNumber(-range.x, range.y), GetRandomNumber(range.x, range.y), GetRandomNumber(-range.x, range.y));
    }

    private void SetPositionParticle(ParticleSystem particleSystem, Transform parent, Vector3 position)
    {
        particleSystem.gameObject.transform.parent = parent;
        particleSystem.gameObject.transform.position = position;
    }
}
