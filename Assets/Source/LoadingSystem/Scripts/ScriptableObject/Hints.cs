using UnityEngine;

[CreateAssetMenu(menuName = "Hints", order = 51)]
public class Hints : ScriptableObject
{
    [SerializeField] private Sprite[] _sprites;
    [SerializeField] private string[] _texts;

    public Sprite[] SpriteHints => _sprites;
    public string[] TextHints => _texts;
}
