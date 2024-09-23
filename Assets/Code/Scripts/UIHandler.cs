using System.Collections;
using System.Collections.Generic;
using AlligatorUtils;
using AYellowpaper.SerializedCollections;
using TMPro;
using UnityEngine;

public class UIHandler : MonoBehaviour
{
    public GameObject[] RotaryIndicators = new GameObject[4];
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
    public void ChangeRotaryCountText(int index, int value)
    {
        //GameObject fg = RotaryIndicators[index].transform.GetChildWithName("fg").gameObject;
        TextMeshProUGUI countText = RotaryIndicators[index].transform.GetChildWithName("Num").GetComponent<TextMeshProUGUI>();

        //fg.SetActive(true);
        countText.text = value.ToString();
    }

    public void SetRotaryAsActive(int index)
    {
        GameObject activeRotary = RotaryIndicators[index];
        foreach (GameObject rotary in RotaryIndicators)
        {
            GameObject fg = rotary.transform.GetChildWithName("fg").gameObject;
            if(rotary == activeRotary)
            {
                fg.SetActive(true);
            }
            else
            {
                fg.SetActive(false);
            }
        }
        
        
    }
}
