using System;
using TMPro;
using UnityEngine;

public class ClockUI : MonoBehaviour
{
    [SerializeField] private MainClock _timeHandler;
    [SerializeField] private TMP_Text _currentTimeText;

    private void OnEnable()
    {
        _timeHandler.OnTimeUpdate += UpdateTime;
    }

    private void OnDisable()
    {
        _timeHandler.OnTimeUpdate -= UpdateTime;
    }

    private void UpdateTime(DateTime dateTime)
    {
        _currentTimeText.text = dateTime.ToString(GameConstants.HHmm);
    }
}
