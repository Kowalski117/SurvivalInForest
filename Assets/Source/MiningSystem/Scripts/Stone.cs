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
    
    public override void Die()
    {
        Сollider.enabled = false;
        base.Die();
    }
}
