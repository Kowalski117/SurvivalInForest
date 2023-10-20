using UnityEngine;

public class BrokenPackage : MonoBehaviour
{
    private BrokenObject _brokenObject;
    private Collider _collider;
    private Outline _outline;

    private void Awake()
    {
        _brokenObject = GetComponentInParent<BrokenObject>();
        _collider = GetComponent<Collider>();
        _outline = GetComponent<Outline>();
        TurnOffPackage();
    }

    private void OnEnable()
    {
        _brokenObject.OnDied += TurnOnPackage;
    }

    private void OnDisable()
    {
        _brokenObject.OnDied -= TurnOnPackage;
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
