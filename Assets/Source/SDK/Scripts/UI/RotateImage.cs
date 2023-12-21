using UnityEngine;

public class RotateImage : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed = 50f;

    private void Update()
    {
        transform.Rotate(Vector3.forward, -_rotationSpeed * Time.deltaTime);
    }
}
