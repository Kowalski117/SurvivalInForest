using System.Collections;
using UnityEngine;

public abstract class FoodEffect : MonoBehaviour
{
    private Coroutine _coroutine;

    public void StartEffect( float duration)
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
            _coroutine = null;
        }

        _coroutine = StartCoroutine(RotateColors(duration));
    }

    protected abstract IEnumerator RotateColors(float duration);
}
