using UnityEditor;
using UnityEngine;

namespace CwispyStudios.TankMania.Stats
{
  [CustomPropertyDrawer(typeof(StatsCategoryStatNamesInPath))]
  public class StatsCategoryStatNamesInPathPropertyDrawer : PropertyDrawer
  {
    private GUIContent folderButtonContent;
    private GUIStyle folderButtonStyle;

    private string selectedFolderPath = "";

    public override float GetPropertyHeight( SerializedProperty property, GUIContent label )
    {
      return base.GetPropertyHeight(property, label) * 2f + EditorGUIUtility.standardVerticalSpacing;
    }

    public override void OnGUI( Rect position, SerializedProperty property, GUIContent label )
    {
      SerializedProperty statName = property.FindPropertyRelative("StatName");
      SerializedProperty findInsidePath = property.FindPropertyRelative("FindInsidePath");

      float originalX = position.x;

      position.height = EditorGUIUtility.singleLineHeight;

      EditorGUI.PropertyField(position, statName, new GUIContent("Stat names"));

      position.x = originalX;
      position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

      string shortenedPath = findInsidePath.stringValue.Replace("Assets/Scriptable Objects/", "");
      EditorGUI.LabelField(position, $"at path: {shortenedPath}");

      position.x -= 25f;
      position.width = 20f;

      if (DrawFolderButton(position))
      {
        string folderPath = GetFolderPath();

        if (!string.IsNullOrEmpty(folderPath))
        {
          selectedFolderPath = folderPath;
        }

        // Prevents error from the OpenFolderPanel method, but also exits the whole method without saving anything
        // That's why we need to store the path elsewhere first
        GUIUtility.ExitGUI();
      }

      if (!string.IsNullOrEmpty(selectedFolderPath) && !string.Equals(findInsidePath.stringValue, selectedFolderPath))
        findInsidePath.stringValue = selectedFolderPath;
    }

    private bool DrawFolderButton( Rect position )
    {
      if (folderButtonContent == null || folderButtonStyle == null)
      {
        folderButtonContent = EditorGUIUtility.IconContent("d_Folder Icon");

        folderButtonStyle = GUI.skin.GetStyle("PaneOptions");
        // Sets the graphic to take up the entire button space
        folderButtonStyle.imagePosition = ImagePosition.ImageOnly;
      }

      return GUI.Button(position, folderButtonContent, folderButtonStyle);
    }

    private string GetFolderPath()
    {
      string selectedFolderPath = EditorUtility.OpenFolderPanel("Folder to retrieve stats from.", "Assets/Scriptable Objects/Stats", "");

      // If no selection was made, the path will be an empty string
      bool noSelectionMade = string.IsNullOrEmpty(selectedFolderPath);

      if (noSelectionMade) return string.Empty;

      // Check if the user selected a path within the project
      bool pathIsInApplicationDataPath = selectedFolderPath.StartsWith(Application.dataPath);

      if (!pathIsInApplicationDataPath)
      {
        EditorUtility.DisplayDialog("File path error!", "Select a folder within the project assets folder!", "Close");

        return string.Empty;
      }

      // Convert the absolute path to relative path
      string relativeFolderPath = "Assets" + selectedFolderPath.Substring(Application.dataPath.Length);

      // Check if the user selected a path where Stat objects would be stored
      bool validPath = relativeFolderPath.StartsWith("Assets/Scriptable Objects/Stats");

      if (!validPath)
      {
        EditorUtility.DisplayDialog("File path error!", "Select a folder within \"Assets/Scriptable Objects/Stats\"!", "Close");

        return string.Empty;
      }

      return relativeFolderPath;
    }
  }
}
