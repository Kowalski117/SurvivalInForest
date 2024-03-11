using UnityEngine;

public class InfoObject : MonoBehaviour
{
    [SerializeField] private TutorialDataObject _infoObjectSO;

    private bool _isActive = true;

    public TutorialDataObject InfoObjectSO => _infoObjectSO;
    public bool IsActive => _isActive;

    public void SetActive(bool isActive)
    {
        _isActive = isActive;
    }
}
