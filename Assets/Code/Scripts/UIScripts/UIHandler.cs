using AlligatorUtils;
using AYellowpaper.SerializedCollections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UIHandler : MonoBehaviour
{
    [Header("HUD Related")]
    public GameObject[] RotaryIndicators = new GameObject[4];
    public TextMeshProUGUI InputText;

    [Header("Level Related")]
    [SerializeField] private GameObject MainMenu;
    [SerializeField] private GameObject LevelPanelMain;
    [SerializeField] private GameObject StepPanel;

    private RotaryUI _rotaryUI;
    private LevelUI _levelUI;
    public RotaryUI RotaryUI
    {
        get
        {
            "Accessing RotaryUI".Print();
            return _rotaryUI;
        }
        set
        {
            _rotaryUI = value;
            "Setting RotaryUI".Print();
        }
    }

    public LevelUI LevelUI
    {
        get
        {
            "Accessing LevelUI".Print();
            return _levelUI;
        }
        set
        {
            _levelUI = value;
            "Setting LevelUI".Print();
        }
    }
    // public static UIHandler Instance;
    private void OnEnable()
    {
        _rotaryUI = new RotaryUI(RotaryIndicators);
        _levelUI = new LevelUI
        (
            MainMenu, 
            this.transform
        ); 
    }
    public void DisplayInputText(string text)
    {
        InputText.text = text;
    }
}
