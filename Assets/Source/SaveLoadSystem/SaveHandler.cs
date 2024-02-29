using System.Collections.Generic;
using UnityEngine;

public abstract class SaveHandler : MonoBehaviour
{
    [SerializeField] protected Transform Container;

    protected List<string> Ids = new List<string>();

    private void OnEnable()
    {
        SavingGame.OnGameLoaded += LoadBase;
        SavingGame.OnGameSaved += SaveBase;
    }

    private void OnDisable()
    {
        SavingGame.OnGameLoaded -= LoadBase;
        SavingGame.OnGameSaved -= SaveBase;
    }

    public void AddId(string id)
    {
        Ids.Add(id);
    }

    protected abstract void SaveBase();
    protected abstract void LoadBase();
}
