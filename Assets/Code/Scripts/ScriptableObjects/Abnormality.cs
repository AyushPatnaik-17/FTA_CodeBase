using UnityEngine;
using System;

[Serializable]
[CreateAssetMenu(fileName = "NewAbnormality", menuName = "Levels/Abnormalities/New Abnormality")]
public class Abnormality : ScriptableObject
{
    public string Name;
    public string Description;
    public virtual void TriggerAbnormality()
    {
        Debug.Log($"{Name} has occurred!");
    }
}
