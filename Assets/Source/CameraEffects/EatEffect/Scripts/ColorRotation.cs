using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ColorRotation : FoodEffect
{
    [SerializeField] private Image _targetImage;
    [SerializeField] private float _rotationSpeed = 30f;
    [SerializeField] private float _alphaValue = 0.3f;

    private float _startTime;

    protected override IEnumerator Rotate(float duration)
    {
        _startTime = Time.time;
        _targetImage.enabled = true;

        while (Time.time - _startTime < duration)
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
