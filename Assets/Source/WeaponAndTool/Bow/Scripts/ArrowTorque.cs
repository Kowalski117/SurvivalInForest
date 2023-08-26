using UnityEngine;

public class ArrowTorque : MonoBehaviour
{
    [SerializeField] private float _velocityValue = 1;
    [SerializeField] private float _angularVelocity = 0.05f;

    private Rigidbody _rigidbody;
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Vector3 cross = Vector3.Cross(transform.forward, _rigidbody.velocity.normalized);

        _rigidbody.AddTorque(cross * _rigidbody.velocity.magnitude * _velocityValue);
        _rigidbody.AddTorque((-_rigidbody.angularVelocity + Vector3.Project(_rigidbody.angularVelocity, transform.forward) * _rigidbody.velocity.magnitude * _angularVelocity));
    }
}
