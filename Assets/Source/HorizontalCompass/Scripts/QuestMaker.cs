using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(UniqueID))]
public class QuestMaker : MonoBehaviour
{
    [SerializeField] private Sprite _icon;

    private UniqueID _uniqueID;
    private Image _image;

    public Image Image => _image;
    public Vector2 Position => new Vector2(transform.position.x, transform.position.z);
    public string Id => _uniqueID.Id;

    private void Awake()
    {
        _uniqueID = GetComponent<UniqueID>();
    }

    public void SetImage(Image image)
    {
        _image = image;
        _image.sprite = _icon;
    }
}
