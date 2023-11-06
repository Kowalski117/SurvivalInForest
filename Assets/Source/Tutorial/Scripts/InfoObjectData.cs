using UnityEngine;

[CreateAssetMenu(menuName = "Tutorial/Info Object Data", order = 51)]
public class InfoObjectData : ScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField] private string _description;
    [SerializeField] private Sprite _sprite;
    [SerializeField] private bool _isDone;

    public string Name => _name;
    public string Description => _description;
    public Sprite Sprite => _sprite;
    public bool IsDone => _isDone;

    public void SetIsDone(bool isDone)
    {
        _isDone = isDone;
    }
}