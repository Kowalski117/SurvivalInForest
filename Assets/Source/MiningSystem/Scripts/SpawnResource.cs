using System.Collections;
using UnityEngine;
using DG.Tweening;

public class SpawnResource : MonoBehaviour
{
    [SerializeField] private float _spawnTime;
    [SerializeField] private GameObject _remainder;
    private Resource _resource;

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
        StartCoroutine(SpawnOverTime());
    }

    IEnumerator SpawnOverTime()
    {
        float scaleTime = 5f;
        yield return new WaitForSeconds(_spawnTime);
        _resource.gameObject.transform.localScale = new Vector3(0, 0, 0);
        _resource.transform.DOScale(new Vector3(1, 1, 1), scaleTime);
        _remainder.SetActive(false);
        _resource.gameObject.SetActive(true);
    }
}
