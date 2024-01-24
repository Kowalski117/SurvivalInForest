using UnityEngine;

public class InfoObject : MonoBehaviour
{
    [SerializeField] private InfoObjectData _infoObjectSO;

    private bool _isActive = true;

    public InfoObjectData InfoObjectSO => _infoObjectSO;
    public bool IsActive => _isActive;

    public void SetActive(bool isActive)
    {
        _isActive = isActive;
    }
}
