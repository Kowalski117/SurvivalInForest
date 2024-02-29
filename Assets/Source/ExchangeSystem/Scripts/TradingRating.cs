using System;
using UnityEngine;

public class TradingRating : MonoBehaviour
{
    [SerializeField] private int _maxQuestsCount;
    [SerializeField] private int _currentQuestsCount = 0;
    [SerializeField, Range(1, 3)] private int _currentRating = 0;

    private int _maxRating = 3;
    private int _minRating = 1;

    public event Action<int> OnRatingChanged;

    public int CurrentRating => _currentRating;

    private void OnEnable()
    {
        SavingGame.OnGameSaved += Save;
        SavingGame.OnGameLoaded += Load;
    }

    private void OnDisable()
    {
        SavingGame.OnGameSaved -= Save;
        SavingGame.OnGameLoaded -= Load;
    }

    private void AddQuestsCount()
    {
        SerRating(_currentRating++);
    }

    private void UpdateRating()
    {
        float rawRating = (_currentQuestsCount / _maxQuestsCount) * (_maxRating - 1) + 1;
        float roundedRating = Mathf.Floor(rawRating + 0.67f);
        _currentRating = (int)roundedRating;
        _currentRating = Mathf.RoundToInt(rawRating);
    }

    private void SerRating(int rating)
    {
        _currentRating = rating;
        //UpdateRating();
        OnRatingChanged?.Invoke(_currentQuestsCount);
    }

    private void Save()
    {
        ES3.Save(SaveLoadConstants.TradingRating, _currentRating);
    }

    private void Load()
    {
        SerRating(ES3.Load<int>(SaveLoadConstants.TradingRating, _minRating));
    }
}

