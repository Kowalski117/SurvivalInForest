using System.Collections;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof (UniqueID))]
public class SpawnResource : MonoBehaviour
{
    [SerializeField] private float _spawnTime;
    [SerializeField] private float _scaleTime;
    [SerializeField] private GameObject _remainder; 

    private Resource _resource;
    private Coroutine _coroutineSpawn;
    private Vector3 _resurseLocalePosition;
    private Quaternion _resurseLocaleRotation;
    private Vector3 _resurseLocaleScale;
    private bool _isSpawning = false;
    private float _elapsedTime = 0;
    private UniqueID _uniqueID;

    private void Awake()
    {
        _resource = gameObject.GetComponentInChildren<Resource>();
        _resurseLocalePosition = _resource.transform.localPosition;
        _resurseLocaleRotation = _resource.transform.localRotation;
        _resurseLocaleScale = _resource.transform.localScale;

        _uniqueID = GetComponent<UniqueID>();
    }

    private void OnEnable()
    {
        _resource.Died += ResourceDeath;
        _resource.Disappeared += ResourceDisappeared;

        SaveGame.OnSaveGame += Save;
        SaveGame.OnLoadData += Load;
    }

    private void OnDisable()
    {
        _resource.Died -= ResourceDeath;
        _resource.Disappeared -= ResourceDisappeared;

        SaveGame.OnSaveGame -= Save;
        SaveGame.OnLoadData -= Load;
    }

    private void Update()
    {
        if (_isSpawning)
            _elapsedTime += Time.deltaTime;
    }

    private void ResourceDeath()
    {
        _remainder.SetActive(true);
    }

    private void ResourceDisappeared()
    {
        _resource.gameObject.transform.localPosition = _resurseLocalePosition;
        _resource.gameObject.transform.localRotation = _resurseLocaleRotation;

        if (_coroutineSpawn != null)
        {
            StopCoroutine(_coroutineSpawn);
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

        yield return new WaitForSeconds(_scaleTime);
        _resource.EnableCollider();
        PlayerPrefs.DeleteKey(_uniqueID.Id);
    }

    private void SpawnResourse()
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
            PlayerPrefs.SetFloat(_uniqueID.Id + "Time", PlayerPrefs.GetFloat("GameTimeÑounter") + _elapsedTime);
            PlayerPrefs.Save();
        }
    }

    private void Load()
    {
        if (PlayerPrefs.HasKey(_uniqueID.Id + "Time"))
        {
            float savedTime = PlayerPrefs.GetFloat(_uniqueID.Id + "Time");
            float gameTime = PlayerPrefs.GetFloat("GameTimeÑounter");

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
