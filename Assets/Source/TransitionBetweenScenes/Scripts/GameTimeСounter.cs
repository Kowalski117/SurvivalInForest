using UnityEngine;

public class GameTimeСounter : MonoBehaviour
{
    private float _timeCounter;

    private string _gameTime = "GameTimeСounter";

    private void OnEnable()
    {
        SaveGame.OnSaveGame += Save;
        SaveGame.OnLoadData += Load;
    }

    private void OnDisable()
    {
        SaveGame.OnSaveGame -= Save;
        SaveGame.OnLoadData -= Load;
    }

    private void Update()
    {
        _timeCounter += Time.deltaTime;
    }

    private void Save()
    {
        PlayerPrefs.SetFloat(_gameTime, _timeCounter);
        PlayerPrefs.Save();
    }

    private void Load()
    {
        _timeCounter = PlayerPrefs.GetFloat(_gameTime);
    }
}
