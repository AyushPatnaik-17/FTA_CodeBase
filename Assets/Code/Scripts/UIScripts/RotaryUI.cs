using UnityEngine;
using AlligatorUtils;
using TMPro;
public class RotaryUI
{
    public GameObject[] RotaryIndicators = new GameObject[4];

    public RotaryUI(GameObject[] rotaryIndicators)
    {
        RotaryIndicators = rotaryIndicators;
    }

    public void SetRotaryAsActive(int index)
    {
        GameObject activeRotary = RotaryIndicators[index];
        foreach (GameObject rotary in RotaryIndicators)
        {
            GameObject fg = rotary.transform.GetChildWithName("fg").gameObject;
            if (rotary == activeRotary)
            {
                fg.SetActive(true);
            }
            else
            {
                fg.SetActive(false);
            }
        }
    }

    public void ChangeRotaryCountText(int index, int value, string description = "none")
    {
        TextMeshProUGUI countText = RotaryIndicators[index].transform.GetChildWithName("Num").GetComponent<TextMeshProUGUI>();
        countText.text = $"{value}\n<size=30>{description}</size>";
    }
}
