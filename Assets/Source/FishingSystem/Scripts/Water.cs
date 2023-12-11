using UnityEngine;

public class Water : MonoBehaviour 
{
    [SerializeField] private Material _lowMaterial;
    [SerializeField] private Material _highMaterial;

    private MeshRenderer _renderer;

    private void Awake()
    {
        _renderer = GetComponent<MeshRenderer>();
    }

    public void ToggleLowMaterial(bool value)
    {
        ToglleMaterial(value, _lowMaterial);
    }

    public void ToggleHighMaterial(bool value)
    {
        ToglleMaterial(value, _highMaterial);
    }

    private void ToglleMaterial(bool value, Material material)
    {
        if (value)
            _renderer.material = material;
    }
}
