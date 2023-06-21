using UnityEngine;

public class BuildPreview : MonoBehaviour
{
    [SerializeField] private int _defoultLayerInt = 9;

    public BuildPreview()
    {
        gameObject.layer = _defoultLayerInt;
        name = "Build Preview";
    }
}
