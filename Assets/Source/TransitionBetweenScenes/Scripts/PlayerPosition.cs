using UnityEngine;

[CreateAssetMenu(menuName = "PlayerPositionLastScene", order = 51)]
public class PlayerPosition : ScriptableObject
{
    [SerializeField] private ScenePosition[] _scenePositions;
    [SerializeField] private SleepPosition[] _sleepPositions;

    public ScenePosition[] LastScenePosition => _scenePositions;
    public SleepPosition[] SleepPositions => _sleepPositions;
}

[System.Serializable]
public struct ScenePosition
{
    [SerializeField] private int _lastSceneIndex;
    [SerializeField] private int _nextSceneIndex;
    [SerializeField] private Vector3 _position;
    [SerializeField] private Quaternion _rotation;

    public int LastSceneIndex => _lastSceneIndex;
    public int NextSceneIndex => _nextSceneIndex;
    public Vector3 Position => _position;
    public Quaternion Rotation => _rotation;
}

[System.Serializable]
public struct SleepPosition
{
    [SerializeField] private int _sceneIndex;
    [SerializeField] private Vector3 _position;
    [SerializeField] private Quaternion _rotation;

    public int SceneIndex => _sceneIndex;
    public Vector3 Position => _position;
    public Quaternion Rotation => _rotation;
}