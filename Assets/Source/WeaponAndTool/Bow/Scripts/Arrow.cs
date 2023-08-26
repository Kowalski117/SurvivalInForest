using UnityEngine;
using UnityEngine.Events;

public class Arrow : MonoBehaviour
{
    [SerializeField] private TrailRenderer _trailRenderer;

    private Rigidbody _rigidbody;

    public event UnityAction<Vector3, Quaternion, GameObject> OnEnteredCollider;

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
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider != null)
        {
            _rigidbody.isKinematic = true;
            OnEnteredCollider?.Invoke(transform.position, transform.rotation, collision.gameObject);
        }
    }
}
