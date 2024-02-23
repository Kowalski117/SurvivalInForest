using UnityEngine;

public class AudioTriggerHandler : MonoBehaviour
{
    [SerializeField] private MainClock _timeHandler;
    [SerializeField] private AudioClip[] _dayClips;
    [SerializeField] private AudioClip[] _nightClips;

    private Vector2 _day = new Vector2(4, 19);
    private int _currentIndexDayClips;
    private int _previousIndexDayClips;
    private int _currentIndexNightClips;
    private int _previousIndexNightClips;

    public AudioClip GetAudioClip()
    {
        if (_timeHandler.CurrentHurts > _day.x && _timeHandler.CurrentHurts < _day.y)
        {
            do
            {
                _previousIndexDayClips = _currentIndexDayClips;
                _currentIndexDayClips = Random.Range(0, _dayClips.Length);
            }
            while (_currentIndexDayClips == _previousIndexDayClips);

            return _dayClips[_currentIndexDayClips];
        }
        else
        {
            do
            {
                _previousIndexNightClips = _currentIndexNightClips;
                _currentIndexNightClips = Random.Range(0, _nightClips.Length);
            }
            while (_currentIndexNightClips == _previousIndexNightClips);

            return _nightClips[_currentIndexNightClips];
        }
    }
}
