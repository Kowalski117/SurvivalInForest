using UnityEngine;

public class SkyboxClouds : MonoBehaviour
{
    [SerializeField] private float _speedRotation = 100;

    private void Update()
    {
        transform.Rotate(0, _speedRotation * Time.deltaTime, 0);
    }
}