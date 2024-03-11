using TMPro;
using UnityEngine;

public class NotifierPlayer : MonoBehaviour
{
    [SerializeField] SavingGame _saveGame;
    [SerializeField] private TMP_Text _textTimer;
    [SerializeField] private AnimationUI _animationUI;

    private float _timer;

    private void Awake()
    {
        _animationUI.Close();
    }

    private void OnEnable()
    {
        _saveGame.OnNotifyedPlayer += UpdateTimer;
        _saveGame.OnClosedNotifierPlayer += CloseTimer;
    }

    private void OnDisable()
    {
        _saveGame.OnNotifyedPlayer -= UpdateTimer;
        _saveGame.OnClosedNotifierPlayer -= CloseTimer;
    }

    private void Update()
    {
        if(_timer > 0)
        {
            _timer -= Time.deltaTime;
            _textTimer.text = ((int)_timer).ToString();
        }
    }

    private void UpdateTimer(int time)
    {
        if (!_animationUI.IsOpen)
        {
            _animationUI.SetActivePanel(true);
            _animationUI.Open();
            _timer = time;
        }
    }

    private void CloseTimer()
    {
        if (_animationUI.IsOpen)
            _animationUI.Close();
    }
}
