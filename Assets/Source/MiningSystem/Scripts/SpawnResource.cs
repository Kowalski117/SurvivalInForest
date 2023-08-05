using System.Collections;
using UnityEngine;
using DG.Tweening;

public class SpawnResource : MonoBehaviour
{
    [SerializeField] private float _spawnTime;
    private Resource _resource;

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
        StartCoroutine(SpawnOverTime());
    }

    IEnumerator SpawnOverTime()
    {
        float scaleTime = 5f;
        yield return new WaitForSeconds(_spawnTime);
        _resource.gameObject.transform.localScale = new Vector3(0, 0, 0);
        _resource.transform.DOScale(new Vector3(1, 1, 1), scaleTime);
        _resource.gameObject.SetActive(true);
    }
}
