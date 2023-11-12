using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class SaveGame : MonoBehaviour
{
    [SerializeField] private LoadPanel _loadPanel;
    [SerializeField] private bool _isAutoSave;
    [SerializeField] private float _autoSaveDelay;

    float _timer = 0;

    public static UnityAction OnSaveGame;
    public static UnityAction OnLoadData;

    private void Start()
    {
        if (ES3.KeyExists(SaveLoadConstants.StartLastSaveScene) && ES3.Load<bool>(SaveLoadConstants.StartLastSaveScene) == true || ES3.KeyExists(SaveLoadConstants.TransitionScene) && ES3.Load<bool>(SaveLoadConstants.TransitionScene) == true)
            StartCoroutine(WaitForLoad(0.5f));
    }

    private void Update()
    {
        if (_isAutoSave)
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
        OnSaveGame?.Invoke();
    }

    public void Load()
    {
        OnLoadData?.Invoke();
        ES3.Save(SaveLoadConstants.TransitionScene, false);
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