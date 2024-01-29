using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesAnalytics : Analytics
{
    [SerializeField] private LoadingScreenSettings _scenesIndex;

    private void Start()
    {
        DefineScene();
    }

    private void DefineScene()
    {
        foreach (var scene in _scenesIndex.SceneParameters)
        {
            if(SceneManager.GetActiveScene().buildIndex == scene.SceneIndex)
            {
#if UNITY_WEBGL && !UNITY_EDITOR
                GameAnalytics.OnStart(GameAnalyticsConstants.OpenScene + scene.SceneString);
#endif
            }
        }
    }
}
