using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class DownloadIcon : MonoBehaviour
{
    private Image _image;
    private float _duration;

    private Coroutine _coroutine;

    private void Awake()
    {
        _image = GetComponent<Image>();
        _image.fillAmount = 0;
    }

    public void StartFilling(float duration)
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }
        _duration = duration;
        _coroutine = StartCoroutine("Filling");
    }

    public IEnumerable Filling()
    {
        _image.fillAmount = 0;
        float elapsed = 0;
        float nextValue;
        while (elapsed < _duration)
        {
            nextValue = Mathf.Lerp(0, 1, elapsed / _duration);
            _image.fillAmount = nextValue;
            elapsed += Time.deltaTime;
            yield return null;
        }
        gameObject.SetActive(false);
    }
}
