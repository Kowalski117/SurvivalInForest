using System.Collections.Generic;
using UnityEngine;

public abstract class SaveHandler : MonoBehaviour
{
    [SerializeField] protected Transform Container;

    protected List<string> Ids = new List<string>();

    private void OnEnable()
    {
        SaveGame.OnLoadData += LoadBase;
        SaveGame.OnSaveGame += SaveBase;
    }

    private void OnDisable()
    {
        SaveGame.OnLoadData -= LoadBase;
        SaveGame.OnSaveGame -= SaveBase;
    }

    public void AddId(string id)
    {
        Ids.Add(id);
    }

    protected abstract void SaveBase();
    protected abstract void LoadBase();
}
