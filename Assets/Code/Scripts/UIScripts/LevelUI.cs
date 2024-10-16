                        using System.Collections;
using System.Collections.Generic;
using AlligatorUtils;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LevelUI
{
    private LevelManager _levelManager;
    private GameObject _mainMenu, _scoreBoard;
    private Transform _inGameUI, _stepPanel, _levelMainPanel, _levelCompletePanel;

    private TextMeshProUGUI _levelName, _timeLeft, _stepName, _stepDescription, _stepScore, _levelScore; 
    private Transform _stepListHolder;
    private Button _homeBtnMain, _startLevelBtn,_markCompleteBtn, _homeBtnStep, _redoBtn, _nextStepBtn, _nextLevelBtn;

    private GameObject _stepItemPrefab;
    
    public LevelUI
    (
        GameObject mainMenu,
        Transform inGameUI 
    )
    { 
        //_mainMenu = mainMenu;
        _inGameUI = inGameUI;
        SetupElements();
    }

    private void SetupElements()
    {   
        _levelManager = MonoBehaviour.FindObjectOfType<LevelManager>(true);

        _levelMainPanel = _inGameUI.GetChildWithName(ConstantStrings.LEVEL_MAIN_PANEL);
        _stepPanel = _inGameUI.GetChildWithName(ConstantStrings.STEP_PANEL);
        _levelCompletePanel = _inGameUI.GetChildWithName(ConstantStrings.LEVEL_COMPLETE);

        _levelName = _levelMainPanel.GetChildWithName(ConstantStrings.LEVEL_NAME).GetComponent<TextMeshProUGUI>();
        _stepListHolder = _levelMainPanel.GetChildWithName(ConstantStrings.STEP_ITEM_HOLDER);
        _homeBtnMain = _levelMainPanel.GetChildWithName(ConstantStrings.HOME).GetComponent<Button>();
        _startLevelBtn = _levelMainPanel.GetChildWithName(ConstantStrings.START_LEVEL).GetComponent<Button>();

        _timeLeft = _stepPanel.GetChildWithName(ConstantStrings.TIME).GetComponent<TextMeshProUGUI>();
        _scoreBoard = _stepPanel.GetChildWithName(ConstantStrings.SCORE_BOARD).gameObject;
        _markCompleteBtn = _stepPanel.GetChildWithName(ConstantStrings.MARK_COMPLETE).GetComponent<Button>();
        _homeBtnStep = _stepPanel.GetChildWithName(ConstantStrings.HOME).GetComponent<Button>();
        _stepName = _stepPanel.GetChildWithName(ConstantStrings.STEP_NAME).GetComponent<TextMeshProUGUI>();
        _stepDescription = _stepPanel.GetChildWithName(ConstantStrings.DESCRIPTION).GetComponent<TextMeshProUGUI>();
        _redoBtn = _stepPanel.GetChildWithName(ConstantStrings.REDO).GetComponent<Button>();

        _stepScore = _scoreBoard.transform.GetChildWithName(ConstantStrings.SCORE).GetComponent<TextMeshProUGUI>();
        _nextStepBtn = _scoreBoard.transform.GetChildWithName(ConstantStrings.NEXT).GetComponent<Button>();

        _levelScore = _levelCompletePanel.GetChildWithName(ConstantStrings.SCORE).GetComponent<TextMeshProUGUI>();
        _nextLevelBtn = _levelCompletePanel.GetChildWithName(ConstantStrings.NEXT).GetComponent<Button>();

        _stepItemPrefab = Resources.Load(GlobalPaths.STEP_LIST_ELEMENT_PATH) as GameObject;

        AssignListeners();
    }

    private void AssignListeners()
    {
        _homeBtnMain.onClick.AddListener
        (
            delegate
            {
                _mainMenu.SetActive(true);
                _inGameUI.gameObject.SetActive(false);
            }
        );

        _startLevelBtn.onClick.AddListener
        (
            delegate
            {
                _levelMainPanel.gameObject.SetActive(false);
                _stepPanel.gameObject.SetActive(true);
                DisplayCurrentStep();
                // show first step
            }
        );

        _markCompleteBtn.onClick.AddListener
        (
            delegate
            {
                _stepScore.text = _levelManager.CurrentStep.CurrentScore.ToString();
                _scoreBoard.SetActive(true);
            }
        );

        _nextStepBtn.onClick.AddListener
        (
            delegate
            {
                _scoreBoard.SetActive(false);
                _levelManager.NextStep();
                DisplayCurrentStep();
            }
        );
        _nextLevelBtn.onClick.AddListener
        (
            delegate
            {
                DispalyNextLevel();
            }
        );

    }
    
    public void DisplayCurrentStep()
    {
        Step currentStep = _levelManager.CurrentStep;
        int sNo = _levelManager.GetStepIndex() + 1;
        _stepName.text = $"{sNo}.{currentStep.Name}";
        _stepDescription.text = currentStep.Description;
        _timeLeft.text = currentStep.TimeLimitInSeconds.ToString();
    }
    public void DisplayTimeLeft(int minutes, int seconds)
    {
        _timeLeft.text = $"{minutes}:{seconds}";
    }

    public void ProceedToNextLevel()
    {
        _stepPanel.gameObject.SetActive(false);
        _levelScore.text = _levelManager.FinalScore.ToString();
        _levelCompletePanel.gameObject.SetActive(true);

    }

    private void DispalyNextLevel()
    {
        DisplayLevelInfo(_levelManager.CurrentLevel, _levelManager.GetLevelIndex() + 1);
        _levelMainPanel.gameObject.SetActive(true);
        _stepPanel.gameObject.SetActive(false);
        _levelCompletePanel.gameObject.SetActive(false);
    }
    public void DisplayLevelInfo(Level currentLevel, int sNum)
    {
        _levelName.text = $"{sNum}. {currentLevel.Name}";
        var steps = currentLevel.Steps;
        int len = steps.Count;

        for (int i = _stepListHolder.childCount - 1; i >= 0; i--)
        {
            GameObject.Destroy(_stepListHolder.GetChild(i).gameObject);
        }

        for (int i = 0; i < len; i++)
        {
            GameObject stepItem = MonoBehaviour.Instantiate(_stepItemPrefab, _stepListHolder);
            TextMeshProUGUI stepName = stepItem.GetComponent<TextMeshProUGUI>();
            stepItem.name = steps[i].Name;
            stepName.text = $"{i+1}. {steps[i].Name}";
        }
    }
}
 