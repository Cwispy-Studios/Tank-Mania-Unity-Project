using UnityEditor;
using UnityEngine;

namespace CwispyStudios.TankMania.Stats
{
  [CustomEditor(typeof(StatsGroup), true)]
  public class StatsGroupEditor : Editor
  {
    private SerializedProperty stats;
    private SerializedProperty statsNames;
    private SerializedProperty nameOfStatsFolder;

    private string folderName;

    public virtual void OnEnable()
    {
      stats = serializedObject.FindProperty(nameof(stats));
      statsNames = serializedObject.FindProperty(nameof(statsNames));
      nameOfStatsFolder = serializedObject.FindProperty(nameof(nameOfStatsFolder));

      folderName = nameOfStatsFolder.stringValue;
    }

    public override void OnInspectorGUI()
    {
      base.OnInspectorGUI();

      serializedObject.Update();

      DrawAssetManagementOptions();
    }

    public void DrawAssetManagementOptions()
    {
      EditorGUILayout.Space();

      // Draw a seperator line
      Rect lineRect = EditorGUILayout.GetControlRect(false, 1f);
      EditorGUI.DrawRect(lineRect, Color.gray);

      EditorGUILayout.Space();

      EditorGUILayout.LabelField($"Current Folder Name: {nameOfStatsFolder.stringValue}");
      folderName = EditorGUILayout.TextField("Change Folder Name:", folderName);

      if (GUILayout.Button("Use Asset Name as Folder Name"))
      {
        folderName = serializedObject.targetObject.name;
      }

      Color defaultColour = GUI.color;

      GUI.color = Color.magenta;

      if (GUILayout.Button("Rename Assets"))
      {
        RenameAssets();
      }

      EditorGUILayout.Space();

      GUI.color = Color.yellow;

      if (GUILayout.Button("Create Stat Assets"))
      {
        CreateAssets();
      }

      EditorGUILayout.Space();

      GUI.color = Color.red;

      if (GUILayout.Button("Clear References"))
      {
        ClearReferences();
      }

      GUI.color = Color.white;

      if (GUILayout.Button("Retrieve References From Folder"))
      {
        RetrieveReferences();
      }

      GUI.color = defaultColour;

      serializedObject.ApplyModifiedProperties();
    }

    private void RenameAssets()
    {
      string objectPath = AssetDatabase.GetAssetPath(serializedObject.targetObject);
      string originalFolderPath = objectPath.Replace($"{serializedObject.targetObject.name}.asset", nameOfStatsFolder.stringValue);
      string newFolderPath = originalFolderPath.Replace(nameOfStatsFolder.stringValue, folderName);

      nameOfStatsFolder.stringValue = folderName;

      // Check that paths are different
      if (string.Equals(originalFolderPath, newFolderPath)) return;

      // Check if original folder exists
      if (!AssetDatabase.IsValidFolder(originalFolderPath)) return;

      AssetDatabase.MoveAsset(originalFolderPath, newFolderPath);
    }

    private void CreateAssets()
    {
      // Do not allow creating folder with empty name
      if (string.IsNullOrEmpty(folderName))
      {
        folderName = nameOfStatsFolder.stringValue = serializedObject.targetObject.name;
      }

      string objectPath = AssetDatabase.GetAssetPath(serializedObject.targetObject);
      string folderPath = objectPath.Replace($"{serializedObject.targetObject.name}.asset", folderName);

      nameOfStatsFolder.stringValue = folderName;

      // Check if folder exists, if does not exist, then create folder
      if (!AssetDatabase.IsValidFolder(folderPath))
      {
        string parentFolderPath = folderPath.Replace("/" + serializedObject.targetObject.name, "");
        AssetDatabase.CreateFolder(parentFolderPath, folderName);
      }

      // Check every stat asset...
      for (int i = 0; i < stats.arraySize; ++i)
      {
        SerializedProperty statProperty = stats.GetArrayElementAtIndex(i);

        // Create asset if it does not exist yet
        if (statProperty.objectReferenceValue == null)
        {
          Stat statObject = CreateInstance<Stat>();
          string statName = statsNames.GetArrayElementAtIndex(i).stringValue;
          string assetPath = folderPath + $"/{statName}.asset";
          AssetDatabase.CreateAsset(statObject, assetPath);

          serializedObject.FindProperty(statName).objectReferenceValue = statObject;
        }
      }
    }

    private void ClearReferences()
    {
      // Check every stat asset...
      for (int i = 0; i < stats.arraySize; ++i)
      {
        SerializedProperty statProperty = stats.GetArrayElementAtIndex(i);

        // Check reference exists
        if (statProperty.objectReferenceValue != null)
        {
          string statName = statsNames.GetArrayElementAtIndex(i).stringValue;

          serializedObject.FindProperty(statName).objectReferenceValue = null;
        }
      }
    }

    private void RetrieveReferences()
    {
      string objectPath = AssetDatabase.GetAssetPath(serializedObject.targetObject);
      string folderPath = objectPath.Replace($"{serializedObject.targetObject.name}.asset", folderName);

      // Check if folder exists
      if (!AssetDatabase.IsValidFolder(folderPath)) return;

      // For every stat of this group...
      for (int i = 0; i < stats.arraySize; ++i)
      {
        // Build the path to the desired asset path
        string statName = statsNames.GetArrayElementAtIndex(i).stringValue;
        string assetPath = folderPath + $"/{statName}.asset";

        // Attempt to load the asset and assign it. If it is missing then it will be null.
        Stat statObject = AssetDatabase.LoadAssetAtPath(assetPath, typeof(Stat)) as Stat;
        serializedObject.FindProperty(statName).objectReferenceValue = statObject;
      }
    }
  }
}
