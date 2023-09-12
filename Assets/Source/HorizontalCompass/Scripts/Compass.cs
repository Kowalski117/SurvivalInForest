using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Compass : MonoBehaviour
{
    [SerializeField] private RawImage _compassImage;
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private Image _markerPrefab;
    [SerializeField] private Transform _containerMarker;
    [SerializeField] private float _maxDistance = 50;
    [SerializeField] private QuestMaker _questMaker;

    private List<QuestMaker> _questMakers = new List<QuestMaker>();

    private float _compassUnit;
    private float _rotationNumber = 360;
    private int _repeatingPicture = 3;

    private void Awake()
    {
        _compassUnit = _compassImage.rectTransform.rect.width / _rotationNumber;
    }

    private void Start()
    {
        AddQuestMarket(_questMaker);
    }

    private void Update()
    {
        _compassImage.uvRect = new Rect(_playerTransform.localEulerAngles.y / _rotationNumber/_repeatingPicture, 0f, 0.5f, 1f);

        foreach (var marker in _questMakers)
        {
            marker.Image.rectTransform.anchoredPosition = GetPosOnCompass(marker);

            float distance = Vector2.Distance(new Vector2(_playerTransform.position.x, _playerTransform.position.z), marker.Position);
            float scale = 0f;

            if(distance < _maxDistance)
                scale = 1f - (distance / _maxDistance);

            marker.Image.rectTransform.localScale = Vector3.one * scale;
        }
    }

    public void AddQuestMarket(QuestMaker questMaker)
    {
        Image newMarker = Instantiate(_markerPrefab, _containerMarker);
        questMaker.SetImage(newMarker);
        _questMakers.Add(questMaker);
    }

    public Vector2 GetPosOnCompass(QuestMaker questMaker)
    {
        Vector2 playerPosition = new Vector2(_playerTransform.position.x, _playerTransform.position.z);
        Vector2 playerForward = new Vector2(_playerTransform.forward.x, _playerTransform.forward.z);

        float angle = Vector2.SignedAngle(questMaker.Position - playerPosition, playerForward);

        return new Vector2(_compassUnit * angle, 0f);
    }
}
