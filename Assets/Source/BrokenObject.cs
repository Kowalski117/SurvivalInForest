using UnityEngine;

public class BrokenObject : MonoBehaviour, IDamagable
{
    [SerializeField] private float _maxEndurance;
    [SerializeField] private GameObject _brokenObject;

    private float _currentEndurance;
    private BoxCollider _collider;
    private UniqueID _uniqueID;

    public float MaxEndurance => _maxEndurance;
    public float Endurance => _currentEndurance;

    private void Awake()
    {
        _collider = GetComponent<BoxCollider>();
        _uniqueID = GetComponent<UniqueID>();
        _currentEndurance = _maxEndurance;
    }

    private void Start()
    {
        Load();
    }

    private void OnEnable()
    {
        SaveGame.OnSaveGame += Save;
    }

    private void OnDisable()
    {
        SaveGame.OnSaveGame -= Save;
    }

    public void Die()
    {
        _currentEndurance = 0;
        _brokenObject.SetActive(false);
        _collider.enabled = false;
    }

    public void TakeDamage(float damage, float overTimeDamage)
    {
        _currentEndurance -= damage;

        if (_currentEndurance <= 0)
            Die();
    }

    private void Save()
    {
        BrokenObjectSaveData brokenObjectSaveData = new BrokenObjectSaveData(_currentEndurance);
        ES3.Save(_uniqueID.Id, brokenObjectSaveData);
    }

    private void Load()
    {
        if (ES3.KeyExists(_uniqueID.Id))
        {
            BrokenObjectSaveData brokenObjectSaveData = ES3.Load<BrokenObjectSaveData>(_uniqueID.Id);

            _currentEndurance = brokenObjectSaveData.CurrentEndurance;

            if (_currentEndurance <= 0)
                Die();
        }
    }
}

[System.Serializable]
public struct BrokenObjectSaveData
{
    [SerializeField] private float _currentEndurance;

    public float CurrentEndurance => _currentEndurance;

    public BrokenObjectSaveData(float currentEndurance)
    {
        _currentEndurance = currentEndurance;
    }
}
