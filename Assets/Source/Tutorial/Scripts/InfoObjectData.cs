using UnityEngine;

[CreateAssetMenu(menuName = "Tutorial/Info Object Data", order = 51)]
public class InfoObjectData : ScriptableObject
{
    [SerializeField] private string _name;
    [TextArea(4, 4)]
    [SerializeField] private string _description;
    [SerializeField] private Sprite _sprite;
    [SerializeField] private bool _isDone;

    public string Name => _name;
    public string Description => _description;
    public Sprite Sprite => _sprite;
    public bool IsDone => _isDone;

    public void Init()
    {
        _isDone = PlayerPrefs.GetInt("IsDone_" + _name, 0) == 1;
        PlayerPrefs.SetInt("IsDone_" + _name, _isDone ? 1 : 0);
    }

    public void SetIsDone(bool isDone)
    {
        _isDone = isDone;
        PlayerPrefs.SetInt("IsDone_" + _name, isDone ? 1 : 0);
    }
}