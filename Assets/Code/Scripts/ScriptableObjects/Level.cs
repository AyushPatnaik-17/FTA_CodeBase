using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
[CreateAssetMenu(fileName = "NewLevel", menuName = "Levels/New Level", order = 1)]
public class Level : ScriptableObject
{
    public int LevelNumber, MaxScore, CurrentScore;
    public string Name, CompletionDate;
    public CompletionStatus CompletionStatus;
    public LockStatus LockStatus;
    public List<Step> Steps;
}

