using UnityEditor.Animations;
using UnityEngine;

public class ItemAnimator : MonoBehaviour
{
    [SerializeField] private InventoryItemData _itemData;
    [SerializeField] private Animator _animatorHand;
    [SerializeField] private AnimatorController _animatorControllerHand;
    [SerializeField] private Transform _particleSpawnPoint;

    public InventoryItemData ItemData => _itemData;
    public Animator HandAnimator => _animatorHand;
    public Transform ParticleSpawnPoint => _particleSpawnPoint;

    public void ToggleAnimator(bool isActive)
    {
        ToggleItem(isActive);
        _animatorHand.gameObject.SetActive(isActive);

        if(isActive)
            _animatorHand.runtimeAnimatorController = _animatorControllerHand;
    }

    public void ToggleItem(bool isActive)
    {
        transform.gameObject.SetActive(isActive);
    }
}
