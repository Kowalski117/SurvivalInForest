using UnityEngine;

[RequireComponent(typeof(SetMovement))]
public class ViewGizmos : MonoBehaviour
{
    [SerializeField] private SetMovement setMovement;
    [SerializeField] private bool ShowGizmos;
    [SerializeField] private Color _colorFleeRadius = Color.green;
    [SerializeField] private Color _colorSpawnPoint = Color.blue;

    private void Start()
    {
        setMovement = GetComponent<SetMovement>();
    }

    private void OnDrawGizmos()
    {
        if (ShowGizmos)
        {
            Gizmos.color = _colorFleeRadius;
            Gizmos.DrawWireSphere(transform.position, setMovement.FleeRadius);
            Gizmos.color = _colorSpawnPoint;
            Gizmos.DrawSphere(setMovement.SpawnPoint, 1);
            Gizmos.color = _colorSpawnPoint;
            Gizmos.DrawWireSphere(setMovement.SpawnPoint, setMovement.SpawnPointRadius);
        }
    }
}