using UnityEngine;

public class Raycast : MonoBehaviour
{
    [SerializeField] protected float RayDistance;
    [SerializeField] protected Transform RayOrigin;

    protected Camera CameraMain;

    protected virtual void Awake()
    {
        CameraMain = Camera.main;
    }

    protected bool IsRayHittingSomething(LayerMask layerMask, out RaycastHit hitInfo)
    {
        var ray = new Ray(RayOrigin.position, CameraMain.transform.forward * RayDistance);
        Debug.DrawRay(ray.origin, ray.direction * RayDistance, Color.red);
        if(layerMask != 0)
            return Physics.Raycast(ray, out hitInfo, RayDistance, layerMask);
        else
            return Physics.Raycast(ray, out hitInfo, RayDistance);
    }
}
