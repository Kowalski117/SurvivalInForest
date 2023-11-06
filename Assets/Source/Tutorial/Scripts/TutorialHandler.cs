using UnityEngine;

public class TutorialHandler : Raycast
{
    [SerializeField] private InfoScreen _infoScreen;
    [SerializeField] private InfoObjectData[] _infoObjects;
    [SerializeField] private LayerMask _layerMask;

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
            Debug.Log("1");
            foreach (var info in _infoObjects)
            {
                if (infoObject.InfoObjectSO == info && !info.IsDone)
                {
                    Debug.Log("2");
                    _infoScreen.Init(info.Name, info.Description, info.Sprite);
                    _infoScreen.ToggleScreen();
                    info.SetIsDone(true);
                }
            }
        }
    }
}
