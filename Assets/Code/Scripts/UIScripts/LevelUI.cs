using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUI
{
    private GameObject _mainMenu;
    private Transform _inGameUI;
    public LevelUI(GameObject mainMenu, Transform inGameUI) 
    { 
        _mainMenu = mainMenu;
        _inGameUI = inGameUI;
        SetupElements();
    }

    private void SetupElements()
    {

    }
}
