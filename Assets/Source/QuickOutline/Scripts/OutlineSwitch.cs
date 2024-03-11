using UnityEngine;

[RequireComponent(typeof(OutlineObject))]
public class OutlineSwitch : MonoBehaviour
{
    private OutlineObject _outline;
    private Building _building;

    private void Awake()
    {
        _building = GetComponentInParent<Building>();
        _outline = GetComponent<OutlineObject>();
        _outline.enabled = false;
    }

    private void OnEnable()
    {
        _building.OnCompletedBuild += Enable;
    }

    private void OnDisable()
    {
        _building.OnCompletedBuild -= Enable;
    }

    private void Enable()
    {
        _outline.enabled = true;
    }
}
