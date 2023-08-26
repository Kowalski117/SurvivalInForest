using UnityEditor.PackageManager;
using UnityEngine;

public class Bow : MonoBehaviour
{
    [SerializeField] private Transform _shootPoint;
    [SerializeField] private Arrow _currentArrow;
    [SerializeField] private PlayerInputHandler _inputHandler;

    [SerializeField] private WeaponItemData _currentWeapon;

    private bool _isShoot = false;
    [SerializeField] private float _arrowSpeed = 10f;

    private void Start()
    {
        _currentArrow.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        _inputHandler.InteractionPlayerInput.OnThrowFishingRod += Shoot;
        _currentArrow.OnEnteredCollider += InstantiateArrow;
    }

    private void OnDisable()
    {
        _inputHandler.InteractionPlayerInput.OnThrowFishingRod += Shoot;
        _currentArrow.OnEnteredCollider -= InstantiateArrow;
    }

    public void Shoot()
    {
        if (!_isShoot)
        {
            _currentArrow.gameObject.SetActive(true);
            _currentArrow.Shoot(_arrowSpeed);
            _isShoot = true;
        }
        else
        {
            _currentArrow.SetRope(transform);
            _currentArrow.gameObject.SetActive(false);
            _isShoot = false;
        }
    }

    public void InstantiateArrow(Vector3 position, Quaternion rotation, GameObject parent)
    {
        ItemPickUp arrow = Instantiate(_currentWeapon.Bullet, position, rotation, parent.transform);
        arrow.Rigidbody.isKinematic = true;
        arrow.GenerateNewID();

        _currentArrow.SetRope(transform);
        _currentArrow.gameObject.SetActive(false);
        _isShoot = false;
    }
}
