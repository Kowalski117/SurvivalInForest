using UnityEngine;
using UnityEngine.Events;

public class DistanceHandler : MonoBehaviour
{
    private bool _isActive = false;
    private PlayerHealth _playerHealth;
    private SphereCollider _collider;
    private float _offsetRadiuse = 0.1f;

    public event UnityAction OnDistanceExceeded;

    private void Awake()
    {
        _collider = GetComponent<SphereCollider>();
    }

    private void Update()
    {
        if (_isActive && _playerHealth != null && Vector3.Distance(_playerHealth.transform.position, transform.position) > _collider.radius - _offsetRadiuse)
        {
            OnDistanceExceeded?.Invoke();
            _isActive = false;
        }
    }

    public void SetActive(bool isActive)
    {
        _isActive = isActive;
    }

    public void ToggleCollider(bool isActive)
    {
        _collider.enabled = isActive;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out PlayerHealth playerHealth) && _playerHealth == null)
        {
            _playerHealth = playerHealth;
        }
    }
}
