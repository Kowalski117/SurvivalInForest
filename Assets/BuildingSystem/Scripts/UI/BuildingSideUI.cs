using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingSideUI : MonoBehaviour
{
    [SerializeField] private Image _buildingIcon;
    [SerializeField] private TMP_Text _buildingLabel;

    private void Awake()
    {
        _buildingIcon.sprite = null;
        _buildingIcon.color= Color.clear;
        _buildingLabel.text = "";
    }

    public void UpdateSideDisplay(BuildingData data)
    {
        _buildingIcon.sprite = data.Icon;
        _buildingIcon.color = Color.white;
        _buildingLabel.text = data.DisplayName;
    }
}
