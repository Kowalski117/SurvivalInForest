using UnityEngine;

public class QuestMakerZone : MonoBehaviour
{
    [SerializeField] private QuestMaker[] _hiddenQuestMakers;
    [SerializeField] private QuestMaker[] _openQuestMakers;

    private UniqueID _uniqueID;
    private bool _isAddMaker = false;

    public QuestMaker[] HiddenQuestMakers => _hiddenQuestMakers;
    public QuestMaker[] OpenQuestMakers => _openQuestMakers;

    private void Awake()
    {
        _uniqueID = GetComponent<UniqueID>();
    }

    private void OnEnable()
    {
        SaveGame.OnLoadData += Load;
    }

    private void OnDisable()
    {
        SaveGame.OnLoadData -= Load;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerHandler playerHandler) && !_isAddMaker)
        {
            if (_hiddenQuestMakers.Length > 0)
            {
                foreach (var maker in _hiddenQuestMakers)
                {
                    playerHandler.Compass.RemoveQuestMarket(maker);
                }
            }

            if (_openQuestMakers.Length > 0)
            {
                foreach (var maker in _openQuestMakers)
                {
                    playerHandler.Compass.AddQuestMarket(maker);
                }
            }

            _isAddMaker = true; 
            Save();
            gameObject.SetActive(false);
        }
    }

    private void Save()
    {
        ES3.Save(SaveLoadConstants.IsAddMaker + _uniqueID.Id, _isAddMaker);
    }

    private void Load()
    {
        if (ES3.KeyExists(SaveLoadConstants.IsAddMaker + _uniqueID.Id))
        {
            _isAddMaker = ES3.Load<bool>(SaveLoadConstants.IsAddMaker + _uniqueID.Id);

            if (_isAddMaker)
                gameObject.SetActive(false);
        }
    }
}
