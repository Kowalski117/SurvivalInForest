using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenStone : MonoBehaviour
{
    private const float DestroyDelay = 2f;

    [SerializeField] private float _disappearanceDelay = 10f;

    private List<Collider> _colliders = new List<Collider>();

    private WaitForSeconds _disappearanceWait;
    private WaitForSeconds _destroyWait = new WaitForSeconds(DestroyDelay);

    private void Awake()
    {
        _disappearanceWait = new WaitForSeconds(_disappearanceDelay);

        foreach (var child in GetComponentsInChildren<Collider>())
        {
            _colliders.Add(child);
        }
    }

    private void Start()
    {
        StartCoroutine(Destroy());
    }

    private IEnumerator Destroy()
    {
        yield return _disappearanceWait;

        foreach (var child in _colliders)
        {
            child.enabled = false;
        }

        yield return _destroyWait;

        Destroy(this.gameObject);
    }
}
