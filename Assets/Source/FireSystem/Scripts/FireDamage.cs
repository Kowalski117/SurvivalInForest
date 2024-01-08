using UnityEngine;

public class FireDamage : MonoBehaviour
{
    private PlayerHealth _player;
    private float _damage = 5f;
    private float _maxTime = 2f;
    private float _timer;

    private void Update()
    {
        if (_player != null)
        {
            _timer += Time.deltaTime;

            if (_timer >= _maxTime)
            {
                _player.TakeDamage(_damage, 0);
                _timer = 0;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerHealth playerHealth))
            _player = playerHealth;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out PlayerHealth playerHealth))
            _player = null;
    }
}
