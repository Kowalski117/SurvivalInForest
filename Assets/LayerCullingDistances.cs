using UnityEngine;

[RequireComponent(typeof(Camera))]
public class LayerCullingDistances : MonoBehaviour
{
    [SerializeField]private Camera _camera;
    [SerializeField]
    private float[] _cullingDistances = new float[32];
    

    private void Awake()
    {
        SetCullingDistances();
    }

    private void OnValidate()
    {
        SetCullingDistances();
    }

    private void SetCullingDistances()
    {
        _camera.layerCullDistances = _cullingDistances;
    }
}