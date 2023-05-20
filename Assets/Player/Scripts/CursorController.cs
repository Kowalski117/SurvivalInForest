using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    [SerializeField] private FirstPersonController _firstPersonController;

    public void SetCursorVisible(bool visible)
    {
        _firstPersonController.enabled = !visible;
        Cursor.visible = visible;
        Cursor.lockState = visible ? CursorLockMode.None : CursorLockMode.Locked;
    }
}
