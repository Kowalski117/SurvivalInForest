using Tayx.Graphy.Utils.NumString;
using UnityEngine;
using UnityEngine.Events;

public class Box : MonoBehaviour, IDamagable
{
    [SerializeField] private float _maxEndurance;
    [SerializeField] private GameObject _object;
    [SerializeField] private BrokenObject _brokenObject;
    [SerializeField] private Transform _pointForce;

    private float _currentEndurance;
    private BrokenObject _currentBrokenObject;
    private BoxCollider _collider;
    private UniqueID _uniqueID;
    private bool _firstDamage;
    private bool _isDead;

    public event UnityAction OnDied;

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
        SaveGame.OnLoadData += Load;

        _firstDamage = true;
        _isDead = false;
       _currentBrokenObject = Instantiate(_brokenObject, _object.transform.position, Quaternion.identity, gameObject.transform);
       _currentBrokenObject.gameObject.SetActive(false);
        _currentBrokenObject.SetPointForce(_pointForce);
    }

    private void OnDisable()
    {
        SaveGame.OnSaveGame -= Save;
        SaveGame.OnLoadData -= Load;
    }

    public void Die()
    {
        OnDied?.Invoke();
        _currentEndurance = 0;
        _collider.enabled = false;
        _isDead = true;

        _currentBrokenObject.DropFragment(_brokenObject.CountObjectFragments,_isDead);
    }

    public void TakeDamage(float damage, float overTimeDamage)
    {
        if (_firstDamage)
        {
            _object.SetActive(false); 
            _currentBrokenObject.gameObject.SetActive(true);
            _firstDamage = false;
        }

        _currentBrokenObject.DropFragment((damage/10).ToInt(),_isDead);
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
                Destroy(gameObject);
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
