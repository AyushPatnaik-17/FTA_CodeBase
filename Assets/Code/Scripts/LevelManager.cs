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
    public List<int> ScoreList = new();
    public int MaxScoreThisLevel = 0;
    public float FinalScore = 0f;
    private bool _isLevelCompleted = false;

    public bool IsLevelCompleted
    {
        get => _isLevelCompleted;
        set
        {
            _isLevelCompleted = value;
        }
    }

    public Coroutine ScoreCoroutine = null;

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
            MaxScoreThisLevel = _currentLevel.Steps.Count * 100;
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
        //DontDestroyOnLoad(this);
        
    }

    private void Start()
    {
        AssignStep();
    }

    public void AssignStep()
    {
        //CurrentStep = null;
        CurrentLevel = CurrentLevel ?? Levels[0];
        // _currentStep = _currentStep ?? CurrentLevel.Steps[0];
        _currentStep = CurrentLevel.Steps[0];
        _uiHandler.LevelUI.DisplayLevelInfo(CurrentLevel, GetLevelIndex() + 1);
    }

    public void NextStep()
    {
        if(CurrentStep == CurrentLevel.Steps[CurrentLevel.Steps.Count - 1])
        {
            CurrentStep = GetNextLevelStep();
            _uiHandler.LevelUI.ProceedToNextLevel();
            return;
        }
        CurrentStep = CurrentLevel.Steps[GetStepIndex() + 1];
    }

    private Step GetNextLevelStep()
    {
        CurrentLevel = Levels[GetLevelIndex() + 1];
        CurrentStep = CurrentLevel.Steps[0];
        
        return CurrentStep;
    }

    public int GetLevelIndex()
    {
        return Levels.IndexOf(CurrentLevel);
    }
    public int GetStepIndex()
    {
        return CurrentLevel.Steps.IndexOf(CurrentStep);
    }

    private IEnumerator DecreaseScoreCoroutine()
    {
        float timeLimit = CurrentStep.TimeLimitInSeconds;
        float elapsedTime = CurrentStep.TimeLimitInSeconds;
        float currentScore = MaxScoreThisLevel;

        while (elapsedTime > 0 && !IsLevelCompleted)
        {
            yield return new WaitForSeconds(1f);

            elapsedTime -= 1f;
            float timePercentagePassed = elapsedTime / timeLimit;

            float valToReduce = timePercentagePassed switch
            {
                < 0.30f => 75f,
                < 0.5f => 50f,
                < 0.75f => 25f,
                _ => 0f
            };

            currentScore -= valToReduce;
        }
    }
}
