using UnityEngine;

public class QuestMakerZone : MonoBehaviour
{
    [SerializeField] private Compass _compass;
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
        SavingGame.OnGameLoaded += Load;
    }

    private void OnDisable()
    {
        SavingGame.OnGameLoaded -= Load;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerHandler>() && !_isAddMaker)
        {
            if (_hiddenQuestMakers.Length > 0)
            {
                foreach (var maker in _hiddenQuestMakers)
                {
                    _compass.RemoveQuestMarket(maker);
                }
            }

            if (_openQuestMakers.Length > 0)
            {
                foreach (var maker in _openQuestMakers)
                {
                    _compass.AddQuestMarket(maker);
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
