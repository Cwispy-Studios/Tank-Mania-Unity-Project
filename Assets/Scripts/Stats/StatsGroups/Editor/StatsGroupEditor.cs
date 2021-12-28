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

    private string currentFolderPath;

    public virtual void OnEnable()
    {
      Initialise();
    }

    private void Initialise()
    {
      stats = serializedObject.FindProperty(nameof(stats));
      statsNames = serializedObject.FindProperty(nameof(statsNames));
      nameOfStatsFolder = serializedObject.FindProperty(nameof(nameOfStatsFolder));

      // This will be called on a new object, default name is the asset name
      if (string.IsNullOrEmpty(nameOfStatsFolder.stringValue))
      {
        nameOfStatsFolder.stringValue = serializedObject.targetObject.name;

        serializedObject.ApplyModifiedProperties();
      }
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

      ValidateAssets();

      Color defaultColour = GUI.color;

      GUI.color = Color.magenta;

      GUIContent content = new GUIContent();
      content.text = "Create Stat Assets";
      content.tooltip = "Creates an asset for every missing Stat reference in a folder with the same name as this asset's name." +
        " Also creates the folder if it does not exist.";

      if (GUILayout.Button(content))
      {
        CreateAssets();
      }

      EditorGUILayout.Space();

      GUI.color = Color.white;

      content.text = "Rename Assets";
      content.tooltip = "Checks every Stat reference if the asset name is the same as the variable name and renames them." +
        " Also renames the folder to match this asset's name if it exists.";

      if (GUILayout.Button(content))
      {
        RenameAssets();
      }

      content.text = "Retrieve References From Folder";
      content.tooltip = "Searches for a folder with the same name as this asset. " +
        "If it exists, searches for Stat assets with the same names as the variables in this StatsGroup and references them.";

      if (GUILayout.Button(content))
      {
        RetrieveReferences();
      }

      EditorGUILayout.Space();

      GUI.color = Color.yellow;

      content.text = "Clear References";
      content.tooltip = "Removes the reference to every Stat object but does not delete them.";

      if (GUILayout.Button(content))
      {
        ClearReferences();
      }

      GUI.color = defaultColour;

      serializedObject.ApplyModifiedProperties();
    }

    private void ValidateAssets()
    {
      currentFolderPath = GetCurrentFolderPath();

      bool hasAssetReference = HasStatsReference();

      if (hasAssetReference)
      {
        bool folderExists = AssetDatabase.IsValidFolder(currentFolderPath);

        if (!folderExists)
        {
          EditorGUILayout.HelpBox($"Warning! There are assets referenced but the folder \"{nameOfStatsFolder.stringValue}\"" +
            $" cannot be found! Has the folder been renamed by another StatsGroup, or did you rename it manually?", MessageType.Warning);
        }

        bool statsInCorrectFolder = StatsInCorrectFolder();

        if (!statsInCorrectFolder)
        {
          EditorGUILayout.HelpBox($"Warning! One or more Stat assets are in a different folder from \"{nameOfStatsFolder.stringValue}\"!", 
            MessageType.Warning);
        }
      }
    }

    private string GetCurrentFolderPath()
    {
      // Get the path to the asset
      string objectPath = AssetDatabase.GetAssetPath(serializedObject.targetObject);
      // Find the current folder path from the asset path
      string currentFolderPath = objectPath.Replace($"{serializedObject.targetObject.name}.asset", nameOfStatsFolder.stringValue);

      return currentFolderPath;
    }

    private bool HasStatsReference()
    {
      for (int i = 0; i < stats.arraySize; ++i)
      {
        SerializedProperty statProperty = stats.GetArrayElementAtIndex(i);

        // There is a reference to a stat
        if (statProperty.objectReferenceValue != null)
        {
          return true;
        }
      }

      return false;
    }

    private bool StatsInCorrectFolder()
    {
      for (int i = 0; i < stats.arraySize; ++i)
      {
        SerializedProperty statProperty = stats.GetArrayElementAtIndex(i);
        Object objectReferenceValue = statProperty.objectReferenceValue;

        // There is a reference to a stat
        if (objectReferenceValue != null)
        {
          string propertyPath = AssetDatabase.GetAssetPath(objectReferenceValue);
          string replacedString = $"/{objectReferenceValue.name}.asset";
          propertyPath = propertyPath.Replace(replacedString, string.Empty);

          bool differentFolder = !propertyPath.EndsWith(nameOfStatsFolder.stringValue);

          if (differentFolder)
          {
            return false;
          }
        }
      }

      return true;
    }

    private void RenameAssets()
    {
      RenameFolder();
      RenameStats();
    }

    private void RenameFolder()
    {
      // If no folder exists, there is nothing to be done
      if (!AssetDatabase.IsValidFolder(currentFolderPath))
      {
        EditorUtility.DisplayDialog("Cannot rename folder!", $"Folder \"{nameOfStatsFolder.stringValue}\" does not exist.", "Close");
        return;
      }

      // Get the new folder path we want (this will be the same as the asset)
      string newFolderPath = currentFolderPath.Replace(nameOfStatsFolder.stringValue, serializedObject.targetObject.name);

      // Overwrite the property, folder name should always be asset name
      nameOfStatsFolder.stringValue = serializedObject.targetObject.name;

      // Check that paths are different and original folder still exists
      if (!string.Equals(currentFolderPath, newFolderPath))
      {
        AssetDatabase.MoveAsset(currentFolderPath, newFolderPath);
      }
    }

    private void RenameStats()
    {
      // Check asset names if they still match variable names
      for (int i = 0; i < stats.arraySize; ++i)
      {
        SerializedProperty statProperty = stats.GetArrayElementAtIndex(i);

        // There is a reference to a stat
        if (statProperty.objectReferenceValue != null)
        {
          string currentStatName = statProperty.objectReferenceValue.name;
          string desiredStatName = statsNames.GetArrayElementAtIndex(i).stringValue;

          // Check if the asset name is still the same as the variable name
          if (!string.Equals(currentStatName, desiredStatName))
          {
            string assetPath = AssetDatabase.GetAssetPath(statProperty.objectReferenceValue);
            string desiredPath = assetPath.Replace(currentStatName, desiredStatName);

            AssetDatabase.MoveAsset(assetPath, desiredPath);
          }
        }
      }
    }

    private void CreateAssets()
    {
      // Check if folder exists, if does not exist, then create folder
      if (!AssetDatabase.IsValidFolder(currentFolderPath))
      {
        string parentFolderPath = currentFolderPath.Replace("/" + serializedObject.targetObject.name, "");
        AssetDatabase.CreateFolder(parentFolderPath, nameOfStatsFolder.stringValue);
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
          string assetPath = currentFolderPath + $"/{statName}.asset";
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
      // Check if folder exists
      if (!AssetDatabase.IsValidFolder(currentFolderPath)) return;

      // For every stat of this group...
      for (int i = 0; i < stats.arraySize; ++i)
      {
        // Build the path to the desired asset path
        string statName = statsNames.GetArrayElementAtIndex(i).stringValue;
        string assetPath = currentFolderPath + $"/{statName}.asset";

        // Attempt to load the asset and assign it. If it is missing then it will be null.
        Stat statObject = AssetDatabase.LoadAssetAtPath(assetPath, typeof(Stat)) as Stat;
        serializedObject.FindProperty(statName).objectReferenceValue = statObject;
      }
    }
  }
}
