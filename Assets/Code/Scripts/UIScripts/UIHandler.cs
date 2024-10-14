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

    private RotaryUI _rotatoryUI;
    public RotaryUI RotaryUI
    {
        get
        {
            "Accessing RotaryUI".Print();
            return _rotatoryUI;
        }
        set
        {
            _rotatoryUI = value;
            "Setting RotaryUI".Print();
        }
    }
    public static UIHandler Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); 
        }
    }

    private void Start()
    {
        RotaryUI = new RotaryUI(RotaryIndicators);    
    }
    public void DisplayInputText(string text)
    {
        InputText.text = text;
    }
}
