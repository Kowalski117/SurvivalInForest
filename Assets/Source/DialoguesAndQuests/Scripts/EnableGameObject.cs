using UnityEngine;
using UnityEngine.SceneManagement;

public class EnableGameObject : MonoBehaviour
{
    [SerializeField] private int[] _scenesEnable;
    [SerializeField] private Panel[] _panels;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        bool isActive = false;

        for (int i = 0; i < _scenesEnable.Length; i++)
        {
            if (SceneManager.GetActiveScene().buildIndex == _scenesEnable[i])
                isActive = true;
        }

        if (isActive)
        {
            for (int i = 0; i < _panels.Length; i++)
            {
                _panels[i].SetActive(_panels[i].IsActivePreviousScene);
            }
        }
        else
        {
            for (int i = 0; i < _panels.Length; i++)
            {
                _panels[i].SetActive(false);
            }
        }
    }
}

[System.Serializable]
public struct Panel
{
    [SerializeField] private Transform _panel;

    private bool _isActivePreviousScene;

    public bool IsActivePreviousScene => _isActivePreviousScene;

    public void SetActive(bool isActive)
    {
        _isActivePreviousScene = _panel.gameObject.activeInHierarchy;
        _panel.gameObject.SetActive(isActive);
    }
}
