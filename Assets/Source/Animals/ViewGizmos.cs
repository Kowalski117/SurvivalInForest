using UnityEngine;

[RequireComponent(typeof(AnimalsMovement))]
public class ViewGizmos : MonoBehaviour
{
    [SerializeField] private AnimalsMovement _animalsMovement;
    [SerializeField] private bool ShowGizmos;
    [SerializeField] private Color _colorFleeRadius = Color.green;
    [SerializeField] private Color _colorSpawnPoint = Color.blue;

    private void Start()
    {
        _animalsMovement = GetComponent<AnimalsMovement>();
    }

    private void OnDrawGizmos()
    {
        if (ShowGizmos)
        {
            Gizmos.color = _colorFleeRadius;
            Gizmos.DrawWireSphere(transform.position, _animalsMovement.FleeRadius);
            Gizmos.color = _colorSpawnPoint;
            Gizmos.DrawSphere(_animalsMovement.SpawnPoint, 1);
            Gizmos.color = _colorSpawnPoint;
            Gizmos.DrawWireSphere(_animalsMovement.SpawnPoint, _animalsMovement.SpawnPointRadius);
        }
    }
}