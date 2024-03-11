using System.Collections.Generic;
using UnityEngine;

public abstract class SaveHandler : MonoBehaviour
{
    [SerializeField] protected Transform Container;

    protected List<string> Ids = new List<string>();

    private void OnEnable()
    {
        SavingGame.OnGameLoaded += Load;
        SavingGame.OnGameSaved += Save;
        SavingGame.OnSaveDeleted += Delete;
    }

    private void OnDisable()
    {
        SavingGame.OnGameLoaded -= Load;
        SavingGame.OnGameSaved -= Save;
        SavingGame.OnSaveDeleted -= Delete;
    }

    public void AddId(string id)
    {
        Ids.Add(id);
    }

    protected abstract void Save();
    protected abstract void Load();
    protected abstract void Delete();
}
