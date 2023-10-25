using UnityEngine;

public class BrokenPackage : MonoBehaviour
{
    private Box _box;
    private Collider _collider;
    private Outline _outline;

    private void Awake()
    {
        _box = GetComponentInParent<Box>();
        _collider = GetComponent<Collider>();
        _outline = GetComponent<Outline>();
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
