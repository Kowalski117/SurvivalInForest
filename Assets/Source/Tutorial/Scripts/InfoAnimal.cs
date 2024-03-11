using UnityEngine;

[RequireComponent(typeof(Animals))]
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
        _animals.Died += EnableObject;
    }

    private void OnDisable()
    {
        _animals.Died -= EnableObject;
    }

    private void EnableObject()
    {
        for (int i = 0; i < _infoObjects.Length; i++)
        {
            _infoObjects[i].SetActive(true);
        }
    }
}
