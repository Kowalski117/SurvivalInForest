using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Camera))]
public class LayerCullingDistances : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private float[] _cullingDistances = new float[32];

    private int _minDistances = 50;
    private int _maxDistances = 700;
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

    public void IncreaseDistance()
    {
        if (_cullingDistances[0] < _maxDistances)
        {
            for (int i = 0; i < _cullingDistances.Length; i++)
            {
                _cullingDistances[i] = _cullingDistances[i] * 2;
            }
            SetCullingDistances();
        }
    }

    public void DecreaseDistance()
    {
        if (_cullingDistances[0] > _minDistances)
        {
            for (int i = 0; i < _cullingDistances.Length; i++)
            {
                _cullingDistances[i] = _cullingDistances[i] / 2;
            }
            SetCullingDistances();
        }
    }
}