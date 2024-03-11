using UnityEngine;

[RequireComponent(typeof(CutScene))]
public class CatSceneAnalytics : Analytics
{
    private CutScene _cutScene;

    protected override void Awake()
    {
        base.Awake();

        _cutScene = GetComponent<CutScene>();
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
        GameAnalytics.OnStart(GameAnalyticsConstants.StartCutScene);
    }

    private void ScipCatScene()
    {
        GameAnalytics.OnFail(GameAnalyticsConstants.ScipCutScene);
    }

    private void FinishCatScene()
    {
        GameAnalytics.OnComplete(GameAnalyticsConstants.FinishCutScene);
    }
}
