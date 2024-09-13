using System.Collections.Generic;
using AlligatorUtils;
using AYellowpaper.SerializedCollections;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class HMIDManager : MonoBehaviour
{
    public List<Abnormality> Abnormalities;
    public SerializedDictionary<Button, Abnormality> AbnormalityButtonPairs = new();
    public Abnormality CurrentAbnormality;

    public Transform ButtonsParent;
    public Button ButtonPrefab;

    private void Awake()
    {
        foreach(Abnormality abnormality in  Abnormalities)
        {
            Button button = Instantiate(ButtonPrefab,ButtonsParent);
            TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = abnormality.Name;
            AbnormalityButtonPairs.Add(button, abnormality);
            button.onClick.AddListener(() => CheckAbnormality(AbnormalityButtonPairs[button]));
        }
        AbnormalityButtonPairs.Keys.Print();
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
        CurrentAbnormality = Abnormalities[index];
        CurrentAbnormality.TriggerAction();
    }

    public bool IsAbnormalityActive()
    {
        return CurrentAbnormality != null;
    }
    public void CheckAbnormality(Abnormality selectedAbnormality)
    {
        if (CurrentAbnormality == selectedAbnormality)
        {
            "Correct Abnormality Detected!".Print("","green");
        }
        else if (!IsAbnormalityActive())
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

