using DG.Tweening;
using System.Collections;
using UnityEngine;

public class GardenBed : MonoBehaviour
{
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private SeedItemData _seedItem;
    [SerializeField] private AllSeeds _allSeeds;

    private BeingLiftedObject _currentItem;
    private UniqueID _uniqueID;

    private InventoryItemData _currentItemData;
    private float _elapsedTime = 0;
    private bool _isPlantGrows = false;

    private void Awake()
    {
        _uniqueID = GetComponent<UniqueID>();

        if(_uniqueID == null)
            _uniqueID = GetComponentInParent<UniqueID>();
    }

    private void Start()
    {
        Load();
    }

    private void Update()
    {
        if (_isPlantGrows)
            _elapsedTime += Time.deltaTime;
    }

    private void OnEnable()
    {
        SavingGame.OnGameSaved += Save;
        SavingGame.OnGameLoaded += Load;
        SavingGame.OnSaveDeleted += Delete;
    }

    private void OnDisable()
    {
        SavingGame.OnGameSaved -= Save;
        SavingGame.OnGameLoaded -= Load; 
        SavingGame.OnSaveDeleted -= Delete;
    }

    public bool StartGrowingSeed(InventoryItemData inventoryItemData, Vector3 scale = default(Vector3))
    {
        if (_currentItem == null && inventoryItemData != null && inventoryItemData is SeedItemData seedItemData)
        {
            _currentItemData = inventoryItemData;
            _currentItem = Instantiate(seedItemData.ObjectPickUp, _spawnPoint.position, Quaternion.identity, transform);
            _currentItem.gameObject.transform.localScale = scale;
            _currentItem.TurnOff();
            _currentItem.Init(SetLootItem(seedItemData));
            StartCoroutine(SpawnOverTime(seedItemData.GrowthTime - _elapsedTime));
            return true;
        }

        return false;
    }

    private IEnumerator SpawnOverTime(float time)
    {
        _isPlantGrows = true;
        _currentItem.transform.DOScale(Vector3.one, time);
        yield return new WaitForSeconds(time);
        _currentItem.Enable();
        Save();
        _currentItem = null;
        _currentItemData = null;
        _isPlantGrows = false;
        _elapsedTime = 0;
    }

    private void Save()
    {
        GardenBedSaveData gardenBedSaveData;

        if (_currentItemData != null && _currentItem != null)
            gardenBedSaveData = new GardenBedSaveData(_currentItemData.Id, _elapsedTime, _currentItem.gameObject.transform.localScale);
        else
            gardenBedSaveData = new GardenBedSaveData(0, _elapsedTime, Vector3.zero);

        ES3.Save(_uniqueID.Id + SaveLoadConstants.GardenBedSaveData, gardenBedSaveData);
    }

    private void Load()
    {
        if (ES3.KeyExists(_uniqueID.Id + SaveLoadConstants.GardenBedSaveData))
        {
            GardenBedSaveData gardenBedSaveData = ES3.Load<GardenBedSaveData>(_uniqueID.Id + SaveLoadConstants.GardenBedSaveData);

            if(gardenBedSaveData.CurrentItemDataId != 0)
            {
                foreach (var item in _allSeeds.Items)
                {
                    if(item.Id == gardenBedSaveData.CurrentItemDataId)
                        _currentItemData = item;
                }

                _elapsedTime = gardenBedSaveData.ElapsedTime;

                if (gardenBedSaveData.Scale != Vector3.one)
                {
                    StartGrowingSeed(_currentItemData, gardenBedSaveData.Scale);
                }
                else
                {
                    if (_currentItemData is SeedItemData seedItemData)
                    {
                        _currentItem = Instantiate(seedItemData.ObjectPickUp, _spawnPoint.position, Quaternion.identity, transform);
                        _currentItem.Init(SetLootItem(seedItemData));
                    }
                }
            }
        }
        else
        {
            if (_seedItem != null)
            {
                _currentItem = Instantiate(_seedItem.ObjectPickUp, _spawnPoint.position, Quaternion.identity, transform);
                _currentItemData = _seedItem;
                _currentItem.Init(SetLootItem(_seedItem));
            }
        }
    }

    private void Delete()
    {
        if (ES3.KeyExists(_uniqueID.Id + SaveLoadConstants.GardenBedSaveData))
            ES3.DeleteKey(_uniqueID.Id + SaveLoadConstants.GardenBedSaveData);
    }

    private ObjectItemsData SetLootItem(SeedItemData seedItemData)
    {
        return seedItemData.LootItems[Random.Range(0, seedItemData.LootItems.Length)];
    }
}

[System.Serializable]
public struct GardenBedSaveData
{
    [SerializeField] private float _currentItemDataId;
    [SerializeField] private float _elapsedTime;
    [SerializeField] private Vector3 _scale;

    public float CurrentItemDataId => _currentItemDataId;
    public float ElapsedTime => _elapsedTime;
    public Vector3 Scale => _scale;

    public GardenBedSaveData(float currentItemDataId, float elapsedTime, Vector3 scale)
    {
        _currentItemDataId = currentItemDataId;
        _elapsedTime = elapsedTime;
        _scale = scale;
    }
}