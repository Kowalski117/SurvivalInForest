using UnityEngine;

public class InteractionPoint : MonoBehaviour
{
    [SerializeField] private PlayerInventoryHolder _inventoryHolder;

    public PlayerInventoryHolder InventoryHolder => _inventoryHolder;
}
