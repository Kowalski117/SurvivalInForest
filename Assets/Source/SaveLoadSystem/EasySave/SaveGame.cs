using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class SaveGame : MonoBehaviour
{
    [SerializeField] private LoadPanel _loadPanel;
    [SerializeField] private bool _isAutoSave;
    [SerializeField] private float _autoSaveDelay;
    [SerializeField] private bool _isStartLoad = true;

    float _timer = 0;

    public static UnityAction OnSaveGame;
    public static UnityAction OnLoadData;

    private void Start()
    {
        if (_isStartLoad)
            StartCoroutine(WaitForLoad(0.5f));
    }

    private void Update()
    {
        if(_isAutoSave)
        {
            _timer += Time.deltaTime;

            if (_timer >= _autoSaveDelay)
            {
                _timer = 0;
                Save();
            }
        }
    }

    public void Save()
    {
        //_loadPanel.gameObject.SetActive(true);
        OnSaveGame?.Invoke();
        //_loadPanel.Load(1, _loadPanel.Deactivate);
    }

    public void Load()
    {
        //_loadPanel.gameObject.SetActive(true);
        OnLoadData?.Invoke();
        //_loadPanel.Load(1, _loadPanel.Deactivate);
    }

    public void Delete()
    {
        ES3.DeleteFile();
    }

    private IEnumerator WaitForLoad(float delay)
    {
        yield return new WaitForSeconds(delay);
        Load();
    }
}
