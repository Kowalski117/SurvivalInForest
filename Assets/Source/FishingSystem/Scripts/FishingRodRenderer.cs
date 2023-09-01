using UnityEngine;

public class FishingRodRenderer : MonoBehaviour
{
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private FishingRod _fishingRod;
    [SerializeField] private Float _float;

    private bool _isDrawing = false;
    private int _maxPositionCount = 2;

    private void LateUpdate()
    {
        if(_isDrawing)
        {
            _lineRenderer.SetPosition(0, _fishingRod.transform.position);
            _lineRenderer.SetPosition(1, _float.transform.position);
        }
    }

    public void DrawRope()
    {
        _isDrawing = true;
        _lineRenderer.positionCount = _maxPositionCount;
    }

    public void Disable()
    {
        _isDrawing = false;
        _lineRenderer.positionCount = 0;
    }
}
