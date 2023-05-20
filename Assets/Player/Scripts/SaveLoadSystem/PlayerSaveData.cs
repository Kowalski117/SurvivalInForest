using SaveLoadSystem;
using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSaveData : MonoBehaviour
{
    [SerializeField] FirstPersonController _firstPersonController;

    private PlayerData _myData = new PlayerData();

    private void Update()
    {
        _myData.SetPlayerPosition(transform.position, transform.rotation);

        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            SaveGameHandler.CurrentSaveData.SetData(_myData);
            SaveGameHandler.SaveGame();
        }

        if (Keyboard.current.tKey.wasPressedThisFrame)
        {
            _firstPersonController.enabled = false;

            SaveGameHandler.LoadGame();
            _myData = SaveGameHandler.CurrentSaveData.PlayerData;
            transform.position = _myData.PlayerPosition;
            transform.rotation = _myData.PlayerRotation;

            Invoke(nameof(EnableController), 0.25f);
        }
    }

    private void EnableController()
    {
        _firstPersonController.enabled = true;
    }
}

[System.Serializable]
public struct PlayerData
{
    [SerializeField] private Vector3 _playerPosition;
    [SerializeField] private Quaternion _playerRotation;

    public Vector3 PlayerPosition => _playerPosition;
    public Quaternion PlayerRotation => _playerRotation;

    public void SetPlayerPosition(Vector3 playerPosition, Quaternion playerRotation)
    {
        _playerPosition = playerPosition;
        _playerRotation = playerRotation;
    }
}