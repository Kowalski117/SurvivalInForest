using UnityEngine;

public class OutlineSelection : MonoBehaviour
{
    [SerializeField] private Transform _rayOrigin;
    [SerializeField] private float _rayDistance;
    [SerializeField] private float _outlineWidth;
    [SerializeField] private Color _outlineColor;
    [SerializeField] private Outline.Mode _outlineMode = Outline.Mode.OutlineVisible;
    [SerializeField] private LayerMask _outlineLayerMask;

    private Camera _camera;
    private Outline _previousOutline;

    private void Awake()
    {
        _camera = Camera.main;
    }

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

    private bool IsRayHittingSomething(LayerMask layerMask, out RaycastHit hitInfo)
    {
        var ray = new Ray(_rayOrigin.position, _camera.transform.forward * _rayDistance);
        Debug.DrawRay(ray.origin, ray.direction * _rayDistance, Color.red);
        return Physics.Raycast(ray, out hitInfo, _rayDistance, layerMask);
    }
}
