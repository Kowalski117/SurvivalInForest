using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(OutlineObject))]
public class BrokenPackage : MonoBehaviour
{
    private Box _box;
    private Collider _collider;
    private OutlineObject _outline;

    private void Awake()
    {
        _box = GetComponentInParent<Box>();
        _collider = GetComponent<Collider>();
        _outline = GetComponent<OutlineObject>();
    }

    private void Start()
    {
        TurnOffPackage();
    }

    private void OnEnable()
    {
        _box.OnDied += TurnOnPackage;
    }

    private void OnDisable()
    {
        _box.OnDied -= TurnOnPackage;
    }

    private void TurnOnPackage()
    {
        _collider.enabled = true;
        _outline.enabled = true;
    }

    private void TurnOffPackage()
    {
        _collider.enabled = false;
        _outline.enabled = false;
    }
}
