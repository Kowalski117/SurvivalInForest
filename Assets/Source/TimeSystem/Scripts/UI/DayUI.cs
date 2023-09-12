using TMPro;
using UnityEngine;

public class DayUI : MonoBehaviour
{
    [SerializeField] private TimeHandler _timeHandler;
    [SerializeField] private TMP_Text _currentDayText;

    private void OnEnable()
    {
        _timeHandler.OnDayUpdate += UpdateDay;
    }

    private void OnDisable()
    {
        _timeHandler.OnDayUpdate -= UpdateDay;
    }

    private void UpdateDay(int day)
    {
        _currentDayText.text = $"Прожито {day.ToString()} дней.";
    }
}
