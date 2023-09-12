using System.Collections;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof (BoxCollider))]
public class SpawnResource : MonoBehaviour
{
    [SerializeField] private float _spawnTime = 15;
    [SerializeField] private float _scaleTime = 30;

    private Resource _resource;
    private Coroutine _coroutineSpawn;

    private void Awake()
    {
        _resource = gameObject.GetComponentInChildren<Resource>();
    }

    private void OnEnable()
    {
        _resource.Died += TreeDeath;
    }

    private void OnDisable()
    {
        _resource.Died += TreeDeath;
    }

    private void TreeDeath()
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

        _resource.ToggleCollider(false);
        _resource.gameObject.transform.localScale = Vector3.zero;
        _resource.transform.DOScale(Vector3.one, _scaleTime);
        _resource.gameObject.SetActive(true);
        _resource.Enable();

        yield return new WaitForSeconds(_scaleTime);

        _resource.ToggleCollider(true);
    }
}
