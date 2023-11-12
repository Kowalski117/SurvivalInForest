using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardScreen : ScreenUI
{
    private void Awake()
    {
        CloseScreen();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }

    protected override void ExitButtonClick()
    {
        base.ExitButtonClick();
        CloseScreen();
    }
}
