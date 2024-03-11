using System;
using UnityEngine;

[Serializable]
[ExecuteInEditMode]
public class UniqueID : MonoBehaviour
{
    [ReadOnly, SerializeField] private string _id;
    [SerializeField] private bool _isGenerateAwake = true;
    [SerializeField] private static SerializableDictionary<string, GameObject> _idDatabase = new SerializableDictionary<string, GameObject>();

    public string Id => _id;

    private void Awake()
    {
        if(_idDatabase == null )
            _idDatabase = new SerializableDictionary<string, GameObject>();

        if (_idDatabase.ContainsKey(_id) && _isGenerateAwake)
            Generate();
        else
            _idDatabase.Add(_id, this.gameObject);
    }

    private void OnDestroy()
    {
        if(_idDatabase.ContainsKey(_id))
            _idDatabase.Remove(_id);
    }

    [ContextMenu("Generate ID")]
    public void Generate()
    {
        _id = Guid.NewGuid().ToString();
        _idDatabase.Add(_id, this.gameObject);
    }

    public void SetId(string id)
    {
        _id = id;
        if (!_idDatabase.ContainsKey(_id))
            _idDatabase.Add(_id, this.gameObject);
    }
}
