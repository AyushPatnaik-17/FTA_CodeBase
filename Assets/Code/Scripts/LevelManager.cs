using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AlligatorUtils;


public class LevelManager : MonoBehaviour
{
    public List<Level> Levels = new();
    private Level _currentLevel;
    public Level CurrentLevel
    {
        get => _currentLevel;
        set
        {
            _currentLevel = value;
            // some function call here to reflect on the ui
        }
    }

    public static LevelManager Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        CurrentLevel = Levels[0];
        
    }
}
