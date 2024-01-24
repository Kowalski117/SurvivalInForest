using UnityEngine;

public class FishingRodRenderer : MonoBehaviour
{
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private Transform _fishingRod;
    [SerializeField] private Transform _fishingRodReborn;
    [SerializeField] private Float _float;

    private bool _isDrawing = false;
    private bool _isDefoultPoint = true;
    private int _maxPositionCount = 2;

    private void LateUpdate()
    {
        if(_isDrawing)
        {
            if(_isDefoultPoint)
                _lineRenderer.SetPosition(0, _fishingRod.transform.position);
            else
                _lineRenderer.SetPosition(0, _fishingRodReborn.transform.position);

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

    public void ToggleActive(bool isActive)
    {
        _lineRenderer.enabled = isActive;
    }

    public void SetDefoultPoint(bool result)
    {
        _isDefoultPoint = result;
    }
}
