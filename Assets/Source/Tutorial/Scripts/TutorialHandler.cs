using UnityEngine;

public class TutorialHandler : Raycast
{
    [SerializeField] private InfoScreen _infoScreen;
    [SerializeField] private InfoObjectData[] _infoObjects;
    [SerializeField] private LayerMask _layerMask;

    protected override void Awake()
    {
        base.Awake();

        foreach (var obj in _infoObjects)
        {
            obj.Init();
        }
    }

    private void Update()
    {
        if(IsRayHittingSomething(_layerMask, out RaycastHit hitInfo))
        {
            CheckObject(hitInfo.transform.gameObject);
        }
    }

    private void CheckObject(GameObject gameObject)
    {
        if (gameObject.TryGetComponent(out InfoObject infoObject))
        {
            foreach (var info in _infoObjects)
            {
                if (infoObject.InfoObjectSO == info && !info.IsDone)
                {
                    _infoScreen.Init(info.Name, info.Description, info.Sprite);
                    _infoScreen.ToggleScreen();
                    info.SetIsDone(true);
                }
            }
        }
    }
}
