using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenObject : MonoBehaviour
{
    [SerializeField] private List<Rigidbody> _objectFragments;
    [SerializeField] private float _timeDestroyFragments = 30f;
    [SerializeField] private float _timeBetweenFragments = 0.2f;

    private Transform _pointForce;
    private float _force = 3f;

    private Coroutine _coroutine;
    private WaitForSeconds _destroyWait;
    private WaitForSeconds _betweenWait;

    public int CountObjectFragments => _objectFragments.Count;

    private void Awake()
    {
        _destroyWait = new WaitForSeconds(_timeDestroyFragments);
        _betweenWait = new WaitForSeconds(_timeBetweenFragments);

        foreach (var rigidbody in _objectFragments)
        {
            rigidbody.isKinematic = true;
        }
    }

    public void SetPointForce(Transform pointForce)
    {
        _pointForce = pointForce;
    }

    public void DropFragment(int count, bool isDead)
    {
        for (int i = 0; i < count; i++)
        {
            for (int j = 0; j < _objectFragments.Count; j++)
            {
                if (_objectFragments[j].isKinematic)
                {
                    ApplyForceToFragment(_objectFragments[j], _force);
                    break;
                }
            }
        }

        if (isDead)
            StartCoroutine();
    }

    private void ApplyForceToFragment(Rigidbody fragmentRigidbody, float forceMagnitude)
    {
        if (fragmentRigidbody != null)
        {
            fragmentRigidbody.isKinematic = false;

            Vector3 forceDirection = (_pointForce.position - fragmentRigidbody.position).normalized;
            fragmentRigidbody.AddForce(-forceDirection * forceMagnitude, ForceMode.Impulse);
        }
    }

    private void StartCoroutine()
    {
        if(_coroutine != null)
        {
            StopCoroutine(_coroutine);
            _coroutine = null;
        }

        _coroutine = StartCoroutine(DestroyFragments());
    }

    private IEnumerator DestroyFragments()
    {
        yield return _destroyWait;

        for (int i = 0; i < _objectFragments.Count; i++)
        {
            Destroy(_objectFragments[i].gameObject);
            yield return _betweenWait;
        }
    }
}
