using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenObject : MonoBehaviour
{
    [SerializeField] private List<Rigidbody> _objectFragments;
    [SerializeField] private float _timeDestroyFragments = 30f;
    [SerializeField] private float _timeBetweenFragments = 0.2f;

    public int CountObjectFragments => _objectFragments.Count;

    private void Awake()
    {
        foreach (var rigidbody in _objectFragments)
        {
            rigidbody.isKinematic = true;
        }
    }

    public void DropFragment(int count,bool isDead)
    {
        for (int i = 0; i < count; i++)
        {
            for (int j = 0; j < _objectFragments.Count; j++)
            {
                if (_objectFragments[j].isKinematic)
                {
                    _objectFragments[j].isKinematic = false;
                    break;
                }
            }
        }

        if (isDead)
        {
            StartCoroutine(DestroyFragments());
        }
    }

    IEnumerator DestroyFragments()
    {
        yield return new WaitForSeconds(_timeDestroyFragments);
        for (int i = 0; i < _objectFragments.Count; i++)
        {
            Destroy(_objectFragments[i].gameObject);
            yield return new WaitForSeconds(_timeBetweenFragments);
        }
    }
}
