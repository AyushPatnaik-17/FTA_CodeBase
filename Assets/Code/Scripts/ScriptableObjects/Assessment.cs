using UnityEngine;
using System;

[Serializable]
[CreateAssetMenu(fileName = "NewAssessment", menuName = "Levels/Assessments/New Assessment", order = 1)]
public class Assessment : ScriptableObject
{
    public string Name;
    public bool IsPerformed;
}
