using UnityEngine;

[CreateAssetMenu(menuName = "Loading Screen Settings", order = 51)]
public class LoadingScreenSettings : ScriptableObject
{
    [SerializeField] private SceneParameters[] _sceneParameters;

    [SerializeField] private Sprite[] _hintSprites;
    [SerializeField] private string[] _hintTexts;

    public SceneParameters[] SceneParameters => _sceneParameters;
    public Sprite[] HintSprites => _hintSprites;
    public string[] HintTexts => _hintTexts;
}

[System.Serializable]
public struct SceneParameters
{
    [SerializeField] private int _sceneIndex;
    [SerializeField] private string _sceneName;

    public int SceneIndex => _sceneIndex;
    public string SceneName => _sceneName;
}