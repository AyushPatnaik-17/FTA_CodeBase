using System.IO;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAbnormality", menuName = "Abnormalities/Abnormality")]
public class Abnormality : ScriptableObject
{
    public string Name;
    public string Description;
    public virtual void TriggerAction()
    {
        Debug.Log($"{Name} has occurred!");
    }
}

public class AbnormalityGenerator: MonoBehaviour
{
    private const string FILE_PATH = "Assets/Code/Unique Abnormalities.txt";
    public const string FOLDER_PATH = "Assets/Code/Abnormalities/";

    [MenuItem("Assets/Create/Abnormalities/Generate Abnormalities from Text", false, 1)]
    public static void GenerateAbnormalitiesFromTextFile()
    {
        if (!File.Exists(FILE_PATH))
        {
            Debug.LogError($"File not found at: {FILE_PATH}");
            return;
        }

        string[] lines = File.ReadAllLines(FILE_PATH);

        if (!AssetDatabase.IsValidFolder(FOLDER_PATH))
        {
            AssetDatabase.CreateFolder("Assets/Code", "Abnormalities");
        }

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
        var assetNameSplit = abnormalityName.Split('.');
        var assetName = assetNameSplit[1].Trim();
        assetName = assetName.Replace('/', '_').Replace('\\', '_').Replace(':', '_');
        abnormality.Name = assetName;

        string assetPath = $"{FOLDER_PATH}{assetName}.asset";
        AssetDatabase.CreateAsset(abnormality, assetPath);

        Debug.Log($"Created abnormality: {assetName}");
    }
}