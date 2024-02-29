using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasScaler))]
public class UIScaler : MonoBehaviour
{
    [SerializeField] private float _minMatch = 0.4f;
    [SerializeField] private float _maxMatch = 1f;
    [SerializeField] private float _aspectRatioThreshold = 16f / 9f;

    private CanvasScaler _canvasScaler;
    private Vector2 _fullHD = new Vector2(1920, 1080);

    private void Awake()
    {
        _canvasScaler = GetComponent<CanvasScaler>();
    }

    private void Start()
    {
        _canvasScaler.referenceResolution = _fullHD;
        _canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
    }

    private void Update()
    {
        UpdateCanvasScaler();
    }

    private void UpdateCanvasScaler()
    {
        float currentAspectRatio = (float)Screen.width / Screen.height;

        if (currentAspectRatio >= _aspectRatioThreshold)
            _canvasScaler.matchWidthOrHeight = _maxMatch;
        else
            _canvasScaler.matchWidthOrHeight = _minMatch;
    }
}
