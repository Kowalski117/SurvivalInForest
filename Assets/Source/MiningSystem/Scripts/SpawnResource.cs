using System.Collections;
using UnityEngine;
using DG.Tweening;

public class SpawnResource : MonoBehaviour
{
    [SerializeField] private float _spawnTime;
    [SerializeField] private float _scaleTime = 30;
    [SerializeField] private GameObject _remainder;

    private Resource _resource;
    private Coroutine _coroutineSpawn;

    private void Awake()
    {
        _resource = gameObject.GetComponentInChildren<Resource>();
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
        _resource.gameObject.transform.position = transform.position;
        _resource.gameObject.transform.rotation = transform.rotation;

        if (_coroutineSpawn != null)
        {
            StopCoroutine(_coroutineSpawn);
        }
        _coroutineSpawn = StartCoroutine(SpawnOverTime());
    }

    private IEnumerator SpawnOverTime()
    {
        yield return new WaitForSeconds(_spawnTime);

        _remainder.SetActive(false);
        _resource.ToggleCollider(false);
        _resource.gameObject.transform.localScale = Vector3.zero;
        _resource.transform.DOScale(Vector3.one, _scaleTime);
        _resource.gameObject.SetActive(true);
        _resource.Enable();

        yield return new WaitForSeconds(_scaleTime);

        _resource.ToggleCollider(true);
    }
}
