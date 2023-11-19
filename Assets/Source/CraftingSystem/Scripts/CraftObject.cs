using UnityEngine;

public class CraftObject : MonoBehaviour
{
    [SerializeField] private CraftingÑategory _craftingÑategory;
    [SerializeField] private bool _isEnabledInitially = false;

    private Building _building;
    private BoxCollider _boxCollider;
    private Fire _fire;
    private bool _isEnabled = false;

    public bool IsEnabledInitially => _isEnabledInitially;
    public CraftingÑategory CraftingÑategory => _craftingÑategory;
    public bool IsEnabled => _isEnabled;

    private void Awake()
    {
        _building = GetComponentInParent<Building>();
        _boxCollider = GetComponent<BoxCollider>();
        _fire = GetComponent<Fire>();

        if (_building != null)
        {
            _boxCollider.enabled = false;
        }

        if (_isEnabledInitially)
        {
            _boxCollider.enabled = true;
        }
    }

    private void OnEnable()
    {
        if(_building != null )
        {
            _building.OnCompletedBuild += EnableCollider;
        }

        if (_fire != null)
        {
            _fire.OnToggledFire += ToggleEnable;
        }
    }

    private void OnDisable()
    {
        if (_building != null)
        {
            _building.OnCompletedBuild -= EnableCollider;
        }

        if (_fire != null)
        {
            _fire.OnToggledFire -= ToggleEnable;
        }
    }

    public void TurnOff()
    {
        _isEnabled = false;
    }

    private void EnableCollider()
    {
        _isEnabled = true;
    }

    private void ToggleEnable(bool enable)
    {
        if (enable)
            EnableCollider();
        else
            TurnOff();
    }

    private void OnDestroy()
    {
        TurnOff();
    }
}
