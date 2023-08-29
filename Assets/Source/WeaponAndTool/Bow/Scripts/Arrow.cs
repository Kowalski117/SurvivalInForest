using UnityEngine;
using UnityEngine.Events;

public class Arrow : MonoBehaviour
{
    [SerializeField] private TrailRenderer _trailRenderer;

    private Rigidbody _rigidbody;
    private bool _isFlying = false;

    public event UnityAction<Arrow,Vector3, Quaternion, GameObject> OnEnteredCollider;

    public bool IsFlying => _isFlying;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void SetRope(Transform ropePoint)
    {
        transform.parent = ropePoint;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        _trailRenderer.enabled = false;
        _rigidbody.isKinematic = true;
    }

    public void Shoot(float velocity)
    {
        transform.parent = null;
        _rigidbody.isKinematic = false;
        _rigidbody.velocity = transform.forward * velocity;
        _trailRenderer.enabled = true;
        _trailRenderer.Clear();
        _isFlying = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider != null)
        {
            _rigidbody.isKinematic = true;
            _isFlying = false;
            OnEnteredCollider?.Invoke(this, transform.position, transform.rotation, collision.gameObject);
        }
    }
}
