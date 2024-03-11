using UnityEngine;

[CreateAssetMenu(menuName = "DayRewards/DayRewardsData", order = 51)]
public class DayRewardsData : ScriptableObject
{
    [SerializeField] private DayReward[] _dayRewards;

    public DayReward[] DayRewards => _dayRewards;
}

[System.Serializable]
public struct DayReward 
{
    [SerializeField] private InventoryItem _slot;
    [SerializeField] private Color _backgroundÑolor;

    public InventoryItem Slot => _slot;
    public Color BackgroundColor => _backgroundÑolor;
}

