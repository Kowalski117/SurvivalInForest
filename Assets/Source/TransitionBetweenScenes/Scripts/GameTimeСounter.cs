using UnityEngine;

public class GameTimeСounter : MonoBehaviour
{
    private float _timeCounter;

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

    private void Update()
    {
        _timeCounter += Time.deltaTime;
    }

    private void Save()
    {
        PlayerPrefs.SetFloat(SaveLoadConstants.GameTimeCounter, _timeCounter);
        PlayerPrefs.Save();
    }

    private void Load()
    {
        _timeCounter = PlayerPrefs.GetFloat(SaveLoadConstants.GameTimeCounter);
    }
}
