using System.Collections.Generic;

using UnityEditor;
using UnityEngine;

namespace CwispyStudios.TankMania.Stats
{
  [CustomEditor(typeof(StatsCategory))]
  public class StatsCategoryEditor : Editor
  {
    SerializedProperty statNamesInPaths;
    SerializedProperty autoRetrieve;
    SerializedProperty statsInCategory;

    private void OnEnable()
    {
      statNamesInPaths = serializedObject.FindProperty(nameof(statNamesInPaths));
      autoRetrieve = serializedObject.FindProperty(nameof(autoRetrieve));
      statsInCategory = serializedObject.FindProperty(nameof(statsInCategory));
    }

    public override void OnInspectorGUI()
    {
      serializedObject.Update();

      EditorGUILayout.PropertyField(statNamesInPaths);
      EditorGUILayout.PropertyField(statsInCategory);

      EditorGUILayout.Space();

      EditorGUILayout.PropertyField(autoRetrieve);

      if (autoRetrieve.boolValue) RetrieveStats();

      bool guiEnabled = GUI.enabled;
      GUI.enabled = !autoRetrieve.boolValue;
      if (GUILayout.Button("Retrieve Stats from Selected Paths")) RetrieveStats();
      GUI.enabled = guiEnabled;

      serializedObject.ApplyModifiedProperties();
    }

    private void RetrieveStats()
    {
      List<Stat> allStatObjects = new List<Stat>();

      for (int i = 0; i < statNamesInPaths.arraySize; ++i)
      {
        SerializedProperty statNamesInPath = statNamesInPaths.GetArrayElementAtIndex(i);

        SerializedProperty statName = statNamesInPath.FindPropertyRelative("StatName");
        SerializedProperty findInsidePath = statNamesInPath.FindPropertyRelative("FindInsidePath");

        // Do not search for stat objects if no path is specified
        if (!string.IsNullOrEmpty(findInsidePath.stringValue))
        {
          // Retrieve GUIDs of every Stat object with the name specified
          string[] guids = AssetDatabase.FindAssets(statName.stringValue, new string[] { findInsidePath.stringValue });

          // Then retrieve all the Stat objects from all the found GUIDs and add them to a list
          foreach (string guid in guids)
          {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);

            Stat stat = AssetDatabase.LoadAssetAtPath<Stat>(assetPath);

            if (stat != null) allStatObjects.Add(stat);
          }
        }
      }

      // Clear all the objects in the category
      statsInCategory.ClearArray();
      
      // Assign them from the found Stat objects
      for (int i = 0; i < allStatObjects.Count; ++i)
      {
        statsInCategory.InsertArrayElementAtIndex(i);

        SerializedProperty statProperty = statsInCategory.GetArrayElementAtIndex(i);
        statProperty.objectReferenceValue = allStatObjects[i];
      }
    }
  }
}
