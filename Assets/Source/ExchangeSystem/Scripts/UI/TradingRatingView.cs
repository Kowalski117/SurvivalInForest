using TMPro;
using UnityEngine;

public class TradingRatingView : MonoBehaviour
{
    [SerializeField] private TMP_Text _ratingText;

    private TradingRating _tradingRating;

    private void Awake()
    {
        _tradingRating = GetComponent<TradingRating>();
        UpdateRatingText(_tradingRating.CurrentRating);
    }

    private void OnEnable()
    {
        _tradingRating.OnRatingChanged += UpdateRatingText;
    }

    private void OnDisable()
    {
        _tradingRating.OnRatingChanged -= UpdateRatingText;
    }

    private void UpdateRatingText(int rating)
    {
        _ratingText.text = rating.ToString();
    }
}
