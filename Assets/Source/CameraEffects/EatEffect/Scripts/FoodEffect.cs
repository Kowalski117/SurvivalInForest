using System.Collections;
using UnityEngine;

public abstract class FoodEffect : MonoBehaviour
{
    private Coroutine _coroutine;

    public void StartRotate( float duration)
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
            _coroutine = null;
        }

        _coroutine = StartCoroutine(Rotate(duration));
    }

    protected abstract IEnumerator Rotate(float duration);
}
