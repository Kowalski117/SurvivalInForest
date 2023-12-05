using System.Collections;
using UnityEngine;
using DG.Tweening;
using Unity.Mathematics;
using PixelCrushers.QuestMachine;

[RequireComponent(typeof (UniqueID))]
public class SpawnStone: MonoBehaviour
{
    [SerializeField] private float _spawnTime;
    [SerializeField] private float _scaleTime;
    [SerializeField] private GameObject _remainder;
    [SerializeField] private BrokenStone _brokenStone;
    [SerializeField] private string _id;

    private Resource _resource;
    private Vector3 _resurseLocaleScale;
    private QuestControl _questControl;
    private bool _isSpawning = false;
    private float _elapsedTime = 0;
    private UniqueID _uniqueID;
    
    private void Awake()
    {
        _resource = gameObject.GetComponentInChildren<Resource>();
        _resurseLocaleScale = _resource.transform.localScale;
        _uniqueID = GetComponent<UniqueID>();
        _questControl = GetComponentInParent<QuestControl>();
    }
    
    private void Update()
    {
        if (_isSpawning)
            _elapsedTime += Time.deltaTime;
    }

    private void OnEnable()
    {
        _resource.Died += ResourceDeath;
        SaveGame.OnSaveGame += Save;
        SaveGame.OnLoadData += Load;
    }

    private void OnDisable()
    {
        _resource.Died -= ResourceDeath;
        SaveGame.OnSaveGame -= Save;
        SaveGame.OnLoadData -= Load;
    }

    private void ResourceDeath()
    {
        _questControl.SendToMessageSystem(MessageConstants.Broken + _id);
        _remainder.SetActive(true);
        Instantiate(_brokenStone, transform.position,quaternion.identity,this.transform);
        StartCoroutine(SpawnOverTime());
    }
    
    private void SpawnResourse()
    {
        _resource.gameObject.transform.localScale = Vector3.zero;
        _remainder.SetActive(false);
        _resource.gameObject.SetActive(true);
        _resource.transform.DOScale(_resurseLocaleScale, _scaleTime);
    }
    
    private IEnumerator SpawnOverTime()
    {
        yield return new WaitForSeconds(_spawnTime);
        SpawnResourse();
        yield return new WaitForSeconds(_scaleTime);
        PlayerPrefs.DeleteKey(_uniqueID.Id);
    }
    
    private void ResourceDisappeared()
    {
        StartCoroutine(SpawnOverTime());
    }
    
    private void Save()
    {
        if (_isSpawning)
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
            {
               SpawnResourse();
            }
            else
            {
                _elapsedTime = savedTime - gameTime;
                ResourceDeath();
                ResourceDisappeared();
            }
        }
    }
}
