using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ViewInventoryNotifier : MonoBehaviour
{
    [SerializeField] private Image _frame;
    [SerializeField] private Image _icon;
    [SerializeField] private TMP_Text _amountText;
    [SerializeField] private Color _addItemColor;
    [SerializeField] private Color _removeItemColor;
    [SerializeField] private AnimationUI _animationUI;

    private Coroutine _coroutine;
    private WaitForSeconds _waitForSeconds;
    private float _delay = 1f;

    public bool IsOpen => _animationUI.IsOpen;

    public void Init(InventoryItemData itemData, int amount)
    {
        _icon.sprite = itemData.Icon;

        if(amount > 0)
        {
            _frame.color = _addItemColor;
            _amountText.text = "+" + amount.ToString();
        }
        else
        {
            _frame.color = _removeItemColor;
            _amountText.text = amount.ToString();
        }
    }

    public void UpdateAmount(int amount)
    {
        _amountText.text = amount.ToString();
    }

    public void Open()
    {
        _animationUI.OpenAnimation();

        ClearCoroutine();
        _coroutine = StartCoroutine(CloseWaitForSecound());
    }

    public void Close()
    {
        _animationUI.CloseAnimation();
    }

    private void ClearCoroutine()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
            _coroutine = null;
        }
    }

    private IEnumerator CloseWaitForSecound()
    {
        yield return _waitForSeconds = new WaitForSeconds(_delay);
        Close();
        yield return _waitForSeconds = new WaitForSeconds(_delay);
        gameObject.SetActive(false);
    }
}
