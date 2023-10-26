using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonScenes : MonoBehaviour
{
    [SerializeField] private LoadPanel _loadPanel;

    public void StartSceneForest()
    {
        _loadPanel.gameObject.SetActive(true);
        ES3.Save("StartLastSaveScene", false);
        _loadPanel.Load(1, () => SceneManager.LoadScene(1), 1);
    }

    public void StartSceneCave()
    {
        _loadPanel.gameObject.SetActive(true);
        ES3.Save("StartLastSaveScene", false);
        _loadPanel.Load(1, () => SceneManager.LoadScene(2), 1);
    }

    public void StartLastSaveScene()
    {
        if (ES3.KeyExists("SceneIndex"))
        {
            _loadPanel.gameObject.SetActive(true);

            int indexScene = ES3.Load<int>("SceneIndex");
            ES3.Save("StartLastSaveScene", true);
            _loadPanel.Load(1, () => SceneManager.LoadScene(indexScene), indexScene);
        }
    }
}
