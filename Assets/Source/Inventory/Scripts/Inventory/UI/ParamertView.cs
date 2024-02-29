using TMPro;
using UnityEngine;

public class ParamertView : MonoBehaviour
{
    [SerializeField] private ParamertType _paramertType;
    [SerializeField] private TMP_Text _valueText;
    [SerializeField] private string _textBegin;
    [SerializeField] private string _textEnd;

    public void UpdateInfo(ParamertType paramertType, float valueText)
    {
        if(paramertType == _paramertType)
        {
            if(valueText > 0)
                _valueText.text = _textBegin + valueText + _textEnd;
            else
                _valueText.text = valueText + _textEnd;

            gameObject.SetActive(true);
        }
    }
}

public enum ParamertType
{
    GorenjeTime = 0,
    Endurance = 1,

    DamageToEnemies = 2,
    DamageToResources = 3,
    DamageAfterTime = 4,
    ImpactSpeed = 5,

    Armor = 6,
    Speed = 7,

    Satiety = 8,
    Thirst = 9,
    Helfth = 10,

    GrowthTime = 11,

    Sleep = 12,
}
