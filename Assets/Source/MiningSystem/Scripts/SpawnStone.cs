using System.Collections;
using UnityEngine;
using DG.Tweening;
using Unity.Mathematics;

public class SpawnStone: MonoBehaviour
{
    [SerializeField] private float _spawnTime;
    [SerializeField] private float _scaleTime;
    [SerializeField] private GameObject _remainder;
    [SerializeField] private BrokenStone _brokenStone;

    private Resource _resource;
    private Coroutine _coroutineSpawn;
    private Vector3 _resurseLocaleScale;
    private void Awake()
    {
        _resource = gameObject.GetComponentInChildren<Resource>();
        _resurseLocaleScale = _resource.transform.localScale;
    }

    private void OnEnable()
    {
        _resource.Died += ResourceDeath;
    }

    private void OnDisable()
    {
        _resource.Died -= ResourceDeath;
    }

    private void ResourceDeath()
    {
        _remainder.SetActive(true);
        Instantiate(_brokenStone, transform.position,quaternion.identity,this.transform);
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

    }
}
