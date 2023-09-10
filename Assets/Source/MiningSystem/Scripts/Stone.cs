using DG.Tweening;
using Tayx.Graphy.Utils.NumString;
using UnityEngine;

public class Stone : Resource
{
    private float _maxHealth;

    public override void OnEnable()
    {
        base.OnEnable();
        _maxHealth = Health;
    }

    public override void TakeDamage(float damage, float overTimeDamage)
    {
        base.TakeDamage(damage, overTimeDamage);
    }

    public override void Die()
    {
        Ñollider.enabled = false;
        base.Die();
    }
}
