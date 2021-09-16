//using System.Collections.Generic;
//using System.Reflection;

//using UnityEditor;
//using UnityEngine;

//namespace CwispyStudios.TankMania.Stats
//{
//  [CustomEditor(typeof(StatCategory))]
//  public class StatCategoryEditor : Editor
//  {
//    private List<VariableStat> statsList = new List<VariableStat>();
//    private List<string> statsNameList = new List<string>();

//    // Cache properties
//    private SerializedProperty statsGroupObjects;
//    private SerializedProperty variableStatIndex;
//    private SerializedProperty statModifiers;
//    private SerializedProperty variableName;

//    private bool arrayIsValid = false;
//    private Object statGroupReference;

//    private void OnEnable()
//    {
//      statsGroupObjects = serializedObject.FindProperty(nameof(statsGroupObjects));
//      variableStatIndex = serializedObject.FindProperty(nameof(variableStatIndex));
//      statModifiers = serializedObject.FindProperty(nameof(statModifiers));
//      variableName = serializedObject.FindProperty(nameof(variableName));

//      CheckArrayValidity();
//    }

//    public override void OnInspectorGUI()
//    {
//      EditorGUILayout.HelpBox("NOTE: Stat groups must match the first object of the array.", MessageType.Info);

//      serializedObject.Update();

//      EditorGUI.BeginChangeCheck();

//      // Create property field to input StatGroup objects
//      EditorGUILayout.PropertyField(statsGroupObjects);

//      // Check for changes, then check if the array is valid
//      if (EditorGUI.EndChangeCheck())
//        CheckArrayValidity();

//      if (statGroupReference != null)
//      {
//        if (!arrayIsValid)
//        {
//          EditorGUILayout.HelpBox("ERROR! Duplicate, missing, or Stat Group type mismatch found in list!", MessageType.Error);
//          variableName.stringValue = string.Empty;
//        }

//        else
//        {
//          // Selector for variable name
//          variableStatIndex.intValue = 
//            EditorGUILayout.Popup(Mathf.Clamp(variableStatIndex.intValue, 0, statsNameList.Count - 1), statsNameList.ToArray());
//          variableName.stringValue = statsNameList[variableStatIndex.intValue];
//        }
//      }

//      else variableName.stringValue = string.Empty;

//      EditorGUILayout.HelpBox($"Stat name: {variableName.stringValue}", MessageType.None);

//      EditorGUILayout.Space();

//      Rect lineRect = EditorGUILayout.GetControlRect(false, 1f);
//      EditorGUI.DrawRect(lineRect, Color.gray);

//      EditorGUILayout.Space();

//      EditorGUILayout.PropertyField(statModifiers);

//      serializedObject.ApplyModifiedProperties();
//    }

//    private void CheckArrayValidity()
//    {
//      if (statsGroupObjects.arraySize > 0)
//      {
//        // Get the object from the first element of the array and use that to populate the stats list
//        statGroupReference = statsGroupObjects.GetArrayElementAtIndex(0).objectReferenceValue;

//        if (statGroupReference != null)
//        {
//          PopulateStatsLists(statGroupReference);
//          arrayIsValid = CheckArrayContentsValidity(statGroupReference);
//        }
//      }

//      // Empty list, clear all saved references to the reference object
//      else ClearStatsLists();
//    }

//    private void PopulateStatsLists( Object objectReference )
//    {
//      statsNameList.Clear();
//      statsList.Clear();
//      GetStatVariables(objectReference);
//    }

//    private void GetStatVariables( object statsGroup )
//    {
//      // Gets the list of all fields in the object.
//      FieldInfo[] fields = statsGroup.GetType().GetFields();

//      foreach (FieldInfo field in fields)
//      {
//        // Find the fields that are VariableStats
//        if (field.FieldType == typeof(FloatStat) || field.FieldType == typeof(IntStat))
//        {
//          statsNameList.Add(field.Name);
//          statsList.Add(field.GetValue(statsGroup) as VariableStat);
//        }

//        else if (field.FieldType.IsClass)
//          GetStatVariables(field.GetValue(statsGroup));
//      }
//    }

//    private void ClearStatsLists()
//    {
//      statsNameList.Clear();
//      statsList.Clear();

//      variableName.stringValue = string.Empty;
//      statGroupReference = null;
//    }

//    /// <summary>
//    /// Checks the array list of StatGroup objects if they all belong to the same derived class.
//    /// </summary>
//    /// <returns></returns>
//    private bool CheckArrayContentsValidity( Object objectReference )
//    {
//      // Only one object in the array, there won't be any mismatch
//      if (statsGroupObjects.arraySize == 1) return true;

//      System.Type referenceType = objectReference.GetType();

//      for (int i = 1; i < statsGroupObjects.arraySize; ++i)
//      {
//        Object compare = statsGroupObjects.GetArrayElementAtIndex(i).objectReferenceValue;

//        // Duplicate objects, missing objects, or mismatched types
//        if (compare == objectReference) return false;
//        if (compare == null) return false;
//        if (compare.GetType() != referenceType) return false;
//      }

//      return true;
//    }
//  }
//}
