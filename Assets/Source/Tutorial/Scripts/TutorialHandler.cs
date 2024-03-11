using System.Collections.Generic;
using UnityEngine;

public class TutorialHandler : Raycast
{
    [SerializeField] private InfoScreen _infoScreen;
    [SerializeField] private TutorialDataObject[] _infoObjects;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private GameObject _button;

    private InfoObject _infoObject;
    private List<TutorialDetail> _currentDetails = new List<TutorialDetail>();

    private int _minCount = 1;

    protected override void Awake()
    {
        base.Awake();
    }

    private void OnEnable()
    {
        SavingGame.OnGameLoaded += InitInfoObjects;
        SavingGame.OnSaveDeleted += Delete;
        _infoScreen.OnButtonResumed += ReplaceDelail;
    }

    private void OnDisable()
    {
        SavingGame.OnGameLoaded -= InitInfoObjects;
        SavingGame.OnSaveDeleted -= Delete;
        _infoScreen.OnButtonResumed -= ReplaceDelail;
    }

    private void Update()
    {
        if(IsRayHittingSomething(_layerMask, out RaycastHit hitInfo))
            CheckObject(hitInfo.transform.gameObject);
        else
        {
            if (_infoObject)
            {
                _infoObject = null;

                if (_infoScreen.IsOpenPanel)
                    _infoScreen.Toggle();
            }
        }
    }
    public void UpdateInfo(TutorialDataObject info)
    {
        if (!info.IsDone)
        {
            foreach (var detail in info.Details)
            {
                _currentDetails.Add(detail);
            }

            info.SetIsDone(true);
            _infoScreen.Toggle();
            ReplaceDelail();
        }
    }

    private void CheckObject(GameObject gameObject)
    {
        if (gameObject.TryGetComponent(out InfoObject infoObject))
        {
            foreach (var info in _infoObjects)
            {
                if (infoObject.InfoObjectSO == info && !info.IsDone && infoObject.IsActive)
                {
                    _infoObject = infoObject;

                    UpdateInfo(_infoObject.InfoObjectSO);
                }
            }
        }
    }


    private void ReplaceDelail()
    {
        if(_currentDetails.Count > 0)
        {
            _infoScreen.Init(_currentDetails[0].Name, _currentDetails[0].Description, _currentDetails[0].Sprite);
            _currentDetails.RemoveAt(0);
        }

        if(_currentDetails.Count >= _minCount)
            _button.SetActive(true);
        else
            _button.SetActive(false);   
    }

    private void InitInfoObjects()
    {
        foreach (var obj in _infoObjects)
        {
            obj.Init();
        }
    }

    private void Delete()
    {
        foreach (var obj in _infoObjects)
        {
            obj.Delete();
        }
    }
}
