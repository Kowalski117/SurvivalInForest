using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Resource : MonoBehaviour, IDamagable
{
    [SerializeField] private int _health;
    [SerializeField] private ToolType _extractionType;
    [SerializeField] private GameObject _resourceObject;
    [SerializeField] private ItemPickUp[] _additionalResources;

    private BoxCollider _collider;

    public int Health => _health;
    public ToolType ExtractionType => _extractionType;

    private void Awake()
    {
        _collider = GetComponent<BoxCollider>();
    }

    private void Start()
    {
        SetActiveResources(false);
    }

    public void TakeDamage(float damage, float overTimeDamage)
    {
        _health -= (int)damage;

        if (_health <= 0)
        {
            _health = 0;
            Die();
        }
    }

    public void Die()
    {
        _resourceObject.SetActive(false);
        SetActiveResources(true);
        _collider.enabled = false;
        enabled = false;
    }

    private void SetActiveResources(bool isActive)
    {
        foreach (var resource in _additionalResources)
        {
            resource.gameObject.SetActive(isActive);
        }
    }
}
