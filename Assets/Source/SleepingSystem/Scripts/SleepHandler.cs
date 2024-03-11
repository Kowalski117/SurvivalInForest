using UnityEngine;
using UnityEngine.SceneManagement;

public class SleepHandler : MonoBehaviour
{
    [SerializeField] private PlayerPosition _playerPositionLastScene;
    [SerializeField] private SleepPanel _sleepPanel;

    private SpawnPointSaveData _sleepPointSaveData;

    public SpawnPointSaveData CurrentSpawnPoint => _sleepPointSaveData;

    private  void Awake()
    {
        if (_sleepPointSaveData.Position == Vector3.zero)
        {
            foreach (var sleepPosition in _playerPositionLastScene.SleepPositions)
            {
                if (SceneManager.GetActiveScene().buildIndex == sleepPosition.SceneIndex)
                    _sleepPointSaveData = new SpawnPointSaveData(sleepPosition.Position, sleepPosition.Rotation);
            }
        }
    }

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

    public void SetPoint(Transform point)
    {
        _sleepPointSaveData = new SpawnPointSaveData(point.position, point.rotation);
        _sleepPanel.Toggle();
    }

    private void Save()
    {
        ES3.Save(SaveLoadConstants.SpawnPosition + SceneManager.GetActiveScene().buildIndex, _sleepPointSaveData);
    }

    private void Load()
    {
        if (ES3.KeyExists(SaveLoadConstants.SpawnPosition + SceneManager.GetActiveScene().buildIndex))
        {
            _sleepPointSaveData = ES3.Load<SpawnPointSaveData>(SaveLoadConstants.SpawnPosition + SceneManager.GetActiveScene().buildIndex);
            return;
        }
    }

    private void Delete()
    {
        if (ES3.KeyExists(SaveLoadConstants.SpawnPosition + SceneManager.GetActiveScene().buildIndex))
            ES3.DeleteKey(SaveLoadConstants.SpawnPosition + SceneManager.GetActiveScene().buildIndex);
    }
}

[System.Serializable]
public struct SpawnPointSaveData
{
    [SerializeField] private Vector3 _position;
    [SerializeField] private Quaternion _rotation;

    public Vector3 Position => _position;
    public Quaternion Rotation => _rotation;

    public SpawnPointSaveData(Vector3 position, Quaternion rotation)
    {
        _position = position;
        _rotation = rotation;
    }
}
