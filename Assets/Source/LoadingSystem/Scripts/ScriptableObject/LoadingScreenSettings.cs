using PixelCrushers.DialogueSystem;
using PixelCrushers.Wrappers;
using UnityEngine;

[CreateAssetMenu(menuName = "Loading Screen Settings", order = 51)]
public class LoadingScreenSettings : ScriptableObject
{
    [SerializeField] private SceneParameters[] _sceneParameters;

    [SerializeField] private Sprite[] _hintSprites;
    [SerializeField] private TextTable _textTableHint;
    [SerializeField] private int _maxLengthTextTableHint;

    public SceneParameters[] SceneParameters => _sceneParameters;
    public Sprite[] HintSprites => _hintSprites;
    public string HintText => _textTableHint.GetFieldTextForLanguage(Random.Range(0,_maxLengthTextTableHint),  /*ES3.KeyExists(ConstantsSDK.Language) ? ES3.Load<string>(ConstantsSDK.Language) :*/ Localization.language);
}

[System.Serializable]
public struct SceneParameters
{
    [SerializeField] private int _sceneIndex;
    [SerializeField] private string _sceneString;
    [SerializeField] private string _sceneName;
    [SerializeField] private TextTable _textTableName;

    public int SceneIndex => _sceneIndex;
    public string SceneString => _sceneString;
    public string SceneName => _textTableName.GetFieldTextForLanguage(_sceneString, /*ES3.KeyExists(ConstantsSDK.Language) ? ES3.Load<string>(ConstantsSDK.Language) : */Localization.language);
}