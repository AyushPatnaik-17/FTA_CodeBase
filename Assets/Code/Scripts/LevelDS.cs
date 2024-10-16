using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using AlligatorUtils;

public enum CompletionStatus { Completed, NotComplete }
public enum LockStatus { Locked, Unlocked }

[Serializable]
public class Step
{
    public string Name, Description;
    public int MaxScore, CurrentScore;
    public float TimeLimitInSeconds;
    public List<Assessment> Assessments;
    public List<Abnormality> Abnormalities;
    public CompletionStatus CompletionStatus;
}

// public class LevelGenerator : MonoBehaviour
// {
//     private const string FILE_PATH = "Assets/Code/LevelsDetails.xlxs";
//     [MenuItem("Assets/Create/Levels/Generate Levels", false, 1)]
//     public static void GenerateLevelsFromTextFile()
//     {
//         if (!File.Exists(FILE_PATH))
//         {
//             Debug.LogError($"File not found at: {FILE_PATH}");
//             return;
//         }
//         if (!AssetDatabase.IsValidFolder(GlobalPaths.LEVELS_PATH))
//         {
//             AssetDatabase.CreateFolder("Assets/Code", "Abnormalities");
//         }
//     }
// }

public class AssessmentGenerator : MonoBehaviour
{
    [MenuItem("Assets/Create/Levels/Assessments/Generate Assessments from Text", false, 1)]
    public static void GenerateAssessmentsFromTextFile()
    {
        if (!File.Exists(GlobalPaths.UNIQUE_ASSESSMENTS))
        {
            Debug.LogError($"File not found at: {GlobalPaths.UNIQUE_ASSESSMENTS}");
            return;
        }

        if (!AssetDatabase.IsValidFolder(GlobalPaths.ASSESSMENTS_PATH))
        {
            AssetDatabase.CreateFolder(GlobalPaths.LEVEL_RELATED_FOLDER, "Assessments");
        }

        string[] lines = File.ReadAllLines(GlobalPaths.UNIQUE_ASSESSMENTS);

        foreach (string line in lines)
        {
            if (!string.IsNullOrWhiteSpace(line))
            {
                CreateAssessment(line);
            }
        }
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
    private static void CreateAssessment(string assessmentName)
    {
        Assessment assessment = ScriptableObject.CreateInstance<Assessment>();
        var assetName = assessmentName.Trim();
        assetName = assetName.Replace('/', '_').Replace('\\', '_').Replace(':', '_');
        assessment.Name = assetName;

        string assetPath = $"{GlobalPaths.ASSESSMENTS_PATH}{assetName}.asset";
        AssetDatabase.CreateAsset(assessment, assetPath);
        assetName.Print("Created Assessment: ");
    }
}
public class AbnormalityGenerator: MonoBehaviour
{
    [MenuItem("Assets/Create/Levels/Abnormalities/Generate Abnormalities from Text", false, 1)]
    public static void GenerateAbnormalitiesFromTextFile()
    {
        if (!File.Exists(GlobalPaths.UNIQUE_ABNORMALITIES))
        {
            Debug.LogError($"File not found at: {GlobalPaths.UNIQUE_ABNORMALITIES}");
            return;
        }


        if (!AssetDatabase.IsValidFolder(GlobalPaths.ABNORMALITIES_PATH))
        {
            AssetDatabase.CreateFolder(GlobalPaths.LEVEL_RELATED_FOLDER, "Abnormalities");
        }

        string[] lines = File.ReadAllLines(GlobalPaths.UNIQUE_ABNORMALITIES);

        foreach (string line in lines)
        {
            if (!string.IsNullOrWhiteSpace(line))
            {
                CreateAbnormality(line);
            }
        }
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private static void CreateAbnormality(string abnormalityName)
    {
        Abnormality abnormality = ScriptableObject.CreateInstance<Abnormality>();
        // var assetNameSplit = abnormalityName.Split('.');
        var assetName = abnormalityName.Trim();
        assetName = assetName.Replace('/', '_').Replace('\\', '_').Replace(':', '_');
        abnormality.Name = assetName;

        string assetPath = $"{GlobalPaths.ABNORMALITIES_PATH}{assetName}.asset";
        AssetDatabase.CreateAsset(abnormality, assetPath);
        assetName.Print("Created Abnormality: ");   
    }
}


