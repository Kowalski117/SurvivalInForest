using UnityEngine;

[CreateAssetMenu(menuName = "Inventory System/BuildingRecipe", order = 51)]
public class BuildingRecipe : CraftRecipe
{
    [SerializeField] private BuildingData _buildingData;

    public BuildingData BuildingData => _buildingData;
}
