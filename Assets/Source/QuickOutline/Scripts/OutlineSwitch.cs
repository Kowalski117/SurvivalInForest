using UnityEngine;

public class OutlineSwitch : MonoBehaviour
{
    private Outline _outline;
    private Building _building;

    private void Awake()
    {
        _building = GetComponentInParent<Building>();
        _outline = GetComponent<Outline>();
        _outline.enabled = false;
    }

    private void OnEnable()
    {
        _building.OnCompletedBuild += EnableOutline;
    }

    private void OnDisable()
    {
        _building.OnCompletedBuild -= EnableOutline;
    }

    private void EnableOutline()
    {
        _outline.enabled = true;
    }
}
