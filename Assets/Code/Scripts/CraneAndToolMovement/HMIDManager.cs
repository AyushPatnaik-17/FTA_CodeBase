using System.Collections.Generic;
using AlligatorUtils;
using AYellowpaper.SerializedCollections;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class HMIDManager : MonoBehaviour
{
    public const string PREFIX = "Do you want to select ";
    public List<Abnormality> Abnormalities;
    public SerializedDictionary<Button, Abnormality> AbnormalityButtonPairs = new();
    public Transform ButtonsParent;
    public GameObject ConfirmationPanel;
    public Button ButtonPrefab, YesBtn, NoBtn;
    public TextMeshProUGUI ConfirmationText;

    private Abnormality _currentAbnormality,_selectedAbnormality;
    private void Awake()
    {
        foreach(Abnormality abnormality in  Abnormalities)
        {
            Button button = Instantiate(ButtonPrefab,ButtonsParent);
            TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
            button.name = buttonText.text = abnormality.Name;
            AbnormalityButtonPairs.Add(button, abnormality);
            button.onClick.AddListener(() =>
            {
                _selectedAbnormality = AbnormalityButtonPairs[button];
                ConfirmationText.text = $"{PREFIX}{_selectedAbnormality.Name}"; 
                ConfirmationPanel.SetActive(true);
            });
        }

        YesBtn.onClick.AddListener(delegate
        {
            CheckAbnormality();
            ConfirmationPanel.SetActive(false);
        });
        NoBtn.onClick.AddListener(delegate
        {
            ConfirmationPanel.SetActive(false);
        }); 
    }


    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            TriggerRandomAbnormality();
        }
    }
    public void TriggerRandomAbnormality()
    {
        int index = Random.Range(0, Abnormalities.Count);
        _currentAbnormality = Abnormalities[index];
        _currentAbnormality.TriggerAction();
    }

    // public bool IsAbnormalityActive()
    // {
    //     return CurrentAbnormality != null;
    // }
    public void CheckAbnormality()
    {
        if (_currentAbnormality == _selectedAbnormality)
        {
            "Correct Abnormality Detected!".Print("","green");
        }
        else if (_currentAbnormality == null)
        {
            "No abnormality has occurred.".Print("","red");
        }
        else
        {
            "Incorrect Abnormality Selected!".Print("","red");
        }
    }

#if UNITY_EDITOR
    [ContextMenu("Populate Abnormalities List")]
    private void PopulateAbnormalities()
    {
        Abnormalities.Clear();

        string[] guids = AssetDatabase.FindAssets("t:Abnormality", new[] { "Assets/Code/Abnormalities" });
        foreach (string guid in guids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            Abnormality abnormality = AssetDatabase.LoadAssetAtPath<Abnormality>(assetPath);
            if (abnormality != null)
            {
                Abnormalities.Add(abnormality);
            }
        }

        Debug.Log($"Loaded {Abnormalities.Count} abnormalities.");
    }
#endif
}

