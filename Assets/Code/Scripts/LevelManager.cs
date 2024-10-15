using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AlligatorUtils;
using Unity.VisualScripting;


public class LevelManager : MonoBehaviour
{
    private UIHandler _uiHandler;
    public List<Level> Levels = new();
    [SerializeField] private Level _currentLevel;
    [SerializeField] private Step _currentStep = null;
    public Step CurrentStep
    {
        get => _currentStep;
        set
        {
            _currentStep = value;
            // some function call here to reflect on the ui
        }
    }
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
        _uiHandler = FindObjectOfType<UIHandler>(true);
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
        AssignStep();

    }

    private void AssignStep()
    {
        //CurrentStep = null;
        CurrentLevel = CurrentLevel ?? Levels[0];
        // Debug.Log("step name: "+CurrentLevel.Steps[0].Name);
        // _currentStep = _currentStep ?? CurrentLevel.Steps[0];
        _currentStep = CurrentLevel.Steps[0];
        Debug.Log("step name: "+_currentStep.Name);
        _uiHandler.LevelUI.DisplayLevelInfo(CurrentLevel, 1);
    }

    public void NextStep()
    {
        CurrentStep = CurrentStep == CurrentLevel.Steps[CurrentLevel.Steps.Count - 1] ? GetNextLevelStep() : CurrentLevel.Steps[CurrentLevel.Steps.IndexOf(CurrentStep) + 1];
    }

    private Step GetNextLevelStep()
    {
        CurrentLevel = Levels[Levels.IndexOf(CurrentLevel) + 1];
        CurrentStep = CurrentLevel.Steps[0];
        
        return CurrentStep;
    }
}
