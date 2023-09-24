using System;
using DG.Tweening;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Treasure : Resource
{
    [SerializeField] private ObjectItemsData[] _lootItems;
    [SerializeField] private ObjectPickUp _lootObject;

    private int[] _array = new int[] { 75, 50, 25, 0};
    private int _index;
    private float _changePositionDirection = 0.1f;
    private Vector3 _lootObjectPosition;
    private Tween _tweenTransform;
    private Tween _tweenLoot;

    private void Awake()
    {
        base.Awake();
        _lootObjectPosition = _lootObject.transform.localPosition;
    }
    
    public override void OnEnable()
    {
        base.OnEnable();
        _index = 0;
        _lootObject.gameObject.SetActive(true);
        _lootObject.TurnOff();
        SetRandomLoot();
        _lootObject.transform.localPosition = _lootObjectPosition;
    }

    public override void TakeDamage(float damage, float overTimeDamage)
    {
        base.TakeDamage(damage, overTimeDamage);
        float damagePercentage = Health / MaxHealth * 100;

        if (damagePercentage < _array[_index])
        {
            ChangePosition();
            _index++;
        }
    }

    public override void Die()
    {
        ClearTween(_tweenLoot);
        ClearTween(_tweenTransform);
        _lootObject.Enable();
        Сollider.enabled = false;
        base.Die();
    }
    
    private void SetRandomLoot()
    {
        int index = Random.Range(0, _lootItems.Length);
        _lootObject.Init(_lootItems[index]);
    }

    private void ChangePosition()
    {
        float ChangePositionTime = 1f;
        _tweenTransform = transform.DOMoveY(transform.position.y - _changePositionDirection, ChangePositionTime);
        _tweenLoot = _lootObject.transform.DOMoveY(_lootObject.transform.position.y + _changePositionDirection, ChangePositionTime);
    }

    private void ClearTween(Tween tween)
    {
        if (tween != null && tween.IsActive())
        {
            tween.Kill();
            tween = null;
        }
    }
}
