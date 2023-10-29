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

    public int CountObjectFragments => _objectFragments.Count;

    private void Awake()
    {
        foreach (var rigidbody in _objectFragments)
        {
            rigidbody.isKinematic = true;
        }
    }

    public void SetPointForce(Transform pointForce)
    {
        _pointForce = pointForce;
    }

    public void DropFragment(int count,bool isDead)
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
        {
            StartCoroutine(DestroyFragments());
        }
    }

    private void ApplyForceToFragment(Rigidbody fragmentRigidbody, float forceMagnitude)
    {
        if (fragmentRigidbody != null)
        {
            fragmentRigidbody.isKinematic = false;

            Vector3 forceDirection = (_pointForce.position - fragmentRigidbody.position).normalized;
            fragmentRigidbody.AddForce(forceDirection * forceMagnitude, ForceMode.Impulse);
        }
    }

    private IEnumerator DestroyFragments()
    {
        yield return new WaitForSeconds(_timeDestroyFragments);
        for (int i = 0; i < _objectFragments.Count; i++)
        {
            Destroy(_objectFragments[i].gameObject);
            yield return new WaitForSeconds(_timeBetweenFragments);
        }
    }
}
