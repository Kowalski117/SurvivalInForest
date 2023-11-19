using UnityEngine;

public class PositionButton : MonoBehaviour
{
    [SerializeField] private Transform _position;

    private CreativeModeWindow _creativeModeWindow;

    private void Awake()
    {
        _creativeModeWindow = GetComponentInParent<CreativeModeWindow>();
    }

    public void SetPosiion()
    {
        _creativeModeWindow.StartTeleportPlayer(_position);
    }
}
