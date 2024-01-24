using UnityEngine;

public class Note : MonoBehaviour
{
    [SerializeField] private NoteScreen _noteScreen;
    [SerializeField] private NoteItemData _noteItemData;

    public void Init()
    {
        _noteScreen.UpdateLetterText(_noteItemData.Letter);
    }
}
