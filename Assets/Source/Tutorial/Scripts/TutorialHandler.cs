using UnityEngine;

public class TutorialHandler : Raycast
{
    [SerializeField] private InfoScreen _infoScreen;
    [SerializeField] private InfoObjectData[] _infoObjects;
    [SerializeField] private LayerMask _layerMask;

    private InfoObject _infoObject;

    protected override void Awake()
    {
        base.Awake();
    }

    private void OnEnable()
    {
        SavingGame.OnGameLoaded += InitInfoObjects;
    }

    private void OnDisable()
    {
        SavingGame.OnGameLoaded -= InitInfoObjects;
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

    private void CheckObject(GameObject gameObject)
    {
        if (gameObject.TryGetComponent(out InfoObject infoObject))
        {
            foreach (var info in _infoObjects)
            {
                if (infoObject.InfoObjectSO == info && !info.IsDone && infoObject.IsActive)
                {
                    _infoObject = infoObject;
                    _infoScreen.Init(info.Name, info.Description, info.Sprite);
                    _infoScreen.Toggle();
                    info.SetIsDone(true);
                }
            }
        }
    }

    private void InitInfoObjects()
    {
        foreach (var obj in _infoObjects)
        {
            obj.Init();
        }
    }
}
