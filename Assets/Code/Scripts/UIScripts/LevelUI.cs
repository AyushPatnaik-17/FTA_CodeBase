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
    private Transform _inGameUI, _stepPanel, _levelMainPanel;

    private TextMeshProUGUI _levelName, _timeLeft, _stepName, _stepDescription, _score; 
    private Transform _stepListHolder;
    private Button _homeBtnMain, _startLevelBtn,_markCompleteBtn, _homeBtnStep, _redoBtn, _nextLevelBtn;

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

        _score = _scoreBoard.transform.GetChildWithName(ConstantStrings.SCORE).GetComponent<TextMeshProUGUI>();
        _nextLevelBtn = _scoreBoard.transform.GetChildWithName(ConstantStrings.NEXT_STEP).GetComponent<Button>();

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
                Level currentLevel = _levelManager.CurrentLevel;
                Step currentStep = _levelManager.CurrentStep;
                _score.text = currentStep.CurrentScore.ToString();
                _scoreBoard.SetActive(true);
            }
        );

        _nextLevelBtn.onClick.AddListener
        (
            delegate
            {
                _scoreBoard.SetActive(false);
                _levelManager.NextStep();
                DisplayCurrentStep();
            }
        );

    }
    
    public void DisplayCurrentStep()
    {
        Debug.Log(_levelManager);
        Level currentLevel = _levelManager.CurrentLevel;
        Step currentStep = _levelManager.CurrentStep;

        _stepName.text = currentStep.Name;
        _stepDescription.text = currentStep.Description;
        _timeLeft.text = currentStep.TimeLimit.ToString();
    }
    public void DisplayTimeLeft(int timeLeft)
    {

    }

    public void DisplayLevelInfo(Level currentLevel, int sNum)
    {
        _levelName.text = $"{sNum}. {currentLevel.Name}";
        var steps = currentLevel.Steps;
        int len = steps.Count;
        for (int i = 0; i < len; i++)
        {
            GameObject stepItem = MonoBehaviour.Instantiate(_stepItemPrefab, _stepListHolder);
            TextMeshProUGUI stepName = stepItem.GetComponent<TextMeshProUGUI>();
            stepItem.name = steps[i].Name;
            stepName.text = $"{i+1}. {steps[i].Name}";
        }
    }

    public void HandleStepUI()
    {

    }
}
 