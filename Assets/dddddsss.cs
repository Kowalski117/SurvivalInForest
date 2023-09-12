using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]

public class dddddsss : MonoBehaviour
{
    private void Awake()
    {
        Camera camera = GetComponent<Camera>();
        camera.depthTextureMode = DepthTextureMode.Depth;

#if UNITY_EDITOR
        // Force depth render in the editor.
        camera.forceIntoRenderTexture = true;
#endif
    }
}
