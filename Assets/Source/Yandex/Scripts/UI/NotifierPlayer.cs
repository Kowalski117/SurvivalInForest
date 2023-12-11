using TMPro;
using UnityEngine;

public class NotifierPlayer : MonoBehaviour
{
    [SerializeField] SaveGame _saveGame;
    [SerializeField] private TMP_Text _textTimer;
    [SerializeField] private Transform _panel;

    private float _timer;

    private void Awake()
    {
        _panel.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        _saveGame.OnNotifyPlayer += UpdateTimer;
        _saveGame.OnCloseNotifierPlayer += CloseTimer;
    }

    private void OnDisable()
    {
        _saveGame.OnNotifyPlayer -= UpdateTimer;
        _saveGame.OnCloseNotifierPlayer -= CloseTimer;
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
        if (!_panel.gameObject.activeInHierarchy)
        {
            _panel.gameObject.SetActive(true);
            _timer = time;
        }
    }

    private void CloseTimer()
    {
        if (_panel.gameObject.activeInHierarchy)
            _panel.gameObject.SetActive(false);
    }
}
