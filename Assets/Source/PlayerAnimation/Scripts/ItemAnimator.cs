using UnityEngine;

public class ItemAnimator : MonoBehaviour
{
    [SerializeField] private InventoryItemData _itemData;
    [SerializeField] private Animator _animatorHand;
    [SerializeField] private Transform _particleSpawnPoint;
    [SerializeField] private Transform[] _additionalSubjects;
    [SerializeField] private int _indexLayer;

    public InventoryItemData ItemData => _itemData;
    public Animator HandAnimator => _animatorHand;
    public Transform ParticleSpawnPoint => _particleSpawnPoint;
    public int IndexLayer => _indexLayer;

    public void ToggleLayer(bool isActive)
    {
        ToggleAnimator(isActive);
        ToggleItem(isActive);

        if (isActive) 
            _animatorHand.SetLayerWeight(_indexLayer, 1);
        else
            _animatorHand.SetLayerWeight(_indexLayer, 0);
    }

    public void ToggleAnimator(bool isActive)
    {
        _animatorHand.gameObject.SetActive(isActive);
    }

    public void ToggleItem(bool isActive)
    {
        if (_additionalSubjects.Length > 0) 
        {
            for (int i = 0; i < _additionalSubjects.Length; i++)
            {
                _additionalSubjects[i].gameObject.SetActive(isActive);
            }
        }

        transform.gameObject.SetActive(isActive);
    }
}
