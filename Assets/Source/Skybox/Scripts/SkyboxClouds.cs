using UnityEngine;

public class SkyboxClouds : MonoBehaviour
{
    [SerializeField] private float _speedRotation = 100;

    private void Update()
    {
        // Вращаем объект по оси Y
        transform.Rotate(0, _speedRotation * Time.deltaTime, 0);
    }
}