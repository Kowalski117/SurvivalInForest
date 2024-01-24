using UnityEngine;

[CreateAssetMenu(menuName = "Note", order = 51)]
public class NoteItemData : ScriptableObject
{
    [TextArea(4, 4)]
    [SerializeField] private string _letter;

    public string Letter => _letter;
}
