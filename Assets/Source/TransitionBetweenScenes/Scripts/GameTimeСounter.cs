using UnityEngine;

public class GameTimeСounter : MonoBehaviour
{
    private float _timeCounter;

    private void OnEnable()
    {
        SavingGame.OnGameSaved += Save;
        SavingGame.OnGameLoaded += Load;
        SavingGame.OnSaveDeleted += Delete;
    }

    private void OnDisable()
    {
        SavingGame.OnGameSaved -= Save;
        SavingGame.OnGameLoaded -= Load;
        SavingGame.OnSaveDeleted -= Delete;
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
        if(PlayerPrefs.HasKey(SaveLoadConstants.GameTimeCounter))
            _timeCounter = PlayerPrefs.GetFloat(SaveLoadConstants.GameTimeCounter);
    }

    private void Delete()
    {
        if (PlayerPrefs.HasKey(SaveLoadConstants.GameTimeCounter))
            PlayerPrefs.DeleteKey(SaveLoadConstants.GameTimeCounter);
    }
}
