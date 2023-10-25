using UnityEngine;

public class Stone : Resource
{
    [SerializeField] private ResourseType _resourseType;

    public ResourseType ResourseType => _resourseType;

    public override void Die()
    {
        gameObject.SetActive(false);
        SpawnLoot();
        DiedEvent();
    }
}
