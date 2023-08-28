using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ColorRotation : MonoBehaviour
{
    [SerializeField] private Image _targetImage;
    [SerializeField] private float _rotationSpeed = 30f;
    [SerializeField] private float _alphaValue = 0.3f;
    [SerializeField] private float _effectDuration = 5f;

    private float _startTime;

    private void Start()
    {
        _startTime = Time.time;
        StartCoroutine(RotateColors());
    }

    private IEnumerator RotateColors()
    {
        _targetImage.enabled = true;

        while (Time.time - _startTime < _effectDuration)
        {
            float hue = ((Time.time - _startTime) * _rotationSpeed) % 1f;
            Color newColor = Color.HSVToRGB(hue, 1f, 1f);
            newColor.a = _alphaValue;

            _targetImage.color = newColor;

            yield return null;
        }

        _targetImage.enabled = false;
    }
}
