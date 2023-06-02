using System;
using UnityEngine;

[System.Serializable]
[ExecuteInEditMode]
public class UniqueID : MonoBehaviour
{
    [ReadOnly, SerializeField] private string _id;
    [SerializeField] private static SerializableDictionary<string, GameObject> _idDatabase = new SerializableDictionary<string, GameObject>();

    public string Id => _id;

    private void Awake()
    {
        if(_idDatabase == null )
            _idDatabase = new SerializableDictionary<string, GameObject>();

        if (_idDatabase.ContainsKey(_id))
            Generate();
        else
            _idDatabase.Add(_id, this.gameObject);
    }

    private void OnDestroy()
    {
        if(_idDatabase.ContainsKey(_id))
            _idDatabase.Remove(_id);
    }

    [ContextMenu("Genereta ID")]
    private void Generate()
    {
        _id = Guid.NewGuid().ToString();
        _idDatabase.Add(_id, this.gameObject);
    }
}
