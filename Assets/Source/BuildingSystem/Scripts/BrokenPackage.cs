using UnityEngine;

public class BrokenPackage : MonoBehaviour
{
    private Box _box;
    private Collider _collider;
    private Outline _outline;
    private DistanceHandler _distanceHandler;

    private void Awake()
    {
        _box = GetComponentInParent<Box>();
        _collider = GetComponent<Collider>();
        _outline = GetComponent<Outline>();
        _distanceHandler = GetComponentInChildren<DistanceHandler>();
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
        _distanceHandler.gameObject.SetActive(true);
    }

    private void TurnOffPackage()
    {
        _collider.enabled = false;
        _outline.enabled = false;
        _distanceHandler.gameObject.SetActive(false);
    }
}
