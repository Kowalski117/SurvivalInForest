using TMPro;
using UnityEngine;

public class DayUI : MonoBehaviour
{
    [SerializeField] private MainClock _timeHandler;
    [SerializeField] private TMP_Text _currentDayText;

    private void OnEnable()
    {
        _timeHandler.OnDayUpdated += UpdateDay;
    }

    private void OnDisable()
    {
        _timeHandler.OnDayUpdated -= UpdateDay;
    }

    private void UpdateDay(int day)
    {
        _currentDayText.text = day.ToString();
    }
}
