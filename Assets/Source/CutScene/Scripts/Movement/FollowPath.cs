using UnityEngine;

public class FollowPath : MonoBehaviour
{
    [SerializeField] private MovementType _type = MovementType.Moving;
    [SerializeField] private MovementPath _path;
    [SerializeField] private float _speed = 1f;
    [SerializeField] private float _speedRotate = 0.1f;
    [SerializeField] private float _maxDistance = 1f;
    [SerializeField] private Transform _lookAt;
    
    private Transform _targetPoint;
    private bool _isActive = false;

    private void OnEnable()
    {
        _path.OnStarted += StartMovement;
    }

    private void OnDisable()
    {
        _path.OnStarted -= StartMovement;
    }

    private void Update()
    {
        if (_targetPoint != null && _isActive)
        {
            if (_type == MovementType.Moving)
                MoveToPoint();
            else if (_type == MovementType.Lerping)
                LerpToPoint();
        }
    }

    private void MoveToPoint()
    {
        transform.position = Vector3.MoveTowards(transform.position, _targetPoint.position, _speed * Time.deltaTime);
        RotateTowardsPoint();
        CheckDistance();
    }

    private void LerpToPoint()
    {
        transform.position = Vector3.Lerp(transform.position, _targetPoint.position, _speed * Time.deltaTime);
        RotateTowardsPoint();
        CheckDistance();
    }

    private void RotateTowardsPoint()
    {
        //var targetDirection = _targetPoint.position - transform.position;
        //var newDirection = Vector3.RotateTowards(transform.forward, targetDirection, _speedRotate * Time.deltaTime, 0.0f);
        if (_lookAt != null)
            transform.LookAt(_lookAt.transform);
    }

    private void CheckDistance()
    {
        var distanceSquare = (transform.position - _targetPoint.position).sqrMagnitude;

        if (distanceSquare < _maxDistance * _maxDistance)
        {
            _targetPoint = _path.GetNextPathPoint();
        }
    }

    private void StartMovement()
    {
        if (_path)
        {
            _targetPoint = _path.GetNextPathPoint();
            if (_targetPoint == null)
                return;

            transform.position = _targetPoint.position;
        }
    }

    public void SetEnable(bool active)
    {
        _isActive = active;
    }
}

public enum MovementType
{
    Moving,
    Lerping
}
