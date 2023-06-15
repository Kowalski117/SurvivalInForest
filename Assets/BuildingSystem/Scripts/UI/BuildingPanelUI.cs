using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class BuildingPanelUI : MonoBehaviour
{
    [SerializeField] private BuildingSideUI _sideUI;
    [SerializeField] private BuildingData[] _knownBuildingParts;
    [SerializeField] private BuildingPartUI _buildingButtonPrefab;
    [SerializeField] private Transform _itemContainer;

    public static UnityAction<BuildingData> OnPartChosen;

    public void OnClick(BuildingData chosenData)
    {
        OnPartChosen?.Invoke(chosenData);
        _sideUI.UpdateSideDisplay(chosenData);
    }

    public void OnClickAllParts()
    {
        PopulateButtons();
    }

    public void OnClickWall()
    {
        PopulateButtons(PartType.Wall);
    }

    public void PopulateButtons()
    {
        SpawnButtons(_knownBuildingParts);
    }

    public void PopulateButtons(PartType chosenPartType)
    {
        BuildingData[] BuildingPieces = _knownBuildingParts.Where(i => i.PartType == chosenPartType).ToArray();
        SpawnButtons(BuildingPieces);
    }

    public void SpawnButtons(BuildingData[] buttonsData)
    {
        ClearButtons();

        foreach (var data in buttonsData)
        {
            BuildingPartUI spawnedButton = Instantiate(_buildingButtonPrefab, _itemContainer);
            spawnedButton.Init(data, this);
        }
    }

    public void ClearButtons()
    {
        foreach (var button in _itemContainer.Cast<Transform>())
        {
            Destroy(button.gameObject);
        }
    }
}
