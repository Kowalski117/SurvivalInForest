using System;
using UnityEngine;

public class MovementPath : MonoBehaviour
{
    [SerializeField] private PathType _pathType;
    [SerializeField] private int _direction = 1;
    [SerializeField] private int _movingTo = 0;
    [SerializeField] private Transform[] _pathElements;

    private bool _isActive = false;

    public Action OnStarted;
    public bool IsActive => _isActive;

    public void OnDrawGizmos()
    {
        if (_pathElements == null || _pathElements.Length < 2)
            return;

        for (int i = 1; i < _pathElements.Length; i++)
        {
            Gizmos.DrawLine(_pathElements[i - 1].position, _pathElements[i].position);
        }

        if (_pathType == PathType.Loop)
            Gizmos.DrawLine(_pathElements[0].position, _pathElements[_pathElements.Length - 1].position);
    }

    public Transform GetNextPathPoint()
    {
        if (_pathElements == null || _pathElements.Length < 1)
            return null;

        if (_movingTo < 0 || _movingTo >= _pathElements.Length)
        {
            return null;
        }

        var nextPoint = _pathElements[_movingTo];

        if (_pathElements.Length == 1)
            return nextPoint;

        if (_pathType == PathType.Linear)
        {
            if (_movingTo <= 0)
                _direction = 1;
            else if (_movingTo >= _pathElements.Length - 1)
                _isActive = false;
        }

        _movingTo += _direction;

        if (_pathType == PathType.Loop)
        {
            if (_movingTo >= _pathElements.Length)
                _movingTo = 0;
            else if (_movingTo < 0)
                _movingTo = _pathElements.Length - 1;
        }

        return nextPoint;
    }

    public void SetEnable(bool active)
    {
        _isActive = active;

        if (_isActive)
            OnStarted?.Invoke();
    }
}

public enum PathType
{
    Linear,
    Loop
}
