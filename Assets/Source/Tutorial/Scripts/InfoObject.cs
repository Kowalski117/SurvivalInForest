using UnityEngine;

public class InfoObject : MonoBehaviour
{
    [SerializeField] private InfoObjectData _infoObjectSO;

    public InfoObjectData InfoObjectSO => _infoObjectSO;
}
