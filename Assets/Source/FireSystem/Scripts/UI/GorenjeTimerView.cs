using System;
using TMPro;
using UnityEngine;

public class GorenjeTimerView : MonoBehaviour
{
    [SerializeField] private Fire _fire;
    [SerializeField] private TMP_Text _timerText;
    [SerializeField] private bool _isRotate = false;

    private float _minTotalMilliseconds = 10000;
    private Camera _mainCamera;

    private void Awake()
    {
        _mainCamera = Camera.main;
        _timerText.gameObject.SetActive(false); 
    }

    private void OnEnable()
    {
        _fire.OnCompletionTimeUpdate += UpdateTimer;
    }

    private void OnDisable()
    {
        _fire.OnCompletionTimeUpdate -= UpdateTimer;
    }

    private void LateUpdate()
    {
        if (_mainCamera != null && _timerText.gameObject.activeInHierarchy && _isRotate)
        {
            transform.LookAt(transform.position + _mainCamera.transform.rotation * Vector3.forward, _mainCamera.transform.rotation * Vector3.up);
        }
    }

    private void UpdateTimer(DateTime dateTime)
    {
        if(!_timerText.gameObject.activeInHierarchy)
            _timerText.gameObject.SetActive(true);

        _timerText.text = dateTime.ToString(GameConstants.HHmm);

        if (dateTime.TimeOfDay.TotalMilliseconds < _minTotalMilliseconds)
        {
            _timerText.gameObject.SetActive(false);
        }
    }
}
