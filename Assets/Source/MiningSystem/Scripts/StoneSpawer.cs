using System.Collections;
using UnityEngine;
using DG.Tweening;
using Unity.Mathematics;
using PixelCrushers.QuestMachine;

[RequireComponent(typeof (UniqueID))]
public class StoneSpawer: MonoBehaviour
{
    [SerializeField] private float _spawnTime;
    [SerializeField] private float _scaleTime;
    [SerializeField] private GameObject _remainder;
    [SerializeField] private BrokenStone _brokenStone;

    private Resource _resource;
    private QuestControl _questControl;
    private UniqueID _uniqueID;
    private Coroutine _coroutineSpawn;
    private WaitForSeconds _scaleWait;

    private Vector3 _resurseLocaleScale;
    private bool _isSpawning = false;
    private float _elapsedTime = 0;
    
    private void Awake()
    {
        _resource = gameObject.GetComponentInChildren<Resource>();
        _resurseLocaleScale = _resource.transform.localScale;
        _uniqueID = GetComponent<UniqueID>();
        _questControl = GetComponentInParent<QuestControl>();
        _scaleWait = new WaitForSeconds(_scaleTime);
    }
    
    private void Update()
    {
        if (_isSpawning)
            _elapsedTime += Time.deltaTime;
    }

    private void OnEnable()
    {
        _resource.Died += ResourceDeath;

        SavingGame.OnGameSaved += Save;
        SavingGame.OnGameLoaded += Load;
        SavingGame.OnSaveDeleted += Delete;
    }

    private void OnDisable()
    {
        _resource.Died -= ResourceDeath;

        SavingGame.OnGameSaved -= Save;
        SavingGame.OnGameLoaded -= Load;
        SavingGame.OnSaveDeleted -= Delete;
    }

    private void ResourceDeath()
    {
        _questControl.SendToMessageSystem(MessageConstants.Broken + _resource.Name);
        _remainder.SetActive(true);
        Instantiate(_brokenStone, transform.position,quaternion.identity,this.transform);
        StartSpawn();
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
        yield return new WaitForSeconds(_spawnTime - _elapsedTime);

        SpawnResourse();
        _elapsedTime = 0;

        yield return new WaitForSeconds(_scaleTime);

        PlayerPrefs.DeleteKey(_uniqueID.Id);
    }
    
    private void StartSpawn()
    {
        if (_coroutineSpawn != null)
        {
            StopCoroutine(_coroutineSpawn);
            _coroutineSpawn = null;
        }

        _coroutineSpawn = StartCoroutine(SpawnOverTime());
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
               SpawnResourse();
            else
            {
                _elapsedTime = savedTime - gameTime;
                ResourceDeath();
                StartSpawn();
            }
        }
    }

    private void Delete()
    {
        if (PlayerPrefs.HasKey(_uniqueID.Id + SaveLoadConstants.ResourceRevivalTime))
            PlayerPrefs.DeleteKey(_uniqueID.Id + SaveLoadConstants.ResourceRevivalTime);
    }
}
