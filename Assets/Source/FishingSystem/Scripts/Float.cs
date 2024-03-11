using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Float : MonoBehaviour
{
    private const float TimeBeforeFishEscapes = 1f;
    private const float AnimationDuration = 2f;
    private const float ReturnDelay = 1f;
    private const int NumberOfRepetitions = -1;

    [SerializeField] private FishingRod _fishingRod;
    [SerializeField] private ParticleSystem _startInWaterParticle;
    [SerializeField] private ParticleSystem _waterParticle;
    [SerializeField] private ParticleSystem _splashingFishParticle;

    private Rigidbody _rigidbody;
    private Vector3 _initialPosition;
    private Vector3 _positionInWater;
    private Quaternion _initialRotation;
    private Vector2 _randomTime;
    private bool _isFishOnHook = false;
    private InventoryItemData _currentExtraction;

    private Vector3 _calmWaterOffset = new Vector3(0, -0.15f, 0);
    private Vector2 _fishOnHookOffset = new Vector2(0.4f, 1f);

    private Tween _fishingTween;
    private Coroutine _fishingCoroutine;
    private WaitForSeconds _escapesWait = new WaitForSeconds(TimeBeforeFishEscapes);
    private WaitForSeconds _returnWait = new WaitForSeconds(ReturnDelay);

    public event Action<InventoryItemData> FishCaught;
    public event Action FishMissed;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _initialPosition = transform.localPosition;
        _initialRotation = transform.localRotation;
    }

    private void Update()
    {
        if(_splashingFishParticle.gameObject.activeInHierarchy)
            _splashingFishParticle.gameObject.transform.localPosition = new Vector3(transform.localPosition.x, _splashingFishParticle.gameObject.transform.position.y , transform.localPosition.z);
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

        ClearFishingCoroutine();

        transform.parent = parent;

        _waterParticle.Stop();
        _splashingFishParticle.Stop();
        SetPositionParticle(_startInWaterParticle, transform, transform.position);
        SetPositionParticle(_waterParticle, transform, transform.position);
        SetPositionParticle(_splashingFishParticle, transform, transform.position);
        _waterParticle.gameObject.SetActive(false);
        _startInWaterParticle.gameObject.SetActive(false);
        _splashingFishParticle.gameObject.SetActive(false);

        ClearTween();
        if (transform.position != _initialPosition)
            _fishingTween = transform.DOLocalMove(_initialPosition, ReturnDelay);

        transform.localRotation = _initialRotation;

        if (_isFishOnHook)
        {
            FishCaught?.Invoke(_currentExtraction);
            _isFishOnHook = false;
        }

        _randomTime = Vector2.zero;
        _currentExtraction = null;
    }

    private void StartFishingOverTime()
    {
        ClearFishingCoroutine();

        _fishingCoroutine = StartCoroutine(FishingOverTime());
    }

    private void ClearFishingCoroutine()
    {
        if (_fishingCoroutine != null)
        {
            StopCoroutine(_fishingCoroutine);
            _fishingCoroutine = null;
        }
    }

    private IEnumerator FishingOverTime()
    {
        ClearTween();
        _positionInWater = transform.position;
        _fishingTween = transform.DOMove(_positionInWater + _calmWaterOffset, AnimationDuration).SetLoops(NumberOfRepetitions, LoopType.Yoyo);

        _waterParticle.gameObject.SetActive(true);
        _waterParticle.Play();
        SetPositionParticle(_waterParticle, null, _positionInWater);
        SetPositionParticle(_splashingFishParticle, null, _positionInWater);
        _isFishOnHook = false;

        yield return new WaitForSeconds(GetRandomNumber(_randomTime.x, _randomTime.y));

        _waterParticle.Stop();
        _splashingFishParticle.gameObject.SetActive(true);
        _splashingFishParticle.Play();

        ClearTween();
        _fishingTween = transform.DOMove(_positionInWater - GetRandomOffset(_fishOnHookOffset), TimeBeforeFishEscapes).SetLoops(NumberOfRepetitions, LoopType.Yoyo);
        _isFishOnHook = true;

        yield return _escapesWait;

        _splashingFishParticle.Stop();
        ClearTween();
        _fishingTween = transform.DOMove(new Vector3(transform.position.x, _positionInWater.y, transform.position.z), ReturnDelay);
        _isFishOnHook = false;

        yield return _returnWait;

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
        return UnityEngine.Random.Range(min, max);
    }

    private Vector3 GetRandomOffset(Vector2 range)
    {
        return new Vector3(GetRandomNumber(-range.x, range.y), GetRandomNumber(range.x, range.y), GetRandomNumber(-range.x, range.y));
    }

    private void SetPositionParticle(ParticleSystem particleSystem, Transform parent, Vector3 position)
    {
        particleSystem.gameObject.transform.parent = parent;
        particleSystem.gameObject.transform.position = new Vector3(position.x, position.y + 0.15f, position.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Water>())
        {
            SetPositionParticle(_startInWaterParticle, null, transform.position);
            _rigidbody.isKinematic = true;
            StartFishingOverTime();
            _startInWaterParticle.gameObject.SetActive(true);
            _startInWaterParticle.Play();
        }
    }
}
