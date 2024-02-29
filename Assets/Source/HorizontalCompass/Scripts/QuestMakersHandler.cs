using System.Collections.Generic;
using UnityEngine;

public class QuestMakersHandler : MonoBehaviour
{
    [SerializeField] private QuestMaker[] _defaultMakers;
    [SerializeField] private QuestMakerZone[] _allMakersZone;   
    [SerializeField] private Compass _compass;

    private List<string> _idMakers = new List<string>();

    private void OnEnable()
    {
        _compass.OnQuestMakerAdded += Add;

        SavingGame.OnGameSaved += Save;
        SavingGame.OnGameLoaded += Load;
    }

    private void OnDisable()
    {
        _compass.OnQuestMakerAdded -= Add;

        SavingGame.OnGameSaved -= Save;
        SavingGame.OnGameLoaded -= Load;
    }

    public void Add(QuestMaker questMaker)
    {
        if (!_idMakers.Contains(questMaker.Id))
            _idMakers.Add(questMaker.Id);
    }

    private void Save()
    {
        ES3.Save(SaveLoadConstants.IdMakers, _idMakers);
    }

    private void Load()
    {
        if (ES3.KeyExists(SaveLoadConstants.IdMakers))
        {
            _idMakers.Clear();
            _idMakers = new List<string>();

            _idMakers = ES3.Load<List<string>>(SaveLoadConstants.IdMakers);

            if (_idMakers.Count <= 0)
                return;

            foreach (string id in _idMakers)
            {
                foreach (var makerZone in _allMakersZone)
                {
                    bool isAdd = false;

                    foreach (var maker in makerZone.OpenQuestMakers)
                    {
                        if (id == maker.Id)
                        {
                            _compass.AddQuestMarket(maker);
                            isAdd = true;
                        }
                    }

                    foreach (var maker in makerZone.HiddenQuestMakers)
                    {
                        if (isAdd)
                            _compass.RemoveQuestMarket(maker);
                    }
                }
            }
        }
    }
}
