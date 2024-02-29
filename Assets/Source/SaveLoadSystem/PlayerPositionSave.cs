using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerPositionSave : MonoBehaviour
{
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private PlayerPosition _playerPositionLastScene;
    [SerializeField] private SleepHandler _sleepHandler;
    [SerializeField] private bool _isTransitionLastPosition;

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

    public void TeleportSpawnPoint()
    {
        _playerTransform.position = _sleepHandler.CurrentSpawnPoint.Position;
        _playerTransform.rotation = _sleepHandler.CurrentSpawnPoint.Rotation;
    }

    private void Save()
    {
        PlayerSaveData playerSaveData = new PlayerSaveData(transform.position, transform.rotation);

        ES3.Save(SaveLoadConstants.SceneIndex, SceneManager.GetActiveScene().buildIndex);
        ES3.Save(SaveLoadConstants.PlayerSaveData + SceneManager.GetActiveScene().buildIndex, playerSaveData);
    }

    private void Load()
    {
        if (ES3.KeyExists(SaveLoadConstants.LastSceneIndex))
        {
            int lastSceneIndex = ES3.Load<int>(SaveLoadConstants.LastSceneIndex);
            int nextSceneIndex = ES3.Load<int>(SaveLoadConstants.NextSceneIndex);

            if (ES3.KeyExists(SaveLoadConstants.PlayerSaveData + SceneManager.GetActiveScene().buildIndex) &&
                _isTransitionLastPosition || lastSceneIndex == 1)
            {
                PlayerSaveData playerSaveData = ES3.Load<PlayerSaveData>(SaveLoadConstants.PlayerSaveData + SceneManager.GetActiveScene().buildIndex);
                _playerTransform.position = playerSaveData.Position;
                _playerTransform.rotation = playerSaveData.Rotation;
                return;
            }
            else
            {
                foreach (var lastScene in _playerPositionLastScene.LastScenePosition)
                {
                    if (lastSceneIndex == lastScene.LastSceneIndex && nextSceneIndex == lastScene.NextSceneIndex)
                    {
                        _playerTransform.position = lastScene.Position;
                        _playerTransform.rotation = lastScene.Rotation;
                        return;
                    }
                }
            }
        }
    }

}

[System.Serializable]
public struct PlayerSaveData
{
    [SerializeField] private Vector3 _position;
    [SerializeField] private Quaternion _rotation;

    public Vector3 Position => _position;
    public Quaternion Rotation => _rotation;

    public PlayerSaveData(Vector3 position, Quaternion rotation)
    {
        _position = position;
        _rotation = rotation;
    }
}
