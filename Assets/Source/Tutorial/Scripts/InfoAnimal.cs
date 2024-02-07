using UnityEngine;

public class InfoAnimal : MonoBehaviour
{
    private Animals _animals;
    private InfoObject[] _infoObjects;

    private void Awake()
    {
        _animals = GetComponent<Animals>();
        _infoObjects = GetComponentsInChildren<InfoObject>();

        for (int i = 0; i < _infoObjects.Length; i++)
        {
            _infoObjects[i].SetActive(false);
        }
    }

    private void OnEnable()
    {
        _animals.Died += EnableInfoObject;
    }

    private void OnDisable()
    {
        _animals.Died -= EnableInfoObject;
    }

    private void EnableInfoObject()
    {
        for (int i = 0; i < _infoObjects.Length; i++)
        {
            _infoObjects[i].SetActive(true);
        }
    }
}
