using UnityEngine;

public class PositionButton : MonoBehaviour
{
    [SerializeField] private Vector3 position;

    private CreativeModeWindow _creativeModeWindow;

    private void Awake()
    {
        _creativeModeWindow = GetComponentInParent<CreativeModeWindow>();
    }

    public void SetPosiion()
    {
        _creativeModeWindow.PlayerInventoryHolder.gameObject.transform.position = position;
    }
}
