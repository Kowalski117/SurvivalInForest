using System.Collections;
using UnityEngine;
using DG.Tweening;
using PixelCrushers.QuestMachine;

[RequireComponent(typeof (UniqueID))]
public class ResourceSpawner : MonoBehaviour
{
    [SerializeField] private float _spawnTime;
    [SerializeField] private float _scaleTime;
    [SerializeField] private GameObject _remainder;

    private Resource _resource;
    private Coroutine _coroutineSpawn;
    private WaitForSeconds _scaleWait;
    private UniqueID _uniqueID;
    private QuestControl _questControl;

    private Vector3 _resurseLocalePosition;
    private Quaternion _resurseLocaleRotation;
    private Vector3 _resurseLocaleScale;
    private bool _isSpawning = false;
    private float _elapsedTime = 0;

    private void Awake()
    {
        _resource = gameObject.GetComponentInChildren<Resource>();
        _resurseLocalePosition = _resource.transform.localPosition;
        _resurseLocaleRotation = _resource.transform.localRotation;
        _resurseLocaleScale = _resource.transform.localScale;
        _questControl = GetComponentInParent<QuestControl>();
        _uniqueID = GetComponent<UniqueID>();
        _scaleWait = new WaitForSeconds(_scaleTime);
    }

    private void OnEnable()
    {
        _resource.Died += ResourceDeath;
        _resource.Disappeared += StartSpawn;

        SavingGame.OnGameSaved += Save;
        SavingGame.OnGameLoaded += Load;
        SavingGame.OnSaveDeleted += Delete;
    }

    private void OnDisable()
    {
        _resource.Died -= ResourceDeath;
        _resource.Disappeared -= StartSpawn;

        SavingGame.OnGameSaved -= Save;
        SavingGame.OnGameLoaded -= Load;
        SavingGame.OnSaveDeleted += Delete;
    }

    private void Update()
    {
        if (_isSpawning)
            _elapsedTime += Time.deltaTime;
    }

    private void ResourceDeath()
    {
        _remainder.SetActive(true);
        _questControl.SendToMessageSystem(MessageConstants.Broken + _resource.Name);
    }

    private void StartSpawn()
    {
        _resource.gameObject.transform.localPosition = _resurseLocalePosition;
        _resource.gameObject.transform.localRotation = _resurseLocaleRotation;

        if (_coroutineSpawn != null)
        {
            StopCoroutine(_coroutineSpawn);
            _coroutineSpawn = null;
        }

        _coroutineSpawn = StartCoroutine(SpawnOverTime());
    }

    private IEnumerator SpawnOverTime()
    {
        _isSpawning = true;
        _resource.gameObject.transform.localScale = Vector3.zero;

        yield return new WaitForSeconds(_spawnTime - _elapsedTime);

        _isSpawning = false;
        _elapsedTime = 0;
        _remainder.SetActive(false);
        _resource.gameObject.SetActive(true);
        _resource.transform.DOScale(_resurseLocaleScale, _scaleTime);

        yield return _scaleWait;

        _resource.EnableCollider();
        PlayerPrefs.DeleteKey(_uniqueID.Id);
    }

    private void Spawn()
    {
        _remainder.SetActive(false);
        _resource.gameObject.SetActive(true);
        _resource.transform.DOScale(_resurseLocaleScale, _scaleTime);
        _resource.EnableCollider();
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
                Spawn();
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
