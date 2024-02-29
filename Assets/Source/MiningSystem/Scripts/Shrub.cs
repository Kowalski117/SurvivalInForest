using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(UniqueID))]
public class Shrub : MonoBehaviour
{
    private const float RadiusSpawn = 0.5f;
    private const float SpawnPointUp = 0.5f;

    [SerializeField] private ItemPickUp _item;
    [SerializeField] private int _countItems;
    [SerializeField] private float _timeRespawnItem;

    private UniqueID _uniqueID;
    private float _elapsedTime;
    private bool _isReborn = false;
    private int _currentCountItem = 0;

    private void Awake()
    {
        _uniqueID = GetComponent<UniqueID>();
    }

    private void Update()
    {
        if (_isReborn)
            _elapsedTime += Time.deltaTime;
    }

    private void OnEnable()
    {
        SavingGame.OnGameSaved += Save;
        SavingGame.OnGameLoaded += Load;
    }

    private void OnDisable()
    {
        List <ItemPickUp> listChildren = new List<ItemPickUp>(GetComponentsInChildren<ItemPickUp>().ToList());
        
        for (int i = 0; i < listChildren.Count; i++)
        {
            listChildren[i].DestroyItem -= DestroyItem;
        }

        SavingGame.OnGameSaved -= Save;
        SavingGame.OnGameLoaded -= Load;
    }

    private void SpawnItem(int count)
    {
        ItemPickUp itemPickUp;

        for (int i = 0; i < count; i++)
        {
            Vector3 position = (transform.position + Random.insideUnitSphere * RadiusSpawn);
            itemPickUp = SpawnLoots.Spawn(_item,position,transform,true,SpawnPointUp,true);
            itemPickUp.DestroyItem += DestroyItem;
        }
    }

    private void DestroyItem()
    {
        _currentCountItem++;
        
        if (_countItems == _currentCountItem)
        {
            _currentCountItem = 0;
            StartCoroutine(Wait());
        }
    }

    IEnumerator Wait()
    {
        _isReborn = true;
        yield return new WaitForSeconds(_timeRespawnItem - _elapsedTime);

        _elapsedTime = 0;
       SpawnItem(_countItems);
        _isReborn = false;
    }

    private void Save()
    {
        if (_isReborn)
        {
            PlayerPrefs.SetFloat(_uniqueID.Id + SaveLoadConstants.ResourceRevivalTime, PlayerPrefs.GetFloat(SaveLoadConstants.GameTimeCounter) + _elapsedTime);
            PlayerPrefs.Save();
        }
    }

    private void Load()
    {
        if (PlayerPrefs.HasKey(_uniqueID.Id + SaveLoadConstants.ResourceRevivalTime))
        {
            float savedTime = PlayerPrefs.GetFloat(_uniqueID.Id + SaveLoadConstants.ResourceRevivalTime);
            float gameTime = PlayerPrefs.GetFloat(SaveLoadConstants.GameTimeCounter);

            if (savedTime <= gameTime)
                SpawnItem(_countItems);
            else
            {
                _elapsedTime = savedTime - gameTime;
                StartCoroutine(Wait());
            }
        }
        else
            SpawnItem(_countItems);
    }
}
