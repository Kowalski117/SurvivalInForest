using DG.Tweening;
using Tayx.Graphy.Utils.NumString;
using UnityEngine;

public class Stone : Resource
{
    private float _maxHealth;
    private int[] _array = new int[] {75, 50, 25,0};
    private int _index;

    public override void OnEnable()
    {
        base.OnEnable();
        _maxHealth = Health;
        _index = 0;
    }

    public override void TakeDamage(float damage, float overTimeDamage)
    {
        
        base.TakeDamage(damage, overTimeDamage);
        float damagePercentage = Health / _maxHealth*100;

        if (damagePercentage < _array[_index])
        {
            Scale();
            _index++;
        }
    }

    private void Scale()
    {
        float scaleTime = 1f;
        float scale = _array[_index].ToFloat() / 100;
        transform.DOScale(new Vector3(scale, scale, scale),scaleTime);
    }
}
