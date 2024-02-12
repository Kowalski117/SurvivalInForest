using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Compass : MonoBehaviour
{
    private const float _delay = 0.03f;

    [SerializeField] private RawImage _compassImage;
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private Image _markerPrefab;
    [SerializeField] private Transform _containerMarker;
    [SerializeField] private float _maxDistance = 50;
    [SerializeField] private float _minScale = 0.5f;
    [SerializeField] private QuestMaker[] _openQuestMakers;

    private List<QuestMaker> _questMakers = new List<QuestMaker>();

    private float _compassUnit;
    private float _rotationNumber = 360;
    private int _repeatingPicture = 3;
    private bool _isAcrive = true;
    private Coroutine _coroutineUpdateMarker;
    private WaitForSeconds _waitForSeconds = new WaitForSeconds(_delay);
    private QuestMaker _closestMarker = null;
    private float _closestDistance = float.MaxValue;

    public event UnityAction<QuestMaker> OnQuestMakerAdded;

    private void Awake()
    {
        _compassUnit = _compassImage.rectTransform.rect.width / _rotationNumber;

        foreach (var maker in _openQuestMakers)
        {
            AddQuestMarket(maker);
        }
    }

    private void Start()
    {
        StartCoroutine();
    }

    public void AddQuestMarket(QuestMaker questMaker)
    {
        Image newMarker = Instantiate(_markerPrefab, _containerMarker);
        _questMakers.Add(questMaker);
        questMaker.SetImage(newMarker);
        OnQuestMakerAdded?.Invoke(questMaker);
    }

    public void RemoveQuestMarket(QuestMaker questMaker)
    {
        if (_questMakers.Contains(questMaker))
        {
            _questMakers.Remove(questMaker);
            Destroy(questMaker.Image.gameObject);
        }
    }

    public Vector2 GetPosOnCompass(QuestMaker questMaker)
    {
        Vector2 playerPosition = new Vector2(_playerTransform.position.x, _playerTransform.position.z);
        Vector2 playerForward = new Vector2(_playerTransform.forward.x, _playerTransform.forward.z);

        float angle = Vector2.SignedAngle(questMaker.Position - playerPosition, playerForward);

        return new Vector2(_compassUnit * angle, 0f);
    }

    private void StartCoroutine()
    {
        if(_coroutineUpdateMarker != null)
        {
            StopCoroutine(_coroutineUpdateMarker);
            _coroutineUpdateMarker = null;
            _isAcrive = false;
        }

        _coroutineUpdateMarker = StartCoroutine(UpdateMarkerCompass());
        _isAcrive = true;
    }

    private IEnumerator UpdateMarkerCompass()
    {
        while (_isAcrive)
        {
            _compassImage.uvRect = new Rect(_playerTransform.localEulerAngles.y / _rotationNumber / _repeatingPicture, 0f, _compassImage.uvRect.width, 1f);

            _closestMarker = null;
            _closestDistance = float.MaxValue;

            foreach (var marker in _questMakers)
            {
                if (marker != null)
                {
                    marker.Image.rectTransform.anchoredPosition = GetPosOnCompass(marker);

                    float distance = Vector2.Distance(new Vector2(_playerTransform.position.x, _playerTransform.position.z), marker.Position);

                    float scale = 0;

                    if (distance < _maxDistance)
                        scale = 1f - (distance / _maxDistance);

                    if (distance < _closestDistance)
                    {
                        _closestDistance = distance;
                        _closestMarker = marker;
                    }

                    if (scale > _minScale)
                        marker.Image.rectTransform.localScale = Vector3.one * scale;
                    else
                        marker.Image.rectTransform.localScale = Vector3.one * _minScale;
                }
            }

            if (_closestMarker != null)
            {
                _closestMarker.Image.transform.SetAsLastSibling();
            }

            yield return _waitForSeconds;
        }
    }
}
