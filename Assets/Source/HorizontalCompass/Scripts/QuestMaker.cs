using UnityEngine;
using UnityEngine.UI;

public class QuestMaker : MonoBehaviour
{
    [SerializeField] private Sprite _icon;
    [SerializeField] private Image _image;
    
    private Vector2 _position;

    public Image Image => _image;
    public Vector2 Position => _position;

    private void Awake()
    {
        _position = new Vector2(transform.position.x, transform.position.z);
    }

    public void SetImage(Image image)
    {
        _image = image;
        _image.sprite = _icon;
    }
}
