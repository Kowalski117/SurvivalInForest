using System;
using UnityEngine;

namespace SaveLoadSystem
{
    [System.Serializable]
    public class SaveData
    {
        [SerializeField] private PlayerData _playerData = new PlayerData();

        public PlayerData PlayerData => _playerData;

        public void SetData(PlayerData data)
        {
            _playerData = data;
        }
    }
}