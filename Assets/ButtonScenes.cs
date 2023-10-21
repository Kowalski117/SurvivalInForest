using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonScenes : MonoBehaviour
{
    [SerializeField] private LoadPanel _loadPanel;

    public void StartSceneForest()
    {
        _loadPanel.gameObject.SetActive(true);
        _loadPanel.Load(1, () => SceneManager.LoadScene(1), 1);
    }
    
        public void StartSceneCave()
        {
            _loadPanel.gameObject.SetActive(true);
            _loadPanel.Load(1, () => SceneManager.LoadScene(2), 1);
        }
}
