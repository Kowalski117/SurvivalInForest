using UnityEngine;

public class OutlineSelection : Raycast
{
    [SerializeField] private float _outlineWidth;
    [SerializeField] private Color _outlineColor;
    [SerializeField] private Outline.Mode _outlineMode = Outline.Mode.OutlineVisible;
    [SerializeField] private LayerMask _outlineLayerMask;

    private Outline _previousOutline;

    void Update()
    {
        if(IsRayHittingSomething(_outlineLayerMask, out RaycastHit hitInfo))
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
