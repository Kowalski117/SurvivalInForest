using System.Collections;
using UnityEngine;
using DG.Tweening;

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
    private void Awake()
    {
        _resource = gameObject.GetComponentInChildren<Resource>();
        _resurseLocalePosition = _resource.transform.localPosition;
        _resurseLocaleRotation = _resource.transform.localRotation;
        _resurseLocaleScale = _resource.transform.localScale;
    }

    private void OnEnable()
    {
        _resource.Died += ResourceDeath;
        _resource.Disappeared += ResourceDisappeared;
    }

    private void OnDisable()
    {
        _resource.Died -= ResourceDeath;
        _resource.Disappeared -= ResourceDisappeared;
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
        yield return new WaitForSeconds(_spawnTime);
        _resource.gameObject.transform.localScale = Vector3.zero;
        _remainder.SetActive(false);
        _resource.gameObject.SetActive(true);
        _resource.transform.DOScale(_resurseLocaleScale, _scaleTime);
        yield return new WaitForSeconds(_scaleTime);
        _resource.EnableCollider();
    }
}
