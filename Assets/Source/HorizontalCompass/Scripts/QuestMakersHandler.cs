using System.Collections.Generic;
using UnityEngine;

public class QuestMakersHandler : MonoBehaviour
{
    [SerializeField] private QuestMakerZone[] _allMakersZone;   
    [SerializeField] private Compass _compass;

    private List<string> _idMakers = new List<string>();

    private void OnEnable()
    {
        _compass.OnQuestMakerAdded += Add;
        _compass.OnQuestMakerRemoved += Remove;

        SavingGame.OnGameSaved += Save;
        SavingGame.OnGameLoaded += Load;
        SavingGame.OnSaveDeleted += Delete;

    }

    private void OnDisable()
    {
        _compass.OnQuestMakerAdded -= Add;
        _compass.OnQuestMakerRemoved -= Remove;

        SavingGame.OnGameSaved -= Save;
        SavingGame.OnGameLoaded -= Load;
        SavingGame.OnSaveDeleted -= Delete;
    }

    public void Add(QuestMaker questMaker)
    {
        //if (!_idMakers.Contains(questMaker.Id))
        //    _idMakers.Add(questMaker.Id);
    }

    public void Remove(QuestMaker questMaker)
    {
        //if (_idMakers.Contains(questMaker.Id))
        //    _idMakers.Remove(questMaker.Id);

        //if (!_idMakers.Contains(questMaker.Id))
        //    _idMakers.Add(questMaker.Id);
    }

    private void Save()
    {
        ES3.Save(SaveLoadConstants.IdMakers, _idMakers);
    }

    private void Load()
    {
        //if (ES3.KeyExists(SaveLoadConstants.IdMakers))
        //{
        //    _idMakers.Clear();
        //    _idMakers = new List<string>();

        //    _idMakers = ES3.Load<List<string>>(SaveLoadConstants.IdMakers);

        //    if (_idMakers.Count <= 0)
        //        return;

            //foreach (string id in _idMakers)
            //{
                foreach (var makerZone in _allMakersZone)
                {

                    if (makerZone.IsAddMaker)
                    {
                        foreach (var maker in makerZone.OpenQuestMakers)
                        {
                            _compass.AddQuestMarket(maker);
                        }
                    }
                    else
                    {
                        foreach (var maker in makerZone.HiddenQuestMakers)
                        {
                            _compass.AddQuestMarket(maker);
                        }
                    }


                    //foreach (var maker in makerZone.OpenQuestMakers)
                    //{
                    //    if (id == maker.Id)
                    //    {
                    //        _compass.AddQuestMarket(maker);
                    //        isAdd = true;
                    //    }
                    //}

                    //foreach (var maker in makerZone.HiddenQuestMakers)
                    //{
                    //    if (isAdd)
                    //        _compass.RemoveQuestMarket(maker);
                    //}
                }
            //}
        //}
        //else
        //{
        //    //foreach (var makerZone in _allMakersZone)
        //    //{
        //    //    //    foreach (var maker in makerZone.OpenQuestMakers)
        //    //    //    {
        //    //    //        _compass.AddQuestMarket(maker); ;
        //    //    //    }

        //    //    foreach (var maker in makerZone.HiddenQuestMakers)
        //    //    {
        //    //        _compass.AddQuestMarket(maker);
        //    //    }
        //    //}
        //}
    }

    private void Delete()
    {
        if (ES3.KeyExists(SaveLoadConstants.IdMakers))
            ES3.DeleteKey(SaveLoadConstants.IdMakers);
    }
}
