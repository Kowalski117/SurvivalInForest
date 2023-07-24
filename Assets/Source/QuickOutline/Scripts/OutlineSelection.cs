using UnityEngine;

public class OutlineSelection : Raycast
{
    [SerializeField] private float _outlineWidth;
    [SerializeField] private Color _outlineColor;
    [SerializeField] private Outline.Mode _outlineMode = Outline.Mode.OutlineVisible;
    [SerializeField] private LayerMask _outlineLayerMask;
    [SerializeField] private bool _isTrigger = true;
    private Outline _previousOutline;

    void Update()
    {
        if (!_isTrigger)
        {
            if (IsRayHittingSomething(_outlineLayerMask, out RaycastHit hitInfo))
            {
                if (hitInfo.collider.TryGetComponent(out Outline outline))
                {
                    if (_previousOutline != null)
                    {
                        _previousOutline.OutlineWidth = 0f;
                    }

                    outline.OutlineWidth = _outlineWidth;
                    outline.OutlineMode = _outlineMode;
                    outline.OutlineColor = _outlineColor;

                    _previousOutline = outline;
                }
            }
            else
            {
                if (_previousOutline != null)
                {
                    _previousOutline.OutlineWidth = 0f;
                    _previousOutline = null;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_isTrigger)
        {
            if (other.TryGetComponent(out Outline outline))
            {
                outline.OutlineWidth = _outlineWidth;
                outline.OutlineMode = _outlineMode;
                outline.OutlineColor = _outlineColor;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_isTrigger)
        {
            if (other.TryGetComponent(out Outline outline))
            {
                outline.OutlineWidth = 0;
            }
        }
    }
}
