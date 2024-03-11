using UnityEngine;

[CreateAssetMenu(menuName = "Tutorial/Info Object Data", order = 51)]
public class TutorialDataObject : ScriptableObject
{
    [SerializeField] private bool _isDone;
    [SerializeField] private TutorialDetail[] _details;

    public bool IsDone => _isDone;
    public TutorialDetail[] Details => _details;

    public void Init()
    {
        _isDone = PlayerPrefs.GetInt(SaveLoadConstants.IsDone + _details[0].Name, 0) == 1;
        PlayerPrefs.SetInt(SaveLoadConstants.IsDone + _details[0].Name, _isDone ? 1 : 0);
    }

    public void SetIsDone(bool isDone)
    {
        _isDone = isDone;
        PlayerPrefs.SetInt(SaveLoadConstants.IsDone + _details[0].Name, isDone ? 1 : 0);
    }

    public void Delete()
    {
        if (PlayerPrefs.HasKey(SaveLoadConstants.IsDone + _details[0].Name))
            PlayerPrefs.DeleteKey(SaveLoadConstants.IsDone + _details[0].Name);
    }
}

[System.Serializable]
public struct TutorialDetail 
{
    [SerializeField] private string _name;
    [TextArea(4, 4)]
    [SerializeField] private string _description;
    [SerializeField] private Sprite _sprite;

    public string Name => _name;
    public string Description => _description;
    public Sprite Sprite => _sprite;
}
