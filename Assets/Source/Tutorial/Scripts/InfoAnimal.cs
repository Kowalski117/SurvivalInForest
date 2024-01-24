using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InfoObject))]  
public class InfoAnimal : MonoBehaviour
{
    private Animals _animals;
    private InfoObject _infoObject;

    private void Awake()
    {
        _animals = GetComponent<Animals>();
        _infoObject = GetComponent<InfoObject>();
        _infoObject.SetActive(false);
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
        _infoObject.SetActive(true);
    }
}
