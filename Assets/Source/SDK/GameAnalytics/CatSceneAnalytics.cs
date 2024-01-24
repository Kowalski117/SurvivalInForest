using UnityEngine;

public class CatSceneAnalytics : MonoBehaviour
{
    private CutScene _cutScene;
    private Analytics _analytics;

    private void Awake()
    {
        _cutScene = GetComponent<CutScene>();
        _analytics = GetComponent<Analytics>();
    }

    private void OnEnable()
    {
        _cutScene.OnStart += StartCatScene;
        _cutScene.OnScip += ScipCatScene;
        _cutScene.OnFinish += FinishCatScene;
    }

    private void OnDisable()
    {
        _cutScene.OnStart -= StartCatScene;
        _cutScene.OnScip -= ScipCatScene;
        _cutScene.OnFinish -= FinishCatScene;
    }

    private void StartCatScene() 
    {
        _analytics.OnStart("StartCutScene");
    }

    private void ScipCatScene()
    {
        _analytics.OnFail("ScipCutScene");
    }

    private void FinishCatScene()
    {
        _analytics.OnComplete("FinishCutScene");
    }
}
